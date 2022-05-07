using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] private GameObject objectToFacePlayer;
    [Range(0f, 100f)]
    [SerializeField] private float rotateSpeed;

    private GameObject player;
    private bool canFacePlayer = true;


    void Start()
    {
        player = InputManager.Instance.GetPlayer().gameObject;
    }

    void Update()
    {
        if (canFacePlayer && player != null)
        {
            DoFacePlayer();
        }
    }

    void DoFacePlayer()
    {
        Vector2 direction;
        direction.x = player.transform.position.x - transform.position.x;
        direction.y = player.transform.position.y - transform.position.y;
        Vector3 lerpVar = Vector3.Lerp(transform.up, direction, Time.deltaTime * rotateSpeed);
        objectToFacePlayer.transform.up = lerpVar;
    }
}