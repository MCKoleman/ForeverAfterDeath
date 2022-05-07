using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentNode : MonoBehaviour
{
    public Vector2Int pos;

    public GlobalVars.NodePlace nodePlace;
    private LevelRoom parentRoom;

    void Start()
    {
        pos = new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
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

    public LevelRoom GetParentRoom() { return parentRoom; }
}
