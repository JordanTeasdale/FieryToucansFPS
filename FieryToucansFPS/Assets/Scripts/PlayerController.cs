using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable {
    [Header("----- Componets -----")]

    [SerializeField] CharacterController controller;

    [Header("----- Player Attributes -----")]

    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMult;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int maxJumps;
    [Range(1, 100)] public int HP;
    [SerializeField] float invincibilityTimer;
    [SerializeField] GameObject meleeHitbox;
    [SerializeField] int meleeDamage;
    [SerializeField] float meleeSpeed;

    [Header("----- Weapon Attributes -----")]

    [SerializeField] int shootDistance;
    [SerializeField] int shootDamage;
    [Range(0.1f, 5)][SerializeField] float switchRate;
    [SerializeField] float shootRate;
    [SerializeField] int currentAmmo;
    public List<WeaponBase> gunsList = new List<WeaponBase>();
    [SerializeField] GunStats empty;
    [SerializeField] Transform gunPostion;

    [Header("----- Effects -----")]
    [SerializeField] GameObject hitEffect;

    [Header("------Audio------")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] soundDamage;
    [Range(0, 1)][SerializeField] float soundDamageVol;
    [SerializeField] AudioClip soundShoot;
    [Range(0, 1)][SerializeField] float soundShootVol;
    //public AudioClip soundReload;
    //[Range(0, 1)][SerializeField] public float soundReloadVol;
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
    public int weaponIndex = -1;
    int maxAmmo;
    float damageTimer;

    bool isSprinting = false;
    public bool isShooting = false;
    bool isSwitching = false;
    public bool isMeleeing = false;
    bool playFootsteps = true;
    //bool isScoped = false;
    public CameraShake cameraShake;

    // Start is called before the first frame update
    void Start() {
        playerSpeedOrignal = playerSpeed;
        HPOrig = HP;
        cameraShake = gameObject.GetComponentInChildren<CameraShake>();
        ResetHP();
        for (int i = 0; i < 6; ++i) {
            gunsList.Add(empty);
        }
    }

    // Update is called once per frame
    void Update() {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
        //debug code
        if (Input.GetKeyDown(KeyCode.K)) {
            TakeDamage(1);
            StartCoroutine(cameraShake.Shake(0.15f, 0.4f));

        }


        PlayerMovement();
        Sprint();

        StartCoroutine(FootSteps());
        Shoot();
        StartCoroutine(WeaponCycle());
        StartCoroutine(Melee());
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
        if (controller.isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = 0.0f;
            timesJumped = 0;

        }

        //Getting input from Unity Input Manager
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        //Adding the move vector to the character controller
        controller.Move(move * playerSpeed * Time.deltaTime);

        //Jumping functionallity
        if (Input.GetButtonDown("Jump") && timesJumped < maxJumps) {
            playerVelocity.y = jumpHeight;
            timesJumped++;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator FootSteps() {
        if (controller.isGrounded && move.normalized.magnitude > 0.3f && playFootsteps) {
            playFootsteps = false;
            aud.PlayOneShot(soundFootsteps[Random.Range(0, soundFootsteps.Length)], soundFootstepsVol);
            if (isSprinting) {
                yield return new WaitForSeconds(0.3f);
            } else
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
    public void AmmoPickup(int index = -1, int ammo = -1) {
        if (ammo == -1) {
            if (index == -1) {
                for (int i = 0; i < gunsList.Count; ++i) {
                    if (gunsList[i].name != "Gun - Empty") {
                        gunsList[i].currentAmmo += (gunsList[i].maxAmmo / 10);
                        if (gunsList[i].currentAmmo > gunsList[i].maxAmmo) {
                            gunsList[i].currentAmmo = gunsList[i].maxAmmo;
                        }
                    }
                }
            } else {
                gunsList[index].currentAmmo += (gunsList[index].maxAmmo / 10);
                if (gunsList[index].currentAmmo > gunsList[index].maxAmmo) {
                    gunsList[index].currentAmmo = gunsList[index].maxAmmo;
                }
            }
        } else {
            if (index == -1) {
                for (int i = 0; i < gunsList.Count; ++i) {
                    if (gunsList[i].name != "Gun - Empty") {
                        gunsList[i].currentAmmo += ammo;
                        if (gunsList[i].currentAmmo > gunsList[i].maxAmmo) {
                            gunsList[i].currentAmmo = gunsList[i].maxAmmo;
                        }
                    }
                }
            } else {
                gunsList[index].currentAmmo += ammo;
                if (gunsList[index].currentAmmo > gunsList[index].maxAmmo) {
                    gunsList[index].currentAmmo = gunsList[index].maxAmmo;
                }
            }
        }
        GunEquip(gunsList[weaponIndex]);
    }

    public void GunPickup(WeaponBase _stats) {
        _stats.currentAmmo = _stats.maxAmmo;
        gunsList[_stats.gunIndex] = _stats;
        weaponIndex = _stats.gunIndex;
        GunEquip(_stats);
    }

    public void GunEquip(WeaponBase _gun) {
        soundShoot = _gun.shootSound;
        soundShootVol = _gun.shootVol;
        hitEffect = _gun.hitEffect;
        maxAmmo = _gun.maxAmmo;
        currentAmmo = _gun.currentAmmo;
        UpdatedAmmoGUI();
        Destroy(GameObject.FindGameObjectWithTag("Gun Model"));
        Instantiate(_gun.gun, gunPostion.position, gunPostion.rotation, gunPostion);

    }

    IEnumerator WeaponCycle() {
        if (gunsList.Count > 0 && Input.GetAxis("Mouse ScrollWheel") != 0 && !isSwitching) {
            isSwitching = true;
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                RotateUp();
                GunEquip(gunsList[weaponIndex]);
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                RotateDown();
                GunEquip(gunsList[weaponIndex]);
            }
            yield return new WaitForSeconds(switchRate);
            isSwitching = false;
        }
    }
    void RotateUp() {
        ++weaponIndex;
        if (weaponIndex == gunsList.Count) {
            weaponIndex = 0;
        }
        while (gunsList[weaponIndex].name == empty.name) {
            ++weaponIndex;
            if (weaponIndex == gunsList.Count)
                weaponIndex = 0;
        }
    }

    void RotateDown() {
        --weaponIndex;
        if (weaponIndex < 0) {
            weaponIndex = gunsList.Count - 1;
        }
        while (gunsList[weaponIndex].name == empty.name) {
            weaponIndex--;
            if (weaponIndex < 0)
                weaponIndex = gunsList.Count - 1;
        }
    }

    public void TakeDamage(int _dmg) {
        if (damageTimer <= 0) {
            damageTimer = invincibilityTimer;
            if (healthFillAmount == HP) {
                healthSmoothCount = 0;
                prevHP = HP;
            }
            HP -= _dmg;
            StartCoroutine(cameraShake.Shake(0.15f, 0.4f));
            aud.PlayOneShot(soundDamage[Random.Range(0, soundDamage.Length)], soundDamageVol);
            //UpdateHP();
            StartCoroutine(DamageFlash());
            if (HP <= 0) {
                // Kill the player
                Death();
            }
        }
    }

    IEnumerator DamageFlash() {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }

    IEnumerator Melee() {
        if (Input.GetButton("Melee") && !isMeleeing) {
            isMeleeing = true;
            meleeHitbox.SetActive(true);
            yield return new WaitForSeconds(meleeSpeed);
            meleeHitbox.SetActive(false);
            isMeleeing = false;
        }
    }

    void Shoot() {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.000001f); //makes a visible line to visualize the shoot ray

        if (Input.GetButton("Shoot") && !isShooting && gunsList.Count > 0 && currentAmmo > 0) {
            aud.PlayOneShot(soundShoot, soundShootVol);
            if (Input.GetButton("Fire2"))
                StartCoroutine(gunsList[weaponIndex].ShootSecondary());
            else
                StartCoroutine(gunsList[weaponIndex].ShootPrimary());
            UpdatedAmmoGUI();
            gunPostion.GetChild(0).GetComponent<Animation>().Play();
        }
    }

    private void UpdatedAmmoGUI() {
        maxAmmo = gunsList[weaponIndex].maxAmmo;
        currentAmmo = gunsList[weaponIndex].currentAmmo;
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
