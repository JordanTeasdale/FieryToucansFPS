using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponBase : ScriptableObject {
    public int shootSpeedPrimary;
    public float shootRatePrimary;

    public int shootSpeedSecondary;
    public float shootRateSecondary;
    public float secondaryCooldown;

    [Range(0, .5f)] public float spreadFactor;

    [SerializeField] AudioSource gunAud;
    public AudioClip shootSound;

    [Range(0, 1)][SerializeField] public float shootVol;
    public AudioClip reloadSound;
    [Range(0, 1)][SerializeField] public float reloadVol;
    public GameObject hitEffect;
    public Sprite Crosshair;
    public int maxAmmo;
    public int currentAmmo;
    public GameObject gun;
    public int gunIndex;
    public GameObject bulletPrimary;
    public GameObject bulletSecondary;
    public bool secondaryFireActive;


    public Vector3 BulletSpread()
    {
        Vector3 shootDirection = GameManager.instance.gunPosition.transform.forward;
        shootDirection.x += Random.Range(-spreadFactor, spreadFactor);
        shootDirection.y += Random.Range(-spreadFactor, spreadFactor);
        return shootDirection;
    }

    public virtual IEnumerator ShootPrimary() {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;

        GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, bulletPrimary.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = BulletSpread().normalized * shootSpeedPrimary;
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
    }

    public virtual IEnumerator ShootSecondary() {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;

        GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.player.transform.position, bulletSecondary.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = BulletSpread().normalized * shootSpeedSecondary;
        yield return new WaitForSeconds(shootRateSecondary);
        GameManager.instance.playerScript.isShooting = false;
    }
}
