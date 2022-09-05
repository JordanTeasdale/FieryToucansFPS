using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class BowScript : WeaponBase
{
    float power = 1;
    public override IEnumerator ShootPrimary() {
        currentAmmo--;
        GameManager.instance.playerScript.isShooting = true;
        while (Input.GetMouseButton(0)) { 
        power += Time.deltaTime;
            /*if (power >= 10000) {
                break;
            }*/
        }
        Debug.Log(power);
        GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, bulletPrimary.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.forward).normalized * (shootSpeedPrimary * (power/10000));
        bulletPrimary.GetComponent<Bullet>().damage = (int)(bulletPrimary.GetComponent<Bullet>().damage * (power / 10000));
        yield return new WaitForSeconds(shootRatePrimary);
        power = 1;
        GameManager.instance.playerScript.isShooting = false;
    }
}
