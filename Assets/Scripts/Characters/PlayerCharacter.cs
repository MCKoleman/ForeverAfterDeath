using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    private int curDamage; 
    [SerializeField]
    private AudioClip deathSound;
    private SpriteRenderer sprite;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        sprite = this.gameObject.GetComponent<SpriteRenderer>(); 
        audioSource = GetComponent<AudioSource>();

        // Store default values
        GenManager.Instance.SetDefaults(maxHealth, curDamage);
        HandleSpawn();
        UIManager.Instance.UpdateDamage(curDamage);
    }

    protected void HandleSpawn()
    {
        // Reset values to defaults
        maxHealth = GenManager.Instance.GetMaxHealth();
        curDamage = GenManager.Instance.GetMaxDamage();
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
        audioSource.clip = deathSound;
        audioSource.Play();
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
        GenManager.Instance.SetMaxHealth(maxHealth);
        curHealth = maxHealth;
        HandleHealthChange();
    }

    // Adds to the current max damage
    public void AddMaxDamage(int amount)
    {
        curDamage += amount;
        GenManager.Instance.SetMaxDamage(curDamage);
        UIManager.Instance.UpdateDamage(curDamage);
    }

    public int GetCurDamage() { return curDamage; }
}
