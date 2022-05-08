using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject hudObj;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI healthNumber;
    [SerializeField]
    private TextMeshProUGUI damageNumber;
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
    public void UpdateHealth(float newValue)
    {
        healthBar.value = newValue;
        healthNumber.text = newValue.ToString();
        healthNumber.transform.DOScale(new Vector3(1.5f, 3f, 1f), 0.2f).OnComplete(() =>
        { healthNumber.transform.DOScale(new Vector3(1f, 2.5f, 1f), 0.2f); });
    }

    public void UpdateDamage(int newValue)
    {
        damageNumber.text = newValue.ToString();
        damageNumber.transform.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f).OnComplete(() =>
        { damageNumber.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f); });
    }


    public void SetMaxHealth(float newValue) { healthBar.maxValue = newValue; }
    public void UpdateXpDisplay(float percent) { xpBar.UpdateValue(percent); }
    public void UpdateLifeDisplay(int lives) { livesNum.text = GameManager.Instance.GetIsEasyMode() ? '\u221E'.ToString() : lives.ToString(); }
}
