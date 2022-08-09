using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("----- Componets -----")]

    [SerializeField] CharacterController controller;

    [Header("----- Player Attributes -----")]

    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMult;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int maxJumps;

    Vector3 playerVelocity;
    Vector3 move = Vector3.zero;

    [SerializeField] int shootDistance;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;

    [SerializeField] List<GunStats> gunstat = new List<GunStats>();

    float playerSpeedOrignal;
    int timesJumped;

    bool isSpinting = false;
    // Start is called before the first frame update
    void Start() {
        playerSpeedOrignal = playerSpeed;
    }

    // Update is called once per frame
    void Update() {
        PlayerMovement();
        Sprint();
    }

    void PlayerMovement() {

        //Player is currently on the ground and is not jumping
        if(controller.isGrounded && playerVelocity.y < 0) {

            playerVelocity.y = 0.0f;
            timesJumped = 0;

        }

        //Getting input from Unity Input Manager
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        //Adding the move vector to the character controller
        controller.Move(move * playerSpeed * Time.deltaTime);

        //Jumping functionallity
        if(Input.GetButtonDown("Jump") && timesJumped < maxJumps) {
            playerVelocity.y = jumpHeight;
            timesJumped++;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint() {

        if (Input.GetButtonDown("Sprint")) {
            isSpinting = true;
            playerSpeed = playerSpeed * sprintMult;
        }

        if (Input.GetButtonUp("Sprint")) {
            isSpinting = false;
            playerSpeed = playerSpeedOrignal;
        }
    }

    public void GunPickup(float shootR, int shootD, int shootDmg, GunStats stats) {
        shootRate = shootR;
        shootDistance = shootD;
        shootDamage = shootDmg;
        gunstat.Add(stats);
    }
}
