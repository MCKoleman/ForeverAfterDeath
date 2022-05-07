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
    private float dashSpeed;
    private float dashLength = 0.5f;
    private float dashCooldown = 2f;
    private float dashDuration;
    private float activeMoveSpeed;

    

    [SerializeField] private GameObject playerBullet;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private ParticleSystem shootEffect;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetChar();
        activeMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movePos * activeMoveSpeed * Time.fixedDeltaTime);
        }

        if (dashDuration > 0)
        {
            dashDuration -= Time.fixedDeltaTime;
            if (dashDuration <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashDuration = dashLength;
            }
        }
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.fixedDeltaTime;
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
        shootEffect.Play();
        Instantiate(playerBullet, shootPoint.position, shootPoint.rotation, PrefabManager.Instance.projectileHolder);
    }

    public void HandleDash()
    {
        // Handle dashing
        if (dashDuration > 0)
        {
         if (dashCooldown <=0 && dashDuration <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashDuration = dashLength;
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
