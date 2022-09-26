using System;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public float percentOfListComplete;
    public float numSoundsInstansiated;


    public static AudioManager instance;

    void Awake() {

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        numSoundsInstansiated = 0;

        while (percentOfListComplete != 1) {
            percentOfListComplete = numSoundsInstansiated / sounds.Length;
            foreach (Sound sound in sounds) {

                sound.sourceOfSound = gameObject.AddComponent<AudioSource>();
                sound.sourceOfSound.clip = sound.orignalFile;

                sound.sourceOfSound.volume = sound.Volume;
                sound.sourceOfSound.pitch = sound.pitch;
                sound.sourceOfSound.loop = sound.isLooping;
                sound.sourceOfSound.playOnAwake = sound.playOnAwake;
                sound.sourceOfSound.outputAudioMixerGroup = sound.mixerGroup;

                numSoundsInstansiated++;

            } 
        }

       
    }

    public void Play(string _name) {

        Sound soundToPlay = Array.Find<Sound>(sounds, sound => sound.name == _name);
        if (soundToPlay == null)
            return;
        string track = soundToPlay.sourceOfSound.ToString();
        //Debug.Log(track);
        soundToPlay.sourceOfSound.Play();
    }

    public void Stop(string _name) {

        Sound soundToPlay = Array.Find<Sound>(sounds, sound => sound.name == _name);
        if (soundToPlay == null)
            return;

        soundToPlay.sourceOfSound.Stop();
       
    }

    public void PlayOneShot(string _name) {
        
        Sound soundToPlay = Array.Find<Sound>(sounds, sound => sound.name == _name);
        if (soundToPlay == null)
            return;
        
        soundToPlay.sourceOfSound.PlayOneShot(soundToPlay.sourceOfSound.clip);
    }




}
