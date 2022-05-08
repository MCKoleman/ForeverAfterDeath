using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter playerChar;
    private GlobalVars.SelectedAbility curAbility;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dashForce = 50.0f;
    private bool canMove = true;
    private Rigidbody2D rb;
    private Vector2 movePos;
    [SerializeField]
    private Vector2 lastMoveDir;

    [SerializeField]
    private float dashSpeed = 20.0f;
    [SerializeField]
    private float maxDashCooldown = 2.0f;
    [SerializeField]
    private float maxDashDuration = 0.5f;

    [SerializeField]
    private float curDashCooldown;
    [SerializeField]
    private float curDashDuration;
    private float activeMoveSpeed;

    

    [SerializeField]
    private GameObject playerBullet;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private ParticleSystem shootEffect;
    [Range(0f, 100f), SerializeField]
    private float rotateSpeed = 25.0f;

    private Vector3 targetRotation;

    [SerializeField]
    private AudioClip playerLaserAudio;

    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = playerLaserAudio;
        rb = GetComponent<Rigidbody2D>();
        GetChar();
        activeMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        if (canMove)
        {
            rb.MovePosition(rb.position + movePos * activeMoveSpeed * Time.fixedDeltaTime);
        }
        
        // Handle dash unless it's over
        if (IsDashActive())
        {
            curDashDuration -= Time.fixedDeltaTime;
            if (!IsDashActive())
            {
                rb.velocity = Vector2.zero;
                curDashCooldown = maxDashCooldown;
            }
        }
        // Handle dash cooldown
        if (curDashCooldown > 0)
        {
            curDashCooldown -= Time.fixedDeltaTime;
        }   

        // Rotate player to face target
        if(this.transform.up != targetRotation)
        {
            this.transform.up = Vector3.Lerp(transform.up, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    // Handles picking up a powerup
    public void PickupPowerup(GlobalVars.PowerupType type, int strength)
    {
        Camera.main.GetComponent<CameraController>().Shake(0.1f, 0.2f);

        switch (type)
        {
            case GlobalVars.PowerupType.HEALTH:
                GetChar()?.AddMaxHealth(strength);
                break;
            case GlobalVars.PowerupType.DAMAGE:
                GetChar()?.AddMaxDamage(strength);
                break;
            case GlobalVars.PowerupType.DEFAULT:
            default:
                break;
        }
    }

    #region Movement and input
    public void HandleMove(Vector2 newPos)
    {
        if (IsDashActive())
            return;

        movePos = newPos;
        if (movePos != Vector2.zero)
            lastMoveDir = movePos.normalized;
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
        audioSource.Play();
        shootEffect.Play();
        GameObject tempObj = Instantiate(playerBullet, shootPoint.position, shootPoint.rotation, PrefabManager.Instance.projectileHolder);
        PlayerProjectile tempProjectile = tempObj.GetComponent<PlayerProjectile>();
        if (tempProjectile != null && GetChar() != null)
            tempProjectile.SetDamage(GetChar().GetCurDamage());
    }

    public void HandleDash()
    {
        // Handle dashing
        if (curDashCooldown <= 0 && !IsDashActive())
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(lastMoveDir * dashForce, ForceMode2D.Impulse);
            //activeMoveSpeed = dashSpeed;
            curDashDuration = maxDashDuration;
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
#endregion

    // Returns whether the dash is currently active
    public bool IsDashActive() { return curDashDuration > 0.0f; }

    // Returns the character component
    public PlayerCharacter GetChar()
    {
        if(playerChar == null)
            this.GetComponent<PlayerCharacter>();
        return playerChar;
    }
}
