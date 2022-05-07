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

    [SerializeField] private GameObject playerBullet;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private ParticleSystem shootEffect;
    [Range(0f, 100f), SerializeField] private float rotateSpeed = 25.0f;
    private Vector3 targetRotation;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetChar();
    }

    private void Update()
    {
        if (canMove && GameManager.Instance.IsGameActive)
        {
            rb.MovePosition(rb.position + movePos * moveSpeed * Time.fixedDeltaTime);
        }

        // Rotate player to face target
        if(this.transform.up != targetRotation)
        {
            this.transform.up = Vector3.Lerp(transform.up, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    public void HandleMove(Vector2 newPos)
    {
        movePos = newPos;
    }

    public void HandleLook(Vector2 pos)
    {
        // Handle looking
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(pos);
        targetRotation = new Vector3(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y, 0.0f).normalized;
    }

    public void HandleLookDelta(Vector2 delta)
    {
        // Handle looking
        targetRotation = new Vector3(delta.x, delta.y, 0.0f).normalized;
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
