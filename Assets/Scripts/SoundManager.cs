using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;
    private void Awake()
    {
        instance = this;
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        ChangeSoundVolume(0);
        ChangeMusicVolume(0);
    }

    public void PlaySound(AudioClip _sound) 
    
    {
    soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change) 
    {
        float baseVolume = 1;

        float currentVolume = PlayerPrefs.GetFloat("soundVolume",0.6f);
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;
        float finalVolume = currentVolume * baseVolume;
        soundSource.volume = finalVolume;
        
        PlayerPrefs.SetFloat("soundVolume",currentVolume);
    }

    public void ChangeMusicVolume(float _change)
    {
        float baseVolume = 0.3f;
        float currentVolume = PlayerPrefs.GetFloat("musicVolume");
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;
        float finalVolume = currentVolume * baseVolume;
        musicSource.volume = finalVolume;

        PlayerPrefs.SetFloat("musicVolume", currentVolume);
    }
}
