using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSounds : MonoBehaviour
{
    [SerializeField] AudioSource enemyAud;
    public AudioClip[] bites;
    public AudioClip meleeAttack, flames;
    [Range(0, 1)] [SerializeField] float soundVol;

    private void PlayHitSound()
    {
        enemyAud.PlayOneShot(meleeAttack, soundVol);
    }
    private void PlayBiteSound()
    {
        enemyAud.PlayOneShot(bites[Random.Range( 0, bites.Length)], soundVol);
    }
}
