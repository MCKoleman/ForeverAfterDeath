using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonEnterExit : MonoBehaviour
{
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void ButtonHighlight()
    {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f);
    }

    public void ButtonDelight()
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }

}
