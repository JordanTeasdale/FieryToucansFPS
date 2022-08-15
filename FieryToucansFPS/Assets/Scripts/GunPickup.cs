using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gunStat;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.instance.playerScript.GunPickup(gunStat);
            Destroy(gameObject);
        }
    }
}
