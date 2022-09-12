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
    public int secondaryAmmoConsumption;

    [Range(0, .5f)] public float spreadFactor;

    [SerializeField] AudioSource gunAud;
    public AudioClip shootSound;
    [Range(0, 1)][SerializeField] public float shootVol;
    public AudioClip shootSoundSecondary;
    [Range(0, 1)][SerializeField] public float shootVolSecondary;

    public GameObject hitEffect;
    public Sprite Crosshair;
    public int maxAmmo;
    public int currentAmmo;
    public GameObject gun;
    public int gunIndex;
    public GameObject bulletPrimary;
    public GameObject bulletSecondary;
    [HideInInspector] public bool secondaryFireActive;

    public Vector3 BulletSpread()
    {
        //Vector3 shootDirection = GameManager.instance.gunPosition.transform.forward;
        Vector3 shootDirection = ShootDirection();
        shootDirection.x += Random.Range(-spreadFactor, spreadFactor);
        shootDirection.y += Random.Range(-spreadFactor, spreadFactor);
        return shootDirection;
    }

    public Vector3 ShootDirection()
    {
        Vector3 shootDestination; //where the cursor is pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //a ray from the center of the player camera
        RaycastHit hit; //the point along the ray that connects with an object
        if(Physics.Raycast(ray, out hit))
            shootDestination = hit.point;
        else
            shootDestination = ray.GetPoint(1000);

        Vector3 shootDirection = (shootDestination - GameManager.instance.gunPosition.transform.position).normalized;
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

    public virtual void SecondaryFireMode()
    {

    }

    public virtual IEnumerator ShootSecondary()
    {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo -= secondaryAmmoConsumption;

        GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, bulletSecondary.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = BulletSpread().normalized * shootSpeedSecondary;
        yield return new WaitForSeconds(shootRateSecondary);
        GameManager.instance.playerScript.isShooting = false;
    }
}
