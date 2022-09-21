using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSounds : MonoBehaviour
{
    [SerializeField] AudioSource enemyAud;
    public AudioClip stabSound, biteSound;

    private void PlayHitSound()
    {
        enemyAud.PlayOneShot(stabSound);

    }
    private void PlayBiteSound()
    {
        enemyAud.PlayOneShot(biteSound);
    }
}
