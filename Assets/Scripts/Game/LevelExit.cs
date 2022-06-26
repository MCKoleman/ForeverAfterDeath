using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    // Exits the level, completing it
    private void ExitLevel()
    {
        AudioManager.Instance.PlayClip(audioClip);
        GenManager.Instance.IncLevelNum();
        GameManager.Instance.HandleLevelSwap(1);
        Print.Log("Level completed!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExitLevel();
        }
    }
}
