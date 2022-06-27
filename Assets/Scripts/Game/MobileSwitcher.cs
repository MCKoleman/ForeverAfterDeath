using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject pcObj;
    [SerializeField]
    private GameObject mobileObj;

    private void OnEnable()
    {
        if (GameManager.Exists)
        {
            GameManager.Instance.OnMobileStatusChange += SwitchObjs;
            SwitchObjs(GameManager.Instance.IsMobile);
        }
        else
        {
            StartCoroutine(WaitForGameManager());
        }
    }

    private void OnDisable() { GameManager.Instance.OnMobileStatusChange -= SwitchObjs; }
    
    private IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Exists);
        GameManager.Instance.OnMobileStatusChange += SwitchObjs;
    }

    public void SwitchObjs(bool isMobile)
    {
        pcObj.SetActive(!isMobile);
        mobileObj.SetActive(isMobile);
    }
}
