using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public int shootDistancePrimary;
    public int shootSpeedPrimary;
    public int shootDamagePrimary;
    public float shootRatePrimary;

    public int shootDistanceSecondary;
    public int shootSpeedSecondary;
    public int shootDamageSecondary;
    public float shootRateSecondary;

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

    public void ShootPrimary() {
            currentAmmo--;

            GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, bulletPrimary.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * shootSpeedPrimary;
    }

    public void ShootSecondary() {
            currentAmmo--;

            GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.player.transform.position, bulletSecondary.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * shootSpeedSecondary;

    }
}
