using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string name;
   
    public AudioClip orignalFile;
    public AudioMixer mixer;
    public AudioMixerGroup mixerGroup;

    [Range(1, 1.5f)] public float pitch;
    [Range(0, 1f)] public float Volume;

    public bool playOnAwake;
    public bool isLooping;


   [HideInInspector] public AudioSource sourceOfSound;


   
    
}
