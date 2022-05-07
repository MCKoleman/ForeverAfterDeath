using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class EverythingToPositions : MonoBehaviour
{
    [SerializeField] private GameObject titleTop;
    [SerializeField] private GameObject titleBot;
    [SerializeField] private TextMeshProUGUI titleCenter;

    [SerializeField] private Image topBar;
    [SerializeField] private TextMeshProUGUI topText;

    [SerializeField] private Image botBar;
    [SerializeField] private TextMeshProUGUI botText;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject buttonsFolder;





    void Start()
    {
        TitleFadeOut();
        ButtonsRotateOff();
        ButtonsFadeOut();
        ButtonsMoveDown();
        TweenTitle();
        StartCoroutine(ButtonsInCoroutine());
        
    }


    void TweenTitle()
    {
        topText.DOFade(1, 2).OnComplete(() =>
        { titleTop.transform.DOMoveY(titleTop.transform.position.y + 50, 3f).SetEase(Ease.InOutQuad); });
        botText.DOFade(1, 2).OnComplete(() =>
        { titleBot.transform.DOMoveY(titleBot.transform.position.y - 50, 3f).SetEase(Ease.InOutQuad); 
            titleCenter.DOFade(1, 4);
            titleCenter.transform.DOScale(new Vector3(1, 1, 1), 4);
            
        });

        topBar.DOFade(1, 1.5f);        
        botBar.DOFade(1, 1.5f);
    }

    IEnumerator ButtonsInCoroutine()
    {
        yield return new WaitForSeconds(3f);
        ButtonsFadeIn();
        ButtonsMoveUp();
        ButtonsRotateNormal();
    }

    #region Button Tweens
    void TitleFadeOut()
    {
        titleCenter.DOFade(0, 0);
        topBar.DOFade(0, 0);
        topText.DOFade(0, 0);
        botBar.DOFade(0, 0);
        botText.DOFade(0, 0);
        titleCenter.transform.DOScale(new Vector3 (0,0,0), 0);
    }

    void ButtonsFadeOut()
    {
        foreach(GameObject go in buttons)
        {
            go.GetComponent<Image>().DOFade(0, 0);
            go.GetComponentInChildren<TextMeshProUGUI>().DOFade(0,0);
        }
    }

    void ButtonsFadeIn()
    {
        foreach (GameObject go in buttons)
        {
            go.GetComponent<Image>().DOFade(1, 4f);
            go.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 4f);
        }
    }

    void ButtonsMoveUp()
    {
        buttonsFolder.transform.DOMoveY(buttonsFolder.transform.position.y + 30, 3f);
    }

    void ButtonsMoveDown()
    {
        buttonsFolder.transform.DOMoveY(buttonsFolder.transform.position.y -30, 3f);
    }

    void ButtonsRotateOff()
    {
        foreach (GameObject go in buttons)
        {
            go.transform.DORotate(new Vector3(-55, 0, -5), 0);
        }
    }
    void ButtonsRotateNormal()
    {
        foreach (GameObject go in buttons)
        {
            go.transform.DORotate(new Vector3(0, 0, 0), 2.5f);
        }
    }

    #endregion
}
