using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] GameObject enemyDeathParticle;
    protected override void HandleDeath()
    {
        base.HandleDeath();
        Instantiate(enemyDeathParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
