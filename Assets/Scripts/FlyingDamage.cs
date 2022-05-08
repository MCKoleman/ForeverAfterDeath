using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDamage : MonoBehaviour
{

    [SerializeField] private int damage;
    [SerializeField] private float damageCoolDown;
    private bool canAttack = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canAttack)
        {
            StartCoroutine(DoFlyingDamage(collision.GetComponent<PlayerCharacter>()));
        }
    }

    IEnumerator DoFlyingDamage(PlayerCharacter pc)
    {
        canAttack = false;
        pc.TakeDamage(damage);
        yield return new WaitForSeconds(damageCoolDown);
        canAttack = true;
    }
}
