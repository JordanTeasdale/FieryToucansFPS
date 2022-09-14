using System;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake() {

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds) {

            sound.sourceOfSound = gameObject.AddComponent<AudioSource>();
            sound.sourceOfSound.clip = sound.orignalFile;

            sound.sourceOfSound.volume = sound.Volume;
            sound.sourceOfSound.pitch = sound.pitch;
            sound.sourceOfSound.loop = sound.isLooping;
            sound.sourceOfSound.playOnAwake = sound.playOnAwake;
            sound.sourceOfSound.outputAudioMixerGroup = sound.mixerGroup;

        }

       
    }

    public void Play(string _name) {

        Sound soundToPlay = Array.Find<Sound>(sounds, sound => sound.name == _name);
        if (soundToPlay == null)
            return;
        Debug.Log(soundToPlay.sourceOfSound.ToString());
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
