using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;
    [SerializeField] private GameObject impactExplosion;

    void Update()
    {
        MoveForward();
    }

    // Sets the damage of the projectile
    public void SetDamage(int newDamage)
    {
        projectileDamage = newDamage;
    }

    private void MoveForward()
    {
        transform.position += transform.up * Time.deltaTime * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage enemies
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyCharacter>().TakeDamage(projectileDamage);
            Instantiate(impactExplosion, transform.position, Quaternion.identity, PrefabManager.Instance.projectileHolder);
            Destroy(this.gameObject);
        }
        // Destroy on wall collision
        else if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage enemies
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<EnemyCharacter>().TakeDamage(projectileDamage);
            Instantiate(impactExplosion, transform.position, Quaternion.identity, PrefabManager.Instance.projectileHolder);
            Destroy(this.gameObject);
        }
        // Destroy on wall collision
        else if (collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
