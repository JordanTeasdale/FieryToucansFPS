using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
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
}
