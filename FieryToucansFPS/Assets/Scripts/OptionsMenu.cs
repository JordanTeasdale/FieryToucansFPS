using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    float FOV;
    float masterVol;
    float musicVol;
    float SFXVol;

    public void SetFOV(float _FOV) {

        FOV = _FOV;
        GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = FOV;
       
    }
    public void SetVolMaster(float _volume) {
        masterVol = _volume;
        mainMixer.SetFloat("MasterVol", _volume);
        
    }
    public void SetVolMusic(float _volume) {
        musicVol = _volume;
        mainMixer.SetFloat("MusicVol", _volume);
        
    }
    public void SetVolSFX(float _volume) {
        SFXVol = _volume;
        mainMixer.SetFloat("SFXVol", _volume);
       
    }
    public void Save() {
        PlayerPrefs.SetFloat("FOV", FOV);
        PlayerPrefs.SetFloat("MasterVol", masterVol);
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("SFXVol", SFXVol);
    }
}
