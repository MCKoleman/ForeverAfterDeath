using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;
    [SerializeField] private GameObject impactExplosion;

    private string enemyTag = "Enemy";

    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.position += transform.up * Time.deltaTime * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            collision.GetComponent<EnemyCharacter>().TakeDamage(projectileDamage);
            Instantiate(impactExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
