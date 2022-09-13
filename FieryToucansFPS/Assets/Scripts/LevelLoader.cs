using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private AudioMixer mainAudioMixer; 
    public float transitionTime = 1f;

    private void Start() {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV");
    }


    public void CoRoutRun()
    {
        StartCoroutine(StartTransition());
        mainAudioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        mainAudioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        mainAudioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
       
    }

    public IEnumerator StartTransition()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}
