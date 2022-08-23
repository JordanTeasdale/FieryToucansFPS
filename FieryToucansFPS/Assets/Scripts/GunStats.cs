using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class GunStats : ScriptableObject {
    [SerializeField] public int shootDistance;
    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
    [SerializeField] AudioSource gunAud;
    [SerializeField] AudioClip shoot;
    [Range(0, 1)][SerializeField] float shootVol;
    [SerializeField] AudioClip reload;
    [Range(0, 1)][SerializeField] float reloadVol;

}
