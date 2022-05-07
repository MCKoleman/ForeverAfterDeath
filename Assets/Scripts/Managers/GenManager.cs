using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenManager : Singleton<GenManager>
{
    [SerializeField]
    private GameObject baseRoomPrefab;
    private LevelRoom startRoom;

    [Header("Runtime info")]
    private LevelRoom currentRoom;
    private int numRooms = 0;
    private int curLevel = 0;

    private Dictionary<Vector2Int, ContentNode> contentNodes = new Dictionary<Vector2Int, ContentNode>();
    private Dictionary<Vector2Int, WallNode> wallNodes = new Dictionary<Vector2Int, WallNode>();
    private Dictionary<string, RoomNode> roomNodes = new Dictionary<string, RoomNode>();
    private List<RoomNode> spawnNodes = new List<RoomNode>();

    private bool isFullyGenerated = false;

    // Initializer for the singleton. Should only be called from GameManager
    public void Init()
    {
        curLevel = 1;
        UIManager.Instance.SetLevelNum(curLevel);
    }

    public void StartLevel()
    {
        GameObject tempObj = Instantiate(baseRoomPrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelGeoHolder);
        startRoom = tempObj.GetComponent<LevelRoom>();
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

    public void GenerateLevel(LevelRoom room)
    {
        GameObject latestRoom = startRoom.gameObject;
        numRooms = 1;

        // Clear previous dungeon
        roomNodes.Clear();
        contentNodes.Clear();
        wallNodes.Clear();
        spawnNodes.Clear();

        while(true)
        {

            break;
        }
        isFullyGenerated = true;
    }

    public void AddContentNode(ContentNode newNode)
    {
        contentNodes.Add(newNode.pos, newNode);
    }

    public void AddWallNode(WallNode newNode)
    {
        wallNodes.Add(newNode.pos, newNode);
    }
}
