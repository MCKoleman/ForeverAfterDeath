using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private SpriteRenderer sprite;
    private int defaultMaxHealth;
    [SerializeField]
    private int curDamage;
    private int defaultDamage;

    protected override void Start()
    {
        base.Start();
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        
        // Store default values
        defaultMaxHealth = maxHealth;
        defaultDamage = curDamage;
        HandleSpawn();
        UIManager.Instance.UpdateDamage(defaultDamage);
    }

    protected void HandleSpawn()
    {
        // Reset values to defaults
        maxHealth = defaultMaxHealth;
        curDamage = defaultDamage;
        curHealth = maxHealth;

        // Enable player
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

    // Adds health to the player
    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
        curHealth = maxHealth;
        HandleHealthChange();
    }

    // Adds to the current max damage
    public void AddMaxDamage(int amount)
    {
        curDamage += amount;
        UIManager.Instance.UpdateDamage(curDamage);
    }

    public int GetCurDamage() { return curDamage; }
}
