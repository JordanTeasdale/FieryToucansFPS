using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class GunStats : ScriptableObject {
    [SerializeField] public int shootDistance;
    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
}
