using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class TankAppController : MonoBehaviour
{
    public static TankAppController Instance { get; private set; }

    public AudioMixer masterMixer;
    public int SoundVolume = 0; //dB
    public int MusicVolume = 0; //dB

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    public void SetSFXVolume(int value)
    {
        masterMixer.SetFloat("SFX", value);
        SoundVolume = value;
    }

    public void SetMusicVolume(int value)
    {
        masterMixer.SetFloat("Music", value);
        MusicVolume = value;
    }

    public int GetMusicVolume()
    {
        float value;
        masterMixer.GetFloat("Music", out value);
        int musicVolume = (int)value;
        return musicVolume;
    }
}
