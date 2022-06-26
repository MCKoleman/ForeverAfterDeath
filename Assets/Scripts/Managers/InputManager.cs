using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private GlobalVars.SelectedAbility curAbility;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private CurrentDevice curDevice;

    private enum CurrentDevice { INVALID= 0, KEYBOARD_MOUSE = 1, GAMEPAD = 2 };

    // Handles initialization. Should only be called from the game manager
    public void Init()
    {
        
    }

    /* ============================================= Input Functions ====================================== */
    #region Input Functions
    private void HandleMove(Vector2 delta)
    {
        // Handle moving
        GetPlayer()?.HandleMove(delta);
    }

    private void HandleLook(Vector2 pos)
    {
        // Handle looking
        GetPlayer()?.HandleLook(pos);
    }

    private void HandleLookDelta(Vector2 delta)
    {
        // Handle looking
        GetPlayer()?.HandleLookDelta(delta);
    }

    private void HandleAttack()
    {
        // Handle attacking
        GetPlayer()?.HandleAttack();
    }

    private void HandleBlock()
    {
        // Handle blocking
        GetPlayer()?.HandleBlock();
    }

    private void HandleDash()
    {
        // Handle dashing
        GetPlayer()?.HandleDash();
    }

    private void HandleInteract()
    {
        // Handle interaction
        GetPlayer()?.HandleInteract();
    }

    private void HandleMenu()
    {
        // Handle menu selecting
        UIManager.Instance.PauseGameToggle();
    }

    private void HandleAbility1()
    {
        curAbility = GlobalVars.SelectedAbility.ABILITY_1;
        HandleAbilityChange();
    }

    private void HandleAbility2()
    {
        curAbility = GlobalVars.SelectedAbility.ABILITY_2;
        HandleAbilityChange();
    }

    private void HandleAbility3()
    {
        curAbility = GlobalVars.SelectedAbility.ABILITY_3;
        HandleAbilityChange();
    }

    private void HandleAbilityCycle(float axis)
    {
        curAbility = GlobalVars.GetNextAbility(curAbility, axis);
        HandleAbilityChange();
    }

    private void HandleAbilityChange()
    {
        // Update player on new ability selection
        GetPlayer()?.HandleAbilityChange(curAbility);
    }
    #endregion

    /* ============================================= Utility Functions ====================================== */
    #region Utility Functions
    public PlayerController GetPlayer()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        return player;
    }

    // Returns whether the player can take input or not
    public bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused();
    }
    #endregion

    /* ============================================= Input Handles ====================================== */
    #region Input Handles
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleLookContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput() && curDevice == CurrentDevice.KEYBOARD_MOUSE)
            HandleLook(context.ReadValue<Vector2>());
    }

    public void HandleLookDeltaContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput() && curDevice != CurrentDevice.KEYBOARD_MOUSE)
            HandleLookDelta(context.ReadValue<Vector2>());
    }

    public void HandleAttackContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAttack();
    }

    public void HandleAttackButton()
    {
        if (CanTakeInput())
            HandleAttack();
    }

    public void HandleBlockContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleBlock();
    }

    public void HandleInteractContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleInteract();
    }

    public void HandleDashContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleDash();
    }

    public void HandleDashButton()
    {
        if (CanTakeInput())
            HandleDash();
    }

    public void HandleAbility1Context(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility1();
    }

    public void HandleAbility2Context(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility2();
    }

    public void HandleAbility3Context(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility3();
    }
    public void HandleAbilityCycleContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbilityCycle(MathUtils.GetDirection(context.ReadValue<float>()));
    }

    public void HandleMenuContext(InputAction.CallbackContext context)
    {
        if (context.performed)
            HandleMenu();
    }

    public void HandleInputSwap(PlayerInput input)
    {
        // Check if using gamepad
        if (input.currentControlScheme == "Gamepad")
            curDevice = CurrentDevice.GAMEPAD;
        // Check if using keyboard and mouse
        else if (input.currentControlScheme == "Keyboard&Mouse")
            curDevice = CurrentDevice.KEYBOARD_MOUSE;
        // Only support keyboard, mouse, and gamepad
        else
            curDevice = CurrentDevice.INVALID;
    }
    #endregion
}
