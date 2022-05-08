using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float shootCoolDown;
    [SerializeField] private float shootWindUpDuration;
    [SerializeField] private float shootDuration;

    private bool shooting;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCharacter>().TakeDamage(damage);
        }
        
        
    }
}
