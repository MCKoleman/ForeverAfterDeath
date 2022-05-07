using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetecter : MonoBehaviour
{
    [SerializeField]
    private GlobalVars.SceneType sceneType;

    void Start()
    {
        StartCoroutine(CommunicateSceneChange());
    }

    private IEnumerator CommunicateSceneChange()
    {
        yield return new WaitUntil(() => GameManager.Instance.GetIsReady());
        GameManager.Instance.HandleSceneChange(sceneType);
    }
}
