using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnim : MonoBehaviour {
    public Animator animator;

    void Update() {
        if (GameManager.instance.playerScript.HP <= 0)
            animator.SetBool("isDead", true);
        else if (GameManager.instance.playerScript.HP > 0)
            animator.SetBool("isDead", false);

    }
}