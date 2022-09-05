using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonFunctions : MonoBehaviour {

    public GameObject menuFirstButton, OptionsFirstButton, OptionsClosedButton, pauseFirstButton;


    public void Start()
    {
        //clearing the currently selected menu choice
        EventSystem.current.SetSelectedGameObject(null);

        //set a new menu choice
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

    }

    public void Resume() {
        if (GameManager.instance.isPaused) {
            GameManager.instance.isPaused = false;
            GameManager.instance.CursorUnlockUnpause();
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.CursorUnlockUnpause();
    }

    public void Respawn() {
        GameManager.instance.playerScript.ResetHP();
        GameManager.instance.playerScript.Respawn();
        GameManager.instance.isPaused = false;
        GameManager.instance.CursorUnlockUnpause();

    }

    public void Quit() {
        Application.Quit();
    }

    public void PlaysGame()
    {
        SceneManager.LoadScene("Level 1");
    }


}
