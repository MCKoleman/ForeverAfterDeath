using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    // Major UI components
    [SerializeField]
    private HUD hud;
    [SerializeField]
    private PauseMenu pauseMenu;
    [SerializeField]
    private DeathMenu deathMenu;

    // Initializes the UI
    public void Init()
    {

    }

    // Initializes the hud
    public void InitHUD()
    {
        ShowPauseMenu(false);
        ShowDeathMenu(false);
        ShowHUD(true);
    }

    // Displays the HUD
    public void ShowHUD(bool shouldEnable = true)
    {
        hud.EnableHUD(shouldEnable);
    }

    // Displays the death menu
    public void ShowDeathMenu(bool shouldEnable = true)
    {
        hud.EnableHUD(!shouldEnable);
        deathMenu.EnableMenu(shouldEnable);
    }

    // Displays the pause menu
    public void ShowPauseMenu(bool shouldEnable = true)
    {
        pauseMenu.EnableMenu(shouldEnable);
    }

    // Toggles the game's pause state
    public void PauseGameToggle()
    {
        // Pause an unpaused game
        if (Time.timeScale != 0.0f)
            PauseGame();
        // Unpause a paused game
        else
            UnpauseGame();
    }

    // Pauses the game. Should only happen if called from HUD.
    public void PauseGame()
    {
        pauseMenu.PauseGame();
    }

    // Unpauses the game. Should only happen if called from HUD.
    public void UnpauseGame()
    {
        pauseMenu.UnpauseGame();
    }

    // Handles the player leaving the scene or game
    public void HandleExit()
    {
        GameManager.Instance.SetTimeScale(1.0f);
        GameManager.Instance.EndGame();
    }

    // Returns whether the game is paused or not
    public bool IsPaused() { return Time.timeScale == 0.0f; }

    // Wrapper function for refreshing the HUD, callable from anywhere
    public void RefreshHUD() { hud.RefreshHUD(); }

    /* ============================================================ Child component function wrappers ==================================== */
    public void UpdateHealth(float newValue) { hud.UpdateHealth(newValue); }
    public void SetMaxHealth(float newValue) { hud.SetMaxHealth(newValue); }
    public void SetLevelNum(int num) { hud.SetLevelNum(num); }
    public void UpdateXpDisplay(float percent) { hud.UpdateXpDisplay(percent); }
    public void UpdateLifeDisplay(int lives) { hud.UpdateLifeDisplay(lives); }
}
