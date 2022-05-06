using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Sound Sources")]
    [SerializeField]
    private AudioSource mainMusicSource;
    [SerializeField]
    private AudioSource transitionMusicSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource tempSource;

    [Header("Sound clips")]
    [SerializeField]
    private AudioClip uiSelect;
    [SerializeField]
    private AudioClip uiSubmit;

    // Initializes the audio manager
    public void Init()
    {
        //StopMusic();
    }

    // Stops the currently playing music and resets the manager
    public void StopMusic()
    {
        Debug.Log("Stop music called");
        mainMusicSource.Stop();
        transitionMusicSource.Stop();
    }

    // Plays the UI select effect
    public void PlayUISelect()
    {
        tempSource.clip = uiSelect;
        tempSource.Play();
    }

    // Plays the UI submit effect
    public void PlayUISubmit()
    {
        tempSource.clip = uiSubmit;
        tempSource.Play();
    }

    // Plays the given clip in the temporary channel
    public void PlayClip(AudioClip clip)
    {
        tempSource.clip = clip;
        tempSource.Play();
    }

    // Plays the given music clip
    public void PlayMusicClip(AudioClip clip)
    {
        mainMusicSource.clip = clip;
        mainMusicSource.Play();
    }

    // Returns the SFX source
    public AudioSource GetSFXSource() { return sfxSource; }
}
