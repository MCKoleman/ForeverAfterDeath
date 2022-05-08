using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private GameObject enemyDeathParticle;

    protected override void Start()
    {
        base.Start();
        maxHealth = Mathf.FloorToInt(maxHealth * GenManager.Instance.GetDiffMod());
        curHealth = maxHealth;
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        Instantiate(enemyDeathParticle, transform.position, Quaternion.identity, PrefabManager.Instance.projectileHolder);
        Camera.main.GetComponent<CameraController>().Shake(0.1f, 0.2f);

        // Attempt to disable turret
        TurretShooting tempTurret = this.GetComponent<TurretShooting>();
        if (tempTurret != null)
            tempTurret.isShootingActive = false;

        GenManager.Instance.IncEnemiesKilled();
        // Destroy the enemy
        Destroy(this.gameObject);
    }
}
