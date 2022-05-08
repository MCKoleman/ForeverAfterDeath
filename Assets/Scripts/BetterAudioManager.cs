using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterAudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip theme;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = theme;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
