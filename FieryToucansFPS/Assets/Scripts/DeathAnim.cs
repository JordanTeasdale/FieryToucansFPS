using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnim : MonoBehaviour
{
    public Animator animator;

    private bool isDead = true;

    void Update() {

        animator.SetBool("isDead", isDead);
        
    }




}
