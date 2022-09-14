using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class ButtonFunctions : MonoBehaviour {
    LevelLoader levelLoader;
    public GameObject menuFirstButton, OptionsFirstButton, OptionsClosedButton, pauseFirstButton;
    
    float timer;

    public void Start() {
        //clearing the currently selected menu choice
        EventSystem.current.SetSelectedGameObject(null);

        //set a new menu choice
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        //Going through the Heirarchy to find levelloader and gets crossfade object to call coroutine
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();



    }


    public void Resume() {
        if (GameManager.instance.isPaused) {
            MenuSoundTrigger();
            GameManager.instance.UnPause();
        }
    }

    public void Restart() {
        MenuSoundTrigger();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.CursorUnlockUnpause();
        levelLoader.CoRoutRun();
    }

    public void Respawn() {
        GameManager.instance.playerScript.ResetHP();
        GameManager.instance.playerScript.Respawn();
        GameManager.instance.isPaused = false;
        GameManager.instance.CursorUnlockUnpause();
        //levelLoader.CoRoutRun();

    }

    public void Quit() {
        MenuSoundTrigger();
        levelLoader.CoRoutRun();
        Application.Quit();
    }

    public void PlaysGame() {
        MenuSoundTrigger();
        levelLoader.CoRoutRun();
        SceneManager.LoadScene("Level 1");
    }

    public void QCreditScene() {
        MenuSoundTrigger();
        levelLoader.CoRoutRun();
        SceneManager.LoadScene("Credits");
    }

    public void ReturnToMainMenu() {
        MenuSoundTrigger();
        levelLoader.CoRoutRun();
        SceneManager.LoadScene("Main Menu");
    }
    
    public void MenuSoundTrigger() {

        AudioManager.instance.PlayOneShot("Gun Click");
        FindObjectOfType<AudioManager>().PlayOneShot("Main Menu Transition");
    }
}
