using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class BowScript : WeaponBase {
    float power = 5;
    public override IEnumerator ShootPrimary() {
        currentAmmo--;
        GameManager.instance.playerScript.isShooting = true;
        while (Input.GetMouseButton(0)) {
            if (power < 205)
                power += 1;
            yield return null;
        }
        Debug.Log(power);
        GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, GameManager.instance.player.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * (shootSpeedPrimary * (power / 250));
        bulletPrimary.GetComponent<Bullet>().damage = (int)(bulletPrimary.GetComponent<Bullet>().damage * (power / 250));
        yield return new WaitForSeconds(shootRatePrimary);
        power = 5;
        GameManager.instance.playerScript.isShooting = false;
    }

    public override IEnumerator ShootSecondary() {
        for (int i = 0; i < 4; i++) {
            GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, GameManager.instance.player.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * shootSpeedSecondary; 
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(shootRateSecondary);
    }
}
