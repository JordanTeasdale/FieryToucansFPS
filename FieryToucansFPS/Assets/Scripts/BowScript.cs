using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class BowScript : WeaponBase {
    float power = 0.1f;
    public override IEnumerator ShootPrimary() {
        currentAmmo--;
        GameManager.instance.playerScript.isShooting = true;
        while (Input.GetMouseButton(0)) {
            if (power < 1.5f)
                power += Time.deltaTime;
            yield return null;
        }
        Debug.Log(power);
        GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, GameManager.instance.playerScript.cameraMain.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.playerScript.cameraMain.transform.forward).normalized * (shootSpeedPrimary * (power / 1.5f));
        bulletClone.GetComponent<Bullet>().damage = (int)(bulletPrimary.GetComponent<Bullet>().damage * (power / 1.5f));
        yield return new WaitForSeconds(shootRatePrimary);
        power = 0.1f;
        GameManager.instance.playerScript.isShooting = false;
    }

    public override IEnumerator ShootSecondary() {
        currentAmmo -= 3;
        GameManager.instance.playerScript.isShooting = true;
        for (int i = 0; i < 3; i++) {
            GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, GameManager.instance.playerScript.cameraMain.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.playerScript.cameraMain.transform.forward).normalized * shootSpeedSecondary; 
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(shootRateSecondary);
        GameManager.instance.playerScript.isShooting = false;
    }
}
