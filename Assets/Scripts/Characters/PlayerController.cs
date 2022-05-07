using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerChar;
    private GlobalVars.SelectedAbility curAbility;

    [SerializeField] private float moveSpeed;
    private bool canMove = true;
    private Rigidbody2D rb;
    private Vector2 movePos;

    [SerializeField]
    private float activeMoveSpeed;
    private float dashSpeed;
    private float dashLength = 0.5f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    

    [SerializeField] private GameObject playerBullet;
    [SerializeField] private Transform shootPoint;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetChar();
    }

    private void Update()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movePos * moveSpeed * Time.fixedDeltaTime);
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
        }
    }

    public void HandleMove(Vector2 newPos)
    {
        movePos = newPos;
        //movePos = context.ReadValue<Vector2>();
    }



    public void HandleLook(Vector2 delta)
    {
        // Handle looking
    }

    public void HandleBlock()
    {
        // Handle Block
    }
    public void HandleAttack()
    {
        Instantiate(playerBullet, shootPoint.position, shootPoint.rotation);
    }

    public void HandleDash()
    {
        // Handle dashing
        if (dashCounter > 0)
        {
            if(dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }

        }
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
