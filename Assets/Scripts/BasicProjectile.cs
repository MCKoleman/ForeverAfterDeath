using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;

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
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCharacter>().TakeDamage(projectileDamage);
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerCharacter>().TakeDamage(projectileDamage);
            Destroy(this.gameObject);
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
