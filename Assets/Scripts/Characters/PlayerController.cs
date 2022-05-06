using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter playerChar;
    private GlobalVars.SelectedAbility curAbility;

    private void Start()
    {
        GetChar();
    }

    public void HandleMove(Vector2 delta)
    {
        // Handle moving
    }

    public void HandleLook(Vector2 delta)
    {
        // Handle looking
    }

    public void HandleAttack()
    {
        // Handle attacking
    }

    public void HandleBlock()
    {
        // Handle blocking
    }

    public void HandleDash()
    {
        // Handle dashing
    }

    public void HandleInteract()
    {
        // Handle interaction
    }

    public void HandleAbilityChange(GlobalVars.SelectedAbility newAbility)
    {
        // Handle ability change
        curAbility = newAbility;
    }

    // Returns the character component
    public PlayerCharacter GetChar()
    {
        if(playerChar == null)
            this.GetComponent<PlayerCharacter>();
        return playerChar;
    }
}
