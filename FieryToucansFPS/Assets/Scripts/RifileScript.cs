using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RifileScript : WeaponBase {
   /* [SerializeField] public Animator animator;

    
    private bool isScoped = false;
    Scope scope;
    

    public override IEnumerator ShootSecondary() {
        Debug.Log("Entering");
        scope.mainCamera = GameManager.instance.playerScript.cameraMain;

        if (Input.GetButton("Fire2")) {
           
            isScoped = true;
            animator.SetBool("Scoped", isScoped);

            if (isScoped && GameManager.instance.playerScript.weaponIndex == gunIndex) {
                //StartCoroutine(OnScoped());
                scope.OnScoped();

                if (Input.GetButton("Shoot")) {
                    GameManager.instance.playerScript.isShooting = true;
                    currentAmmo--;

                    GameObject bulletClone = Instantiate(bulletSecondary, GameManager.instance.gunPosition.transform.position, bulletSecondary.transform.rotation);
                    bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.gunPosition.transform.forward).normalized * shootSpeedSecondary;
                    yield return new WaitForSeconds(shootRateSecondary);

                    GameManager.instance.playerScript.isShooting = false;
                }
                
            }
        } else {
            isScoped = false;
            scope.OnUnscoped();
        }      
    }

    public void ADS() {

        if (Input.GetButton("Fire2")) {


        }
    }
   */


  
}
