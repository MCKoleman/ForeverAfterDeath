using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private SpriteRenderer sprite;

    protected override void Start()
    {
        base.Start();
        curHealth = maxHealth;
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        UIManager.Instance.SetMaxHealth(maxHealth);
        UIManager.Instance.UpdateHealth(curHealth);
    }
    public override bool IsPlayer() { return true; }
    protected override void HandleDeath()
    {
        base.HandleDeath();
        sprite.enabled = false;
        GameManager.Instance.EndGame();
        //Destroy(this.gameObject);
    }

    protected override void HandleHealthChange()
    {
        base.HandleHealthChange();
        UIManager.Instance.UpdateHealth(curHealth);
    }
}
