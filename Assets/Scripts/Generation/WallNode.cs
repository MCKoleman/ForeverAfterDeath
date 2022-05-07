using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNode : MonoBehaviour
{
    private LevelRoom parentRoom;
    [SerializeField]
    private GlobalVars.WallType type;

    private void Start()
    {
        parentRoom = this.transform.GetComponentInParent<LevelRoom>();
        SpawnWall();
        DestroySelf();
    }

    // Spawns the correct wall type at this position
    private void SpawnWall()
    {
        GameObject tempWall = PrefabManager.Instance.GetWallObject(type);
        if (tempWall != null)
        {
            Instantiate(tempWall, this.transform.position, this.transform.rotation, this.transform.parent);
        }
    }

    // Destroys the node, allowing for additional cleanup
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
