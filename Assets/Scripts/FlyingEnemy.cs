using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float damageCoolDown;
    private bool canAttack = true;

    private string playerTag = "Player";

    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && canAttack == true)
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
