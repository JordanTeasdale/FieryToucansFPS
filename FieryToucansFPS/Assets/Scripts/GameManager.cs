using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject player;
    public PlayerController playerScript;
    public GameObject enemy1;
    public EnemyAI enemy1Script;
    public GameObject enemy2;
    public EnemyAI enemy2Script;
    public GameObject enemy3;
    public EnemyAI enemy3Script;

    public GameObject RespawnPos;

    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject menuCurrentlyOpen;
    public GameObject playerDamageFlash;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        if (enemy1 && enemy2 && enemy3) {
            enemy1 = GameObject.FindGameObjectWithTag("Enemy1");
            enemy1Script = enemy1.GetComponent<EnemyAI>();
            enemy2 = GameObject.FindGameObjectWithTag("Enemy2");
            enemy2Script = enemy2.GetComponent<EnemyAI>();
            enemy3 = GameObject.FindGameObjectWithTag("Enemy3");
            enemy3Script = enemy2.GetComponent<EnemyAI>();
        }

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
