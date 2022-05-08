using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        GetPlayer();
    }

    void Update()
    {
        this.transform.position = player.transform.position;
    }

    private GameObject GetPlayer()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        return player;
    }
}
