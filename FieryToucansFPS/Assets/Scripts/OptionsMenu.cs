using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    public void SetFOV(float _FOV) {

        GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = _FOV;
        PlayerPrefs.SetFloat("FOV", _FOV);
    }
    public void SetVolMaster(float _volume) {

        mainMixer.SetFloat("MasterVol", _volume);
        PlayerPrefs.SetFloat("MasterVol", _volume);
    }
    public void SetVolMusic(float _volume) {

        mainMixer.SetFloat("MusicVol", _volume);
        PlayerPrefs.SetFloat("MusicVol", _volume);
    }
    public void SetVolSFX(float _volume) {

        mainMixer.SetFloat("SFXVol", _volume);
        PlayerPrefs.SetFloat("SFXVol", _volume);
    }
}
