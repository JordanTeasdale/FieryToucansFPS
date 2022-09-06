using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class HandCannonScript : WeaponBase
{
    public float secondarySpread;
    public int secondaryFireAmmount;

    public override IEnumerator ShootSecondary()
    {
        for (int i = 0; i < secondaryFireAmmount; i++)
        {
            GameManager.instance.playerScript.isShooting = true;
            currentAmmo--;

            GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, bulletSecondary.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = SecondarySpread().normalized * shootSpeedSecondary;
            yield return new WaitForSeconds(shootRateSecondary);
        }
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
    }

    public Vector3 SecondarySpread()
    {
        Vector3 shootDirection = GameManager.instance.gunPosition.transform.forward;
        shootDirection.x += Random.Range(-secondarySpread, secondarySpread);
        shootDirection.y += Random.Range(-secondarySpread, secondarySpread);
        return shootDirection;
    }
}

