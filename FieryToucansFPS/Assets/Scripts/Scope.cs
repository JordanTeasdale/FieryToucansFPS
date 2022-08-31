using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
  public Animator animator;

    private bool isScoped = false;

     void Update() {

        if (Input.GetButton("Fire2")) {
            isScoped = !isScoped;
            animator.SetBool("Scope", isScoped );
        }
    }

}
