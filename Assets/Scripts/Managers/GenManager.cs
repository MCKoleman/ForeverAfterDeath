using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenManager : Singleton<GenManager>
{
    [SerializeField]
    private GameObject baseRoomPrefab;
    [SerializeField]
    private LevelRoomList roomList;
    [SerializeField]
    private LevelContentList contentList;
    private GameObject startRoom;

    [Header("Runtime info")]
    private LevelRoom currentRoom;
    [SerializeField]
    private float enemyDifficultyMod = 1.25f;
    private int numRooms = 0;
    [SerializeField]
    private int curLevel = 0;
    [SerializeField]
    private int enemiesKilled = 0;

    [Header("Level Info")]
    [SerializeField]
    private Vector2 roomSize = Vector2.zero;
    [SerializeField]
    private Vector2Int blCoord = new Vector2Int(int.MaxValue, int.MaxValue);
    [SerializeField]
    private Vector2Int trCoord = new Vector2Int(int.MinValue, int.MinValue);
    private CameraController cameraController;

    private Dictionary<Vector2Int, RoomContentNodes> contentNodes = new Dictionary<Vector2Int, RoomContentNodes>();
    private Dictionary<string, RoomNode> roomNodes = new Dictionary<string, RoomNode>();
    private List<RoomNode> spawnNodes = new List<RoomNode>();

    private bool isFullyGenerated = false;

    // Initializer for the singleton. Should only be called from GameManager
    public void Init()
    {
        curLevel = 1;
        enemiesKilled = 0;
        //UIManager.Instance.SetLevelNum(curLevel);
    }

    public void StartLevel()
    {
        cameraController = Camera.main?.GetComponent<CameraController>();

        startRoom = Instantiate(baseRoomPrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelGeoHolder);
        StartCoroutine(AsyncGeneration());
    }

    private IEnumerator AsyncGeneration()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.GENERATING_LEVEL);
        //UIManager.Instance.EnableLoadingScreen(true);

        // Start generation
        isFullyGenerated = false;
        GenerateLevel(startRoom);

        yield return new WaitUntil(() => isFullyGenerated);
        GameManager.Instance.SetGameState(GameManager.GameState.LOADING_LEVEL);

        // Communicate ending of generation to GameStateManager
        GameManager.Instance.StartGame();
        GameManager.Instance.SetGameState(GameManager.GameState.IN_GAME);
        //UIManager.Instance.EnableLoadingScreen(false);
    }

    public void GenerateLevel(GameObject startRoom)
    {
        GameObject latestRoom = startRoom.gameObject;
        numRooms = 1;
        blCoord = new Vector2Int(int.MaxValue, int.MaxValue);
        trCoord = new Vector2Int(int.MinValue, int.MinValue);

        // Clear previous dungeon
        roomNodes.Clear();
        contentNodes.Clear();
        spawnNodes.Clear();

        // Add all start rooms
        var startRooms = startRoom.GetComponentsInChildren<LevelRoom>();
        foreach (LevelRoom room in startRooms)
        {
            AddNodesToDict(roomNodes, spawnNodes, room.roomNodes);
            roomSize = room.GetSize();
        }

        while (spawnNodes.Count > 0)
        {
            // Pop the first spawnNode
            RoomNode tempNode = spawnNodes[0];

            // Find a random room that meets the spawn requirements
            GameObject tempRoomPrefab = SelectRoomPrefab(roomList.GetMinRoomCount(), roomList.GetMaxRoomCount(), roomList.GetPrefRoomCount(), tempNode);
            //Debug.Log($"Selected room [{tempRoomPrefab}] for flag [{tempNode.ReqFlag}]");

            // Spawn the room chosen
            if (tempRoomPrefab != null)
            {
                latestRoom = Instantiate(tempRoomPrefab, tempNode.transform.position, Quaternion.identity, PrefabManager.Instance.levelGeoHolder);

                // Update dungeon room information
                LevelRoom tempDungeonRoom = latestRoom.GetComponent<LevelRoom>();
                roomSize = tempDungeonRoom.GetSize();
                tempDungeonRoom.roomNum = numRooms;
                tempDungeonRoom.roomPos = new Vector2Int(
                    Mathf.FloorToInt(latestRoom.transform.position.x / roomSize.x),
                    Mathf.FloorToInt(latestRoom.transform.position.z / roomSize.y));

                AddNodesToDict(roomNodes, spawnNodes, tempDungeonRoom.roomNodes);

                // Find bottom and top bounds of the map
                blCoord = new Vector2Int(Mathf.Min(blCoord.x, tempDungeonRoom.roomPos.x), Mathf.Min(blCoord.y, tempDungeonRoom.roomPos.y));
                trCoord = new Vector2Int(Mathf.Max(trCoord.x, tempDungeonRoom.roomPos.x), Mathf.Max(trCoord.y, tempDungeonRoom.roomPos.y));

                // Increment the current room number
                numRooms++;
            }
            // Mark the node as completed and remove it from the list
            tempNode.hasSpawned = true;
            spawnNodes.RemoveAt(0);

            // Remove all nodes that have been exhausted
            TrimSpawnedNodes(spawnNodes);
        }
        SpawnExit(latestRoom);
        isFullyGenerated = true;
    }

    // Sets the current room to the given room
    public void SetCurrentRoom(LevelRoom newRoom)
    {
        currentRoom = newRoom;

        // Set camera to follow new room
        if (newRoom != null)
            cameraController.SetRoom(newRoom);
        
    }

    // Selects a room prefab to spawn that meets the current criteria
    private GameObject SelectRoomPrefab(int minRooms, int maxRooms, int prefRooms, RoomNode node)
    {
        GameObject tempPrefab = null;

        // First check if a special room should be attempted (either it is required, or there is space and a random chance succeeds)
        if (GlobalVars.DoesRequireSpecialReq(node.ReqFlag) || (prefRooms > numRooms + spawnNodes.Count && roomList.ShouldAttemptSpecialRoom()))
            tempPrefab = roomList.GetRandomSpecialRoomByFlag(node.ReqFlag);

        // If a suitable special room was not found or searched for, keep going
        if (tempPrefab != null)
            return tempPrefab;

        // If the minimum number of rooms hasn't been met yet and there are less spawn nodes than needed rooms, spawn max rooms
        if (minRooms > numRooms + spawnNodes.Count)
            return roomList.GetRandomRoomByFlag(GlobalVars.LockInverseFlagReqs(node.ReqFlag));

        // If the preferred number of rooms hasn't been met yet, spawn with inverted spawn rates
        else if (prefRooms > numRooms + spawnNodes.Count)
            return roomList.GetInverseRandomRoomByFlag(node.ReqFlag);

        // If the preferred number of rooms has been met, spawn normally until approaching max rooms
        else if (maxRooms > numRooms + spawnNodes.Count)
            return roomList.GetRandomRoomByFlag(node.ReqFlag);

        // If approaching max rooms, force stop
        else
            return roomList.GetRandomRoomByFlag(GlobalVars.LockFlagReqs(node.ReqFlag));
    }

    // Remove all nodes that have already spawned from the list
    private void TrimSpawnedNodes(List<RoomNode> spawnNodes)
    {
        // Find all nodes that have already spawned or don't need to spawn
        for (int i = spawnNodes.Count - 1; i >= 0; i--)
        {
            // If the node doesn't need to spawn or has already spawned, flag it for deletion
            if (!spawnNodes[i].shouldSpawn || spawnNodes[i].hasSpawned)
                spawnNodes.RemoveAt(i);
        }
    }

    // Adds all the nodes from the given list to the dictionary, combining duplicates
    private void AddNodesToDict(Dictionary<string, RoomNode> roomNodes, List<RoomNode> spawnNodes, List<RoomNode> roomList)
    {
        // Check each node against both dicts before adding it
        for (int i = 0; i < roomList.Count; i++)
        {
            // Get the key of the current node
            string tempKey = roomList[i].GetKey();
            RoomNode tempNode = roomList[i];

            // If the list already has a node with the given key, combine them
            if (roomNodes.ContainsKey(tempKey))
            {
                roomNodes[tempKey].CombineNodes(tempNode);
                tempNode = roomNodes[tempKey];
            }
            // If the list does not have a node with the given key, add it
            else
            {
                roomNodes.Add(tempKey, tempNode);
            }

            // If the room is valid for spawning, add it to the spawn list
            if (!tempNode.hasSpawned && tempNode.shouldSpawn && !spawnNodes.Contains(tempNode))
            {
                spawnNodes.Add(tempNode);
            }
        }
    }

    // Spawns content for the given room
    public void SpawnContent(ContentNode node)
    {
        // Find random content for the room
        LevelContentList.ContentGameObject tempContent = contentList.GetRandomContent(node.GetParentRoom().roomNum / (float)numRooms, node.nodePlace);
        if (tempContent.obj != null)
        {
            switch (tempContent.content)
            {
                case GlobalVars.ContentType.HAZARD:
                    Instantiate(tempContent.obj, node.transform.position, node.transform.rotation, PrefabManager.Instance.levelObjHolder);
                    break;
                case GlobalVars.ContentType.TREASURE:
                    Instantiate(tempContent.obj, node.transform.position, node.transform.rotation, PrefabManager.Instance.levelObjHolder);
                    break;
                case GlobalVars.ContentType.ENEMY:
                    Instantiate(tempContent.obj, node.transform.position, node.transform.rotation, PrefabManager.Instance.enemyHolder);
                    break;
                case GlobalVars.ContentType.WALL:
                case GlobalVars.ContentType.NOTHING:
                default:
                    Instantiate(tempContent.obj, node.transform.position, node.transform.rotation, node.GetParentRoom().transform);
                    break;
            }
        }
    }

    // Spawns a way to exit the dungeon
    private void SpawnExit(GameObject latestRoom)
    {
       Instantiate(PrefabManager.Instance.exitPrefab, latestRoom.transform.position, Quaternion.identity, PrefabManager.Instance.levelGeoHolder);
    }

    public void AddContentNode(ContentNode newNode)
    {
        // If adding content for a new room, create a new room to store the info
        Vector2Int key = newNode.GetRoomPos();
        if (!contentNodes.ContainsKey(key))
            contentNodes.Add(key, new RoomContentNodes(newNode));
        else
            contentNodes[key].AddContentNode(newNode);
    }

    public float GetDiffMod() { return Mathf.Max(1.0f, Mathf.Pow(enemyDifficultyMod, curLevel - 1)); }

    public void IncLevelNum() { curLevel++; }
    public void IncEnemiesKilled() { enemiesKilled++; }
    public int GetLevelNum() { return curLevel; }
    public int GetEnemiesKilled() { return enemiesKilled; }

    public void ResetScore()
    {
        curLevel = 1;
        enemiesKilled = 0;
    }
}
