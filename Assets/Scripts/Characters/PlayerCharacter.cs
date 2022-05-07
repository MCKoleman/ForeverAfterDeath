using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    protected override void Start()
    {
        base.Start();
        curHealth = maxHealth;
        UIManager.Instance.SetMaxHealth(maxHealth);
        UIManager.Instance.UpdateHealth(curHealth);
    }
    public override bool IsPlayer() { return true; }
    protected override void HandleDeath()
    {
        base.HandleDeath();
        Destroy(this.gameObject);
    }

    protected override void HandleHealthChange()
    {
        base.HandleHealthChange();
        UIManager.Instance.UpdateHealth(curHealth);
    }
}
