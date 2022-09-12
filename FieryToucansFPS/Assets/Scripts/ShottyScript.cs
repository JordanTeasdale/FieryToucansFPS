using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ShottyScript : WeaponBase
{
    [Range(6, 15)] public int numShottyProjectiles;
    public float secondarySpread;

    public override IEnumerator ShootPrimary() {
        
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;
        for (int i = 0; i < numShottyProjectiles; i++) {

            GameObject pellet = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, bulletPrimary.transform.rotation);
            pellet.GetComponent<Rigidbody>().velocity = BulletSpread().normalized * shootSpeedPrimary;

        }
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
    }
    public Vector3 SecondarySpread()
    {
        Vector3 shootDirection = GameManager.instance.gunPosition.transform.forward;
        shootDirection.x += Random.Range(-secondarySpread, secondarySpread);
        shootDirection.y += Random.Range(-secondarySpread, secondarySpread);
        shootDirection.z += Random.Range(-secondarySpread, secondarySpread);
        return shootDirection;
    }

    public override IEnumerator ShootSecondary()
    {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;
        for (int i = 0; i < numShottyProjectiles; i++)
        {

            GameObject pellet = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, bulletSecondary.transform.rotation);
            pellet.GetComponent<Rigidbody>().velocity = SecondarySpread().normalized * shootSpeedSecondary;

        }
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
    }
}
