using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject player;
    public PlayerController playerScript;

    public GameObject bossDoor;
    public GameObject currentRoom;
    public int clearedRooms = 0;
    [SerializeField] int clearedRoomsRequired;

    public GameObject RespawnPos;

    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerWinMenu;
    public GameObject menuCurrentlyOpen;
    public GameObject playerDamageFlash;
    public GameObject ammoMagGUI;
    public GameObject ammoStockGUI;
    public GameObject reticle;
    public Image playerHPBar;

    public bool isPaused = false;
    bool gameOver = false;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        ammoMagGUI = GameObject.FindGameObjectWithTag("Ammo Mag GUI");
        ammoStockGUI = GameObject.FindGameObjectWithTag("Ammo Stock GUI");

        RespawnPos = GameObject.FindGameObjectWithTag("Respawn Pos");
        playerScript.Respawn();

        bossDoor = GameObject.FindGameObjectWithTag("BossDoor");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel") && playerScript.HP > 0 && !gameOver) {
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
        reticle.SetActive(false);
        Time.timeScale = 0;
    }
    public void CursorUnlockUnpause() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reticle.SetActive(true);
        Time.timeScale = 1;
        menuCurrentlyOpen.SetActive(isPaused);
    }

    public IEnumerator ClearRoom() {
        clearedRooms++;
        if (clearedRooms == clearedRoomsRequired - 1)
            Destroy(bossDoor);
        if (clearedRooms == clearedRoomsRequired) {
            yield return new WaitForSeconds(1f);
            playerWinMenu.SetActive(true);
            menuCurrentlyOpen = playerWinMenu;
            CursorLockPause();
            gameOver = true;
        }
    }
}
