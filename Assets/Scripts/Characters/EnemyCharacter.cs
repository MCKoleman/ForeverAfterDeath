using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] GameObject enemyDeathParticle;
    protected override void HandleDeath()
    {
        base.HandleDeath();
        Instantiate(enemyDeathParticle, transform.position, Quaternion.identity, PrefabManager.Instance.projectileHolder);

        // Attempt to disable turret
        TurretShooting tempTurret = this.GetComponent<TurretShooting>();
        if (tempTurret != null)
            tempTurret.isShootingActive = false;

        // Destroy the enemy
        Destroy(this.gameObject);
    }
}
