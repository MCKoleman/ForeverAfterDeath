using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GlobalVars.PowerupType type;
    public int strength = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerController tempPlayer = collision.GetComponent<PlayerController>();
            if (tempPlayer != null)
            {
                tempPlayer.PickupPowerup(type, strength);
                Destroy(this.gameObject);
            }
        }
    }
}
