using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class ButtonFunctions : MonoBehaviour
{
    LevelLoader levelLoader;
    public GameObject menuFirstButton, OptionsFirstButton, OptionsClosedButton, pauseFirstButton;


    public void Start()
    {
        //clearing the currently selected menu choice
        EventSystem.current.SetSelectedGameObject(null);

        //set a new menu choice
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        //Going through the Heirarchy to find levelloader and gets crossfade object to call coroutine
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

    }

    public void Resume()
    {
        if (GameManager.instance.isPaused)
        {
            GameManager.instance.isPaused = false;
            GameManager.instance.CursorUnlockUnpause();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.CursorUnlockUnpause();
        levelLoader.CoRoutRun();
    }

    public void Respawn()
    {
        GameManager.instance.playerScript.ResetHP();
        GameManager.instance.playerScript.Respawn();
        GameManager.instance.isPaused = false;
        GameManager.instance.CursorUnlockUnpause();
        levelLoader.CoRoutRun();

    }

    public void Quit()
    {
        levelLoader.CoRoutRun();
        Application.Quit();
    }

    public void PlaysGame()
    {
        levelLoader.CoRoutRun();
        SceneManager.LoadScene("Level 1");
    }

    public void QCreditScene()
    {
        levelLoader.CoRoutRun();
        SceneManager.LoadScene("Credits");
    }
}
