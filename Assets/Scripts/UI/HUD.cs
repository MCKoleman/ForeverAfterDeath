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
    private GameObject mobileHUD;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private UIDash dashObj;
    [SerializeField]
    private GameObject powerObj;
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

    [Header("Mobile Tweaks")]
    [SerializeField]
    private Vector2 defaultHealthPos;
    [SerializeField]
    private Vector2 mobileHealthPos;
    [SerializeField]
    private Vector2 defaultDashPos;
    [SerializeField]
    private Vector2 mobileDashPos;
    [SerializeField]
    private Vector2 defaultPowerPos;
    [SerializeField]
    private Vector2 mobilePowerPos;

    // Rects
    private RectTransform healthRect;
    private RectTransform dashRect;
    private RectTransform powerRect;

    private void Start()
    {
        healthRect = healthBar.GetComponent<RectTransform>();
        dashRect = dashObj.GetComponent<RectTransform>();
        powerRect = powerObj.GetComponent<RectTransform>();
    }

    // Enables the hud. Passing false allows the same function to disable the hud
    public void EnableHUD(bool shouldEnable = true)
    {
        hudObj.SetActive(shouldEnable);
        mobileHUD.SetActive(shouldEnable && GameManager.Instance.IsMobile);
        AdjustMobilePos();
        isActive = shouldEnable;
    }

    // Adjusts the positions of HUD elements that are subject to mobile differences
    private void AdjustMobilePos()
    {
        if(GameManager.Instance.IsMobile)
        {
            healthRect.rect.Set(mobileHealthPos.x, mobileHealthPos.y, healthRect.rect.width, healthRect.rect.height);
            dashRect.rect.Set(mobileDashPos.x, mobileDashPos.y, dashRect.rect.width, dashRect.rect.height);
            powerRect.rect.Set(mobilePowerPos.x, mobilePowerPos.y, powerRect.rect.width, powerRect.rect.height);
        }
        else
        {
            healthRect.rect.Set(defaultHealthPos.x, defaultHealthPos.y, healthRect.rect.width, healthRect.rect.height);
            dashRect.rect.Set(defaultDashPos.x, defaultDashPos.y, dashRect.rect.width, dashRect.rect.height);
            powerRect.rect.Set(defaultPowerPos.x, defaultPowerPos.y, powerRect.rect.width, powerRect.rect.height);
        }
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
        healthBar.DOValue(newValue, 0.2f, true);
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

    public void UpdateLevelNum(int newValue)
    {
        levelNum.text = newValue.ToString();
        levelNum.transform.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f).OnComplete(() =>
        { levelNum.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f); });
    }


    public void SetMaxHealth(float newValue) { healthBar.maxValue = newValue; }
    public void UpdateXpDisplay(float percent) { xpBar.UpdateValue(percent); }
    public void UpdateLifeDisplay(int lives) { livesNum.text = GameManager.Instance.GetIsEasyMode() ? '\u221E'.ToString() : lives.ToString(); }
    public void UpdateDashProgress(float percent) { dashObj.UpdateProgress(percent); }
}
