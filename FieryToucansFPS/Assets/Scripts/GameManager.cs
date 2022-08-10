using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject player;
    public PlayerController playerScript;

    public GameObject RespawnPos;

    public GameObject pauseMenu;

    public GameObject menuCurrentlyOpen;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        RespawnPos = GameObject.FindGameObjectWithTag("Respawn Pos");
        playerScript.Respawn();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            isPaused = !isPaused;
            menuCurrentlyOpen = pauseMenu;
            menuCurrentlyOpen.SetActive(isPaused);

            if (isPaused)
                CursorLockPause();
            else
                CursorUnlockUnpause();
        }
    }

    public void CursorLockPause() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }
    public void CursorUnlockUnpause() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        menuCurrentlyOpen.SetActive(isPaused);
    }
}
