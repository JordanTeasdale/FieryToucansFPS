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
    [Range(1,100)] public int HP;

    [Header("----- Weapon Attributes -----")]

    [SerializeField] int shootDistance;
    [SerializeField] int shootDamage;
    [Range(0.1f, 5)][SerializeField] float switchRate;
    [SerializeField] float shootRate;
    [SerializeField] int currentAmmo;
    [SerializeField] List<GunStats> gunsList = new List<GunStats>();

    [Header("----- Effects -----")]
    [SerializeField] GameObject hitEffect;

    [Header("------Audio------")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] soundDamage;
    [Range(0, 1)][SerializeField] float soundDamageVol;
    [SerializeField] AudioClip soundShoot;
    [Range(0, 1)][SerializeField] float soundShootVol;
    [SerializeField] AudioClip[] soundFootsteps;
    [Range(0, 1)][SerializeField] float soundFootstepsVol;

    Vector3 playerVelocity;
    Vector3 move = Vector3.zero;

    float playerSpeedOrignal;
    int timesJumped;
    int HPOrig;
    int prevHP;
    float healthSmoothTime = 0.5f;
    float healthSmoothCount;
    float healthFillAmount;
    int weaponIndex = -1;
    int maxAmmo;

    bool isSprinting = false;
    bool isShooting = false;
    bool isSwitching = false;
    bool playFootsteps = true;
    // Start is called before the first frame update
    void Start() {
        playerSpeedOrignal = playerSpeed;
        HPOrig = HP;

        ResetHP();
    }

    // Update is called once per frame
    void Update() {
        //debug code
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(1);

        PlayerMovement();
        Sprint();
        Reload();

        StartCoroutine(footSteps());
        StartCoroutine(Shoot());
        StartCoroutine(WeaponCycle());
    }

    void FixedUpdate() {
        healthSmoothCount = System.Math.Min(healthSmoothTime, healthSmoothCount + Time.fixedDeltaTime);
        if (healthFillAmount != HP) {
            healthFillAmount = Mathf.Lerp(prevHP, HP, healthSmoothCount / healthSmoothTime);
            UpdateHP();
        }
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

    IEnumerator footSteps()
    {
        if (controller.isGrounded && move.normalized.magnitude > 0.3f && playFootsteps)
        {
            playFootsteps = false;
            aud.PlayOneShot(soundFootsteps[Random.Range(0, soundFootsteps.Length)], soundFootstepsVol);
            if (isSprinting)
            {
                yield return new WaitForSeconds(0.3f);
            }
            else
                yield return new WaitForSeconds(0.4f);
            playFootsteps = true;
        }
    }

    void Sprint() {

        if (Input.GetButtonDown("Sprint")) {
            isSprinting = true;
            playerSpeed = playerSpeed * sprintMult;
        }

        if (Input.GetButtonUp("Sprint")) {
            isSprinting = false;
            playerSpeed = playerSpeedOrignal;
        }
    }

    public void GunPickup( GunStats _stats) {
        GunEquip(_stats);
        gunsList.Add(_stats);
        _stats.currentAmmo = _stats.maxAmmo;
        weaponIndex++;
    }

    public void GunEquip(GunStats _gun)
    {
        shootDamage = _gun.shootDamage;
        shootDistance = _gun.shootDistance;
        shootRate = _gun.shootRate;
        soundShoot = _gun.shootSound;
        soundShootVol = _gun.shootVol;
        hitEffect = _gun.hitEffect;
        maxAmmo = _gun.maxAmmo;
        currentAmmo = _gun.currentAmmo;
        UpdatedAmmoGUI();
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
                weaponIndex = gunsList.Count - 1;
                GunEquip(gunsList[weaponIndex]);
            }
        }
    }

    IEnumerator WeaponCycle() {
        if (gunsList.Count > 0 && Input.GetAxis("Mouse ScrollWheel") != 0 && !isSwitching) {
            isSwitching = true;
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                weaponIndex++;
                if (weaponIndex == gunsList.Count)
                    weaponIndex = 0;
                GunEquip(gunsList[weaponIndex]);
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                weaponIndex--;
                if (weaponIndex < 0)
                    weaponIndex = gunsList.Count - 1;
                GunEquip(gunsList[weaponIndex]);
            }
            yield return new WaitForSeconds(switchRate);
            isSwitching = false;
        }
    }

    public void TakeDamage(int _dmg) {
        //healthSmoothCount = 0;
        if (healthFillAmount == HP) {
            healthSmoothCount = 0;
            prevHP = HP;
        }
        HP -= _dmg;
        aud.PlayOneShot(soundDamage[Random.Range(0, soundDamage.Length)], soundDamageVol);
        //UpdateHP();
        StartCoroutine(DamageFlash());
        if (HP <= 0) {
            // Kill the player
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

        if (Input.GetButton("Shoot") && !isShooting && gunsList.Count > 0 && currentAmmo > 0) {
            isShooting = true;
            gunsList[weaponIndex].currentAmmo--;
            currentAmmo--;
            UpdatedAmmoGUI();
            RaycastHit hit;
            //aud.PlayOneShot(soundShoot[Random.Range(0, soundShoot.Length)], soundShootVol);
            aud.PlayOneShot(soundShoot, soundShootVol);
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance)) {
                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
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

    void Reload() {
        if (Input.GetButtonDown("Reload")) {
            currentAmmo = maxAmmo;
            gunsList[weaponIndex].currentAmmo = maxAmmo;
            UpdatedAmmoGUI();
        }
    }

    private void UpdatedAmmoGUI() {
        GameManager.instance.ammoStockGUI.GetComponent<TMPro.TMP_Text>().text = maxAmmo.ToString();
        GameManager.instance.ammoMagGUI.GetComponent<TMPro.TMP_Text>().text = currentAmmo.ToString();
    }

    public void Respawn() {
        healthSmoothCount = 0;
        controller.enabled = false;
        transform.position = GameManager.instance.RespawnPos.transform.position;
        controller.enabled = true;
        if (weaponIndex != -1) {
            currentAmmo = maxAmmo;
            gunsList[weaponIndex].currentAmmo = maxAmmo;
            UpdatedAmmoGUI();
        }
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

    public void UpdateHP() {
        GameManager.instance.playerHPBar.fillAmount = healthFillAmount / HPOrig;
    }
}
