using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class GunStats : ScriptableObject {
    [SerializeField] public int shootDistance;
    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
    [SerializeField] AudioSource gunAud;
    [SerializeField] public AudioClip shootSound;
    [Range(0, 1)][SerializeField] public float shootVol;
    [SerializeField] public AudioClip reloadSound;
    [Range(0, 1)][SerializeField] public float reloadVol;
    [SerializeField] public GameObject hitEffect;


}
