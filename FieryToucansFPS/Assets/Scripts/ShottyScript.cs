using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ShottyScript : WeaponBase
{
    [Range(6, 15)] public int numShottyProjectiles;
    [Range(0, .5f)] public float primaryPelletSpread;
    List<GameObject> projectiles;

    public override IEnumerator ShootPrimary() {
        
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;
        projectiles.Clear();
        for (int i = 0; i < numShottyProjectiles; i++) {

            GameObject pellet = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, bulletSecondary.transform.rotation);
            pellet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            projectiles.Add(pellet);

        }

        foreach(GameObject pellet in projectiles) {

            pellet.GetComponent<Rigidbody>().velocity = PrimarySpray().normalized * shootSpeedPrimary;
           
        }
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;


    }

    public Vector3 PrimarySpray() {

        Vector3 muzzel = GameManager.instance.gunPosition.transform.forward;
        muzzel.x += Random.Range(-primaryPelletSpread, primaryPelletSpread);
        muzzel.y += Random.Range(-primaryPelletSpread, primaryPelletSpread);
        return muzzel;
    }

}
