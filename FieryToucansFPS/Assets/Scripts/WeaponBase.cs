using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ScriptableObject {
    public int shootSpeedPrimary;
    public float shootRatePrimary;

    public int shootSpeedSecondary;
    public float shootRateSecondary;
    public float secondaryCooldown;

    [SerializeField] AudioSource gunAud;
    public AudioClip shootSound;

    [Range(0, 1)][SerializeField] public float shootVol;
    public AudioClip reloadSound;
    [Range(0, 1)][SerializeField] public float reloadVol;
    public GameObject hitEffect;

    public int maxAmmo;
    public int currentAmmo;
    public GameObject gun;
    public int gunIndex;
    public GameObject bulletPrimary;
    public GameObject bulletSecondary;
    public bool secondaryFireActive;

    public virtual IEnumerator ShootPrimary() {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;

        GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, bulletPrimary.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * shootSpeedPrimary;
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
    }

    public IEnumerator ShootSecondary() {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;

        GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.player.transform.position, bulletSecondary.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * shootSpeedSecondary;
        yield return new WaitForSeconds(shootRateSecondary);
        GameManager.instance.playerScript.isShooting = false;
    }
}
