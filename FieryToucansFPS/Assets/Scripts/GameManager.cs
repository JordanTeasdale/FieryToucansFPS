using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject player;
    public PlayerController playerScript;
    public GameObject gunPosition;

    public GameObject bossDoor;
    public GameObject currentRoom;
    public int clearedRooms = 0;
    [SerializeField] int clearedRoomsRequired;

    public GameObject RespawnPos;
    public GameObject checkpointFeedback;

    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerWinMenu;
    public GameObject menuCurrentlyOpen;
    public GameObject playerDamageFlash;
    public GameObject ammoMagGUI;
    public GameObject ammoStockGUI;
    public GameObject reticle;
    public GameObject radialMenu;
    public Image playerHPBar;
    public GameObject lowHealthIndicator;
    public GameObject roomClearedFeedback;


    public bool isPaused = false;
    public bool isConfigOptions = false;
    bool gameOver = false;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        if (GameObject.FindGameObjectWithTag("Player") != null) { 
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();

            gunPosition = GameObject.FindGameObjectWithTag("Gun Position");

            RespawnPos = GameObject.FindGameObjectWithTag("Respawn Pos");
            playerScript.Respawn();

            bossDoor = GameObject.FindGameObjectWithTag("BossDoor"); 
        }
        else
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel") && playerScript.HP > 0 && !gameOver) {
            isPaused = !isPaused;

            if(!isConfigOptions)
            menuCurrentlyOpen = pauseMenu;

            menuCurrentlyOpen.SetActive(isPaused);

            if (isPaused)
                CursorLockPause();
            else
                CursorUnlockUnpause();
        }
        if (Time.timeScale > 0 && Input.GetButton("Open Radial")) {
            radialMenu.SetActive(true);
            CursorLockSlowed();
        }
        if (Input.GetButtonUp("Open Radial")) {
            if (playerScript.gunsList[radialMenu.GetComponent<RadialMenuScript>().selection].name != "Gun - Empty") {
                playerScript.weaponIndex = radialMenu.GetComponent<RadialMenuScript>().selection;
                playerScript.GunEquip(playerScript.gunsList[playerScript.weaponIndex]);
            }
            radialMenu.SetActive(false);
            CursorUnlockUnslowed();
        }
        if (playerScript.HP <= playerScript.HPOrig * 0.25)
            lowHealthIndicator.SetActive(true);
        else if (playerScript.HP > playerScript.HPOrig * 0.25)
            lowHealthIndicator.SetActive(false);

        AccessShowcase();
    }

    private void AccessShowcase()
    {
        if (Input.GetButton("AccessShow"))
        {
            SceneManager.LoadScene("Showcase Level");
        }
    }

    public void CursorLockPause() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);
        Time.timeScale = 0;
        playerScript.enabled = false;
    }
    public void CursorLockSlowed() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);
        player.GetComponentInChildren<CameraController>().enabled = false;
        Time.timeScale = 0.25f;
    }
    public void CursorUnlockUnslowed() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reticle.SetActive(true);
        player.GetComponentInChildren<CameraController>().enabled = true;
        Time.timeScale = 1;
    }
    public void CursorUnlockUnpause() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        reticle.SetActive(true);
        Time.timeScale = 1;
        menuCurrentlyOpen.SetActive(isPaused);
        playerScript.enabled = true;
    }

    public IEnumerator ClearRoom() {
        clearedRooms++;
        if (clearedRooms == clearedRoomsRequired - 1)
            Destroy(bossDoor);
        if (clearedRooms == clearedRoomsRequired) {
            yield return new WaitForSeconds(1.5f);
            playerWinMenu.SetActive(true);
            menuCurrentlyOpen = playerWinMenu;
            CursorLockPause();
            gameOver = true;
        }
    }
}
