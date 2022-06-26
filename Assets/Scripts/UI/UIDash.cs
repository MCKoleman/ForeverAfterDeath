using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIDash : MonoBehaviour
{
    [SerializeField]
    private Image bgImg;
    [SerializeField]
    private Image btnImg;
    [SerializeField]
    private Color enabledColor;
    [SerializeField]
    private Color disabledColor;

    public void UpdateProgress(float percent)
    {
        btnImg.fillAmount = percent;
        bgImg.color = (percent >= 1.0f ? enabledColor : disabledColor);
        if(percent >= 1.0f)
        {
            this.transform.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.2f).OnComplete(() =>
            { this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f); });
        }
    }
}
