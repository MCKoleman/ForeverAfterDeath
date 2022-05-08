using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuHolder;
    [SerializeField]
    private GameObject firstSelectedObject;
    [SerializeField]
    private List<string> tooltipTexts;
    [SerializeField]
    private TextMeshProUGUI trialText;
    [SerializeField]
    private TextMeshProUGUI levelNumText;
    [SerializeField]
    private TextMeshProUGUI killsNumText;
    [SerializeField]
    private TextMeshProUGUI tooltipText;

    // Enables the death menu
    public void EnableMenu(bool shouldEnable)
    {
        menuHolder.SetActive(shouldEnable);
        killsNumText.text = GenManager.Instance.GetEnemiesKilled().ToString();
        levelNumText.text = (GenManager.Instance.GetLevelNum() - 1).ToString();
        trialText.text = "TRIAL #" + GameManager.Instance.GetTrialNum().ToString() + " OVER";
        tooltipText.text = tooltipTexts[Mathf.Clamp(Random.Range(0, tooltipTexts.Count), 0, tooltipTexts.Count)];

        // Selects the first button
        //if (shouldEnable)
        //    UISelectionManager.Instance.SetDefaultSelection(firstSelectedObject);
    }
}
