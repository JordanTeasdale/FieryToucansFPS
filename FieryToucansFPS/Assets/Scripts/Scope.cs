using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Animator animator;
    public Camera mainCamera;

    private bool isScoped = false;

    public float scopedFOV = 50f;
    private float normalFOV;

     void Update() {
       
        if (Input.GetButtonDown("Fire2")) {
            isScoped = !isScoped;
            animator.SetBool("Scope", isScoped );

            if (isScoped) {
                //StartCoroutine(OnScoped());
                OnScoped();
            }
            else 
               OnUnscoped();
        }


    }

    void OnUnscoped() {
        mainCamera.fieldOfView = normalFOV;  
    }
    //if clicked somethimes it double clicks and the animation doubles or it repeats itself without the intention to it
    //this is for wait time for this not to happen
   // IEnumerator OnScoped() { 
        //yield return new WaitForSeconds(0.5f);
        void OnScoped() { 
        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;
    }

}
