using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{
    private LevelRoom room;

    private void Awake()
    {
        room = this.GetComponentInParent<LevelRoom>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore all collisions except the player
        if (!collision.CompareTag("Player") || collision.isTrigger)
            return;
        
        // Set the current room to this room
        GenManager.Instance.SetCurrentRoom(room);
    }
}
