using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // Handles initialization. Should only be called from the game manager
    public void Init()
    {

    }

    /* ============================================= Input Functions ====================================== */
    private void HandleMove(Vector2 delta)
    {

    }

    private void HandleLook(Vector2 delta)
    {

    }

    private void HandleFire()
    {

    }

    /* ============================================= Input Handles ====================================== */
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleLookContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleLook(context.ReadValue<Vector2>());
    }

    public void HandleFireContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleFire();
    }

    // Returns whether the player can take input or not
    public bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused();
    }
}
