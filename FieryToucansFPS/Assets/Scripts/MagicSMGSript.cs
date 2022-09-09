using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class MagicSMGSript : WeaponBase {
    List<GameObject> clones;

    public override IEnumerator ShootPrimary() {
        GameManager.instance.playerScript.isShooting = true;
        currentAmmo--;
        GameObject bulletClone = Instantiate(bulletPrimary, GameManager.instance.gunPosition.transform.position, GameManager.instance.playerScript.cameraMain.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = BulletSpread().normalized * shootSpeedPrimary;
        clones.Add(bulletClone);
        bulletClone.GetComponent<MagicBulletScript>().Gimmy(this, clones.Count - 1);
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
    }

    public override IEnumerator ShootSecondary() {
        GameManager.instance.playerScript.isShooting = true;
        for (int i = 0; i < clones.Count; i++) {
            clones[i].GetComponent<MagicBulletScript>().Return();
        }
        yield return new WaitForSeconds(shootRatePrimary);
        GameManager.instance.playerScript.isShooting = false;
        //ClearList();
    }

    public void Remove(int id) {
        Debug.Log("Passed in ID: " + id);
        Debug.Log("List count before remove: " + clones.Count);
        clones.RemoveAt(id);
        Debug.Log("List count after remove: " + clones.Count);
        for (int i = id; i < clones.Count; i++) {
            if (clones[i] != null)
                clones[i].GetComponent<MagicBulletScript>().UpdateID();
        }
    }

    public void ClearList() {
        clones.Clear();
    }
}
