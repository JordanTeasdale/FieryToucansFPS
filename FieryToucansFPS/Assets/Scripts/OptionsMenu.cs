using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class OptionsMenu : MonoBehaviour
{
    
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] GameObject optionsMenu;

    float FOV, masterVol, musicVol, SFXVol;
    [SerializeField] public TMP_Text FOVLable, masterVolLable, musicVolLable, SFXVolLable;
    [SerializeField] public Slider FOVSlider, masterVolSlider, musicVolSlider, SFXVolSlider;

    [SerializeField] bool isOptionsMainMenu = false;

    public void SetFOV(float _FOV) {
        GameManager.instance.isMainOptionsMenu = isOptionsMainMenu;
        FOV = _FOV;
        GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = FOV;
        FOVLable.text = Mathf.RoundToInt(FOV).ToString();
      

    }
    public void SetVolMaster(float _volume) {
        masterVol = _volume;
        mainMixer.SetFloat("MasterVol", _volume);
        masterVolLable.text = Mathf.RoundToInt(((masterVol + 41) / 41) * 100).ToString();
        


    }
    public void SetVolMusic(float _volume) {
        musicVol = _volume;
        mainMixer.SetFloat("MusicVol", _volume);
        musicVolLable.text = Mathf.RoundToInt(((musicVol + 41) / 41) * 100).ToString();
       

    }
    public void SetVolSFX(float _volume) {
        SFXVol = _volume;
        mainMixer.SetFloat("SFXVol", _volume);
        SFXVolLable.text = Mathf.RoundToInt(((SFXVol + 41) / 41) * 100).ToString();
        

    }
    public void Back() {
        GameManager.instance.CurrentPlayerPrefValue();
        GameManager.instance.isConfigOptions = false;
    }
    public void Save() {
        GameManager.instance.isMainOptionsMenu = isOptionsMainMenu;
        PlayerPrefs.SetFloat("FOV", FOV);
        PlayerPrefs.SetFloat("MasterVol", masterVol);
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("SFXVol", SFXVol);
        PlayerPrefs.Save();
        GameManager.instance.isConfigOptions = false;
    }
    public void SetSliders() {
        GameManager.instance.isMainOptionsMenu = isOptionsMainMenu;
        float value = 0f;

        mainMixer.GetFloat("MasterVol", out value);
        masterVolSlider.value = value;
        masterVolLable.text = Mathf.RoundToInt(((value + 80) / 80) * 100).ToString();

        mainMixer.GetFloat("MusicVol", out value);
        musicVolSlider.value = value;
        musicVolLable.text = Mathf.RoundToInt(((value + 80) / 80) * 100).ToString();

        mainMixer.GetFloat("SFXVol", out value);
        SFXVolSlider.value = value;
        SFXVolLable.text = Mathf.RoundToInt(((value + 80) / 80) * 100).ToString();

        if (!isOptionsMainMenu) {
            FOVSlider.value = GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView;
            FOVLable.text = Mathf.RoundToInt(GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView).ToString(); 
        }

    }
    public void OptionsMenuVisable() {
        GameManager.instance.isConfigOptions = true;
        GameManager.instance.previousMenu = GameManager.instance.menuCurrentlyOpen;
        GameManager.instance.menuCurrentlyOpen = optionsMenu;
        GameManager.instance.onPauseMenu = false;
    }
}
