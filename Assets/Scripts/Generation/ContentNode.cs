using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentNode : MonoBehaviour
{
    public GlobalVars.NodePlace nodePlace;
    private LevelRoom parentRoom;

    void Start()
    {
        GenManager.Instance.AddContentNode(this);

        parentRoom = GetComponentInParent<LevelRoom>();

        // Don't spawn content in the first room
        if (parentRoom.roomNum >= 1 || nodePlace == GlobalVars.NodePlace.WALL)
            GenManager.Instance.SpawnContent(this);
        // Spawn the entrance in the center of the first room
        else if (nodePlace == GlobalVars.NodePlace.CENTER)
            Instantiate(PrefabManager.Instance.entrancePrefab, new Vector3(0.0f, -0.2f, 0.0f), Quaternion.identity, PrefabManager.Instance.levelGeoHolder);

        DestroySelf();
    }

    // Destroys the node, allowing for additional cleanup
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    // Returns the position of this node
    public Vector2Int GetRoomPos()
    {
        // Return parent room position if possible
        LevelRoom parentRoom = this.GetComponentInParent<LevelRoom>();
        if(parentRoom != null)
            return new Vector2Int(Mathf.RoundToInt(parentRoom.transform.position.x), Mathf.RoundToInt(parentRoom.transform.position.y));
        // Return this node's position if parent room can't be accessed
        else
            return new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
    }

    // Returns the position of this node
    public Vector2Int GetPos() 
    { 
        return new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y)); 
    }

    public LevelRoom GetParentRoom() { return parentRoom; }
}
