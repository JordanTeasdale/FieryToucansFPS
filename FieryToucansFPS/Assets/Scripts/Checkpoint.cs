using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] AudioSource TempAudio;
    [SerializeField] AudioClip TempAudioC;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            TempAudio.PlayOneShot(TempAudioC, 1);
            GameManager.instance.RespawnPos.transform.position = transform.position;           
            Destroy(gameObject);
        }
    }

    
}
