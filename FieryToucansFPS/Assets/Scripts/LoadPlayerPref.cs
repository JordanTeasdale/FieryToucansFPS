using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadPlayerPref : MonoBehaviour
{
    [SerializeField] AudioMixer mainAudioMixer;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV");
        mainAudioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        mainAudioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        mainAudioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
    }
}
