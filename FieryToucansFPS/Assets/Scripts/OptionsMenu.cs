using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    float FOV, masterVol, musicVol, SFXVol;
    [SerializeField] public TMP_Text FOVLable, masterVolLable, musicVolLable, SFXVolLable;


    public void SetFOV(float _FOV) {

        FOV = _FOV;
        GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = FOV;
        FOVLable.text = Mathf.RoundToInt(FOV).ToString();
      

    }
    public void SetVolMaster(float _volume) {
        masterVol = _volume;
        mainMixer.SetFloat("MasterVol", _volume);
        masterVolLable.text = Mathf.RoundToInt(((masterVol + 80) / 80) * 100).ToString();
        


    }
    public void SetVolMusic(float _volume) {
        musicVol = _volume;
        mainMixer.SetFloat("MusicVol", _volume);
        musicVolLable.text = Mathf.RoundToInt(((musicVol + 80) / 80) * 100).ToString();
       

    }
    public void SetVolSFX(float _volume) {
        SFXVol = _volume;
        mainMixer.SetFloat("SFXVol", _volume);
        SFXVolLable.text = Mathf.RoundToInt(((SFXVol + 80) / 80) * 100).ToString();
        

    }
    public void Back() {
        if (PlayerPrefs.HasKey("FOV")) 
            GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV");
        else
            GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = 60f;

        if (PlayerPrefs.HasKey("MasterVol"))
            mainMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));

        if (PlayerPrefs.HasKey("MusicVol"))
            mainMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));

        if (PlayerPrefs.HasKey("SFXVol"))
            mainMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
    }
    public void Save() {
        PlayerPrefs.SetFloat("FOV", FOV);
        PlayerPrefs.SetFloat("MasterVol", masterVol);
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("SFXVol", SFXVol);
        PlayerPrefs.Save();
    }
}
