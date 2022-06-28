using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter playerChar;
    private GlobalVars.SelectedAbility curAbility;

    [Header("Move Info")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dashForce = 50.0f;
    private bool canMove = true;
    private Rigidbody2D rb;
    private Vector2 movePos;
    [SerializeField]
    private Vector2 lastMoveDir;

    [Header("Dash Info")]
    [SerializeField]
    private float maxDashCooldown = 2.0f;
    [SerializeField]
    private float maxDashDuration = 0.5f;

    [SerializeField]
    private float curDashCooldown;
    [SerializeField]
    private float curDashDuration;
    private float activeMoveSpeed;

    [Header("Fire Info")]
    [SerializeField]
    private GameObject playerBullet;
    [SerializeField]
    private Transform shootPoint1;
    [SerializeField]
    private ParticleSystem shootEffect1;
    [SerializeField]
    private Transform shootPoint2;
    [SerializeField]
    private ParticleSystem shootEffect2;
    private int fireCount = 0;
    [Range(0f, 100f), SerializeField]
    private float rotateSpeed = 25.0f;
    [Range(60.0f, 480.0f), SerializeField]
    private float fireRpm = 60.0f;
    private bool isFiring = false;

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
        lastMoveDir = Vector2.up;
        UIManager.Instance.UpdateDashProgress(1.0f);
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive)
            return;
        
        // Handle dash unless it's over
        if (IsDashActive())
        {
            curDashDuration -= Time.fixedDeltaTime;
            if (!IsDashActive())
            {
                rb.velocity = Vector2.zero;
                curDashCooldown = maxDashCooldown;
                UIManager.Instance.UpdateDashProgress(0.0f);
            }
        }
        else if (canMove)
        {
            rb.MovePosition(rb.position + movePos * activeMoveSpeed * Time.fixedDeltaTime);
        }
        // Handle dash cooldown
        if (curDashCooldown > 0)
        {
            curDashCooldown = Mathf.Clamp(curDashCooldown - Time.fixedDeltaTime, 0.0f, maxDashCooldown);
            UIManager.Instance.UpdateDashProgress(1.0f - GetDashCooldownPercent());
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

    // Handles starting attacking
    public void HandleAttack()
    {
        if(!isFiring)
            StartCoroutine(HandleAutoFire());
    }

    // Handles canceling attacking
    public void HandleAttackCancel()
    {
        isFiring = false;
    }

    // Handles firing a projectile
    private void HandleFire()
    {
        audioSource.Play();
        if (fireCount % 2 == 0)
        {
            shootEffect1.Play();
            ShootProjectile(shootPoint1);
        }
        else
        {
            shootEffect2.Play();
            ShootProjectile(shootPoint2);
        }
        fireCount++;
    }

    // Handles automatically firing until released
    private IEnumerator HandleAutoFire()
    {
        float timeToWait = 60.0f / fireRpm;
        isFiring = true;
        while(isFiring)
        {
            HandleFire();
            yield return new WaitForSeconds(timeToWait);
        }
    }

    private void ShootProjectile(Transform shootPoint)
    {
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
            Debug.Log($"Attempting to dash with force: [{lastMoveDir * dashForce}]");
            rb.AddForce(lastMoveDir * dashForce, ForceMode2D.Impulse);
            //activeMoveSpeed = dashSpeed;
            curDashDuration = maxDashDuration;
            UIManager.Instance.UpdateDashProgress(1.0f - GetDashCooldownPercent());
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

    public float GetDashCooldownPercent() { return Mathf.Clamp(curDashCooldown / maxDashCooldown, 0.0f, 1.0f); }
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
