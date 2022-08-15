using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("----- Componets -----")]

    [SerializeField] CharacterController controller;

    [Header("----- Player Attributes -----")]

    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMult;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int maxJumps;
    [Range(1,100)] [SerializeField] int HP;

    [Header("----- Weapon Attributes -----")]

    [SerializeField] int shootDistance;
    [SerializeField] int shootDamage;
    [Range(0.1f, 5)][SerializeField] float switchRate;
    [SerializeField] float shootRate;
    [SerializeField] List<GunStats> gunsList = new List<GunStats>();

    Vector3 playerVelocity;
    Vector3 move = Vector3.zero;

    float playerSpeedOrignal;
    int timesJumped;
    int HPOrig;
    int weaponIndex = 0;

    bool isSpinting = false;
    bool isShooting = false;
    bool isSwitching = false;
    // Start is called before the first frame update
    void Start() {
        playerSpeedOrignal = playerSpeed;
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update() {
        //debug code
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(1);

        PlayerMovement();
        Sprint();
        WeaponSelect();

        StartCoroutine(Shoot());
        //StartCoroutine(WeaponCycle());
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

    public void GunPickup( GunStats _stats) {
        GunEquip(_stats);
        gunsList.Add(_stats);
    }

    public void GunEquip(GunStats _gun)
    {
        shootDamage = _gun.shootDamage;
        shootDistance = _gun.shootDistance;
        shootRate = _gun.shootRate;
    }

    public void WeaponSelect()
    {
        if (gunsList.Count > 0)
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0 && weaponIndex < gunsList.Count -1)
            {
                ++weaponIndex;
                GunEquip(gunsList[weaponIndex]);
            }
            if(Input.GetAxis("Mouse ScrollWheel") > 0 && weaponIndex == gunsList.Count)
            {
                weaponIndex = 0;
                GunEquip(gunsList[weaponIndex]);
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0 && weaponIndex > 0)
            {
                --weaponIndex;
                GunEquip(gunsList[weaponIndex]);
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0 && weaponIndex == 0)
            {
                weaponIndex = gunsList.Count;
                GunEquip(gunsList[weaponIndex]);
            }
        }
    }

    IEnumerator WeaponCycle() {
        if (gunsList.Count > 0 && Input.mouseScrollDelta.y != 0 && !isSwitching) {
            isSwitching = true;
            Debug.Log(Input.mouseScrollDelta.y);
            if (Input.mouseScrollDelta.y > 0) {
                weaponIndex++;
                if (weaponIndex == gunsList.Count)
                    weaponIndex = 0;
                GunStats currentGun = gunsList[weaponIndex];
                shootRate = currentGun.shootRate;
                shootDistance = currentGun.shootDistance;
                shootDamage = currentGun.shootDamage;
            } else if (Input.mouseScrollDelta.y < 0) {
                weaponIndex--;
                if (weaponIndex < 0)
                    weaponIndex = gunsList.Count - 1;
                GunStats currentGun = gunsList[weaponIndex];
                shootRate = currentGun.shootRate;
                shootDistance = currentGun.shootDistance;
                shootDamage = currentGun.shootDamage;
            }
            yield return new WaitForSeconds(switchRate);
            isSwitching = false;
        }
    }

    public void TakeDamage(int _dmg) {
        HP -= _dmg;
       StartCoroutine(DamageFlash());
        if (HP <= 0) {
            //player death state
            Respawn();
            Death();
        }
    }

    IEnumerator DamageFlash() {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }

    IEnumerator Shoot()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.000001f); //makes a visible line to visualize the shoot ray

        if (Input.GetButton("Shoot") && !isShooting && gunsList.Count > 0) {
            isShooting = true;

            //does something
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance)) {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable isDamageable)) {
                    if (hit.collider is SphereCollider) {
                        isDamageable.TakeDamage(shootDamage * 2);
                    }  else
                        isDamageable.TakeDamage(shootDamage);
                }
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void Respawn() {
        controller.enabled = false;
        transform.position = GameManager.instance.RespawnPos.transform.position;
        controller.enabled = true;
    }

    public void Death() {
        GameManager.instance.CursorLockPause();
        GameManager.instance.playerDeadMenu.SetActive(true);
        GameManager.instance.menuCurrentlyOpen = GameManager.instance.playerDeadMenu;
        GameManager.instance.isPaused = true;
    }

    public void ResetHP() {
        HP = HPOrig;
    }
}
