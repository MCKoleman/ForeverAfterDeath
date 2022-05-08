using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected int curHealth;
    [SerializeField]
    protected int maxHealth;

    protected virtual void Start()
    {
        curHealth = maxHealth;
    }

    /* ================================== Public Mutators =========================================== */
    public void HealHealth(int amount) { SetNewHealth(curHealth + amount); }
    public void TakeDamage(int amount) { SetNewHealth(curHealth - amount); }

    public void SetNewHealth(int _newHealth)
    {
        // Don't update health to be more dead
        if (curHealth <= 0 && _newHealth <= 0)
            return;

        curHealth = Mathf.Clamp(_newHealth, 0, maxHealth);
        HandleHealthChange();
    }

    /* ================================== Core Handles =========================================== */
    protected virtual void HandleHealthChange()
    {
        // Update UI display of health if needed
        UpdateUIHealth();

        // Handle death
        if (curHealth <= 0)
            HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        // Handle death
    }

    protected virtual void UpdateUIHealth()
    {
        // Update UI health
    }

    /* ================================== Utility Functions =========================================== */
    public virtual bool IsPlayer() { return false; }
    public int GetCurHealth() { return curHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public float GetHealthPercent() { return (float)curHealth / (float)maxHealth; }
}
