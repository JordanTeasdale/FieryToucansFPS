using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RifileScript : WeaponBase
{
    public Animator animator;

    
    private bool isScoped = false;
    Scope scope;
    

    public override IEnumerator ShootSecondary() {

        scope.mainCamera = GameManager.instance.playerScript.cameraMain;

        if (Input.GetButton("Fire2")) {
            isScoped = !isScoped;
            animator.SetBool("Scope", isScoped);

            if (isScoped && GameManager.instance.playerScript.weaponIndex == gunIndex) {
                //StartCoroutine(OnScoped());
                scope.OnScoped();

                if (Input.GetButton("Shoot")) {

                    GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, bulletSecondary.transform.rotation);
                    bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.gunPosition.transform.forward).normalized * shootSpeedSecondary;
                    yield return new WaitForSeconds(shootRateSecondary);
                }
                yield return new WaitForSeconds(.015f);
            }
        } 
        else 
            scope.OnUnscoped();
        
    }

  
}
