using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject hudObj;
    [SerializeField]
    private UISliderBar healthBar;
    [SerializeField]
    private UISliderBar xpBar;
    [SerializeField]
    private TextMeshProUGUI levelNum;
    [SerializeField]
    private TextMeshProUGUI livesNum;
    [SerializeField]
    private bool isActive;

    // Enables the hud. Passing false allows the same function to disable the hud
    public void EnableHUD(bool shouldEnable = true)
    {
        hudObj.SetActive(shouldEnable);
        isActive = shouldEnable;
    }

    // Refreshes the HUD, getting information from relevant managers
    public void RefreshHUD()
    {
        // Refreshes which part of the HUD should be displayed based on whether it is singleplayer
        if(isActive)
            EnableHUD();
    }

    // Sets the level number displayed
    public void SetLevelNum(int num)
    {
        levelNum.text = num.ToString();
    }

    /* ============================================================ Child component function wrappers ==================================== */
    public void UpdateHealth(float percent) { healthBar.UpdateValue(percent); }
    public void UpdateXpDisplay(float percent) { xpBar.UpdateValue(percent); }
    public void UpdateLifeDisplay(int lives) { livesNum.text = GameManager.Instance.GetIsEasyMode() ? '\u221E'.ToString() : lives.ToString(); }
}
