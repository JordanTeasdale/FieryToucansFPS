using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class GunStats : ScriptableObject {
    public int shootDistance;
    public int shootDamage;
    public float shootRate;
    [SerializeField] AudioSource gunAud;
    public AudioClip shootSound;
    [Range(0, 1)][SerializeField] public float shootVol;
    public AudioClip reloadSound;
    [Range(0, 1)][SerializeField] public float reloadVol;
    public GameObject hitEffect;
    public int maxAmmo;
    public int currentAmmo;


}
