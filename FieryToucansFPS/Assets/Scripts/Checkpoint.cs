using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float TempAudCVol;
    [SerializeField] AudioSource TempAudio;
    [SerializeField] AudioClip TempAudioC;
    float timer;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.instance.RespawnPos.transform.position = transform.position;           
            Destroy(gameObject);
            StartCoroutine(CheckpointFeedbackTimer());
            
        }
    }

    IEnumerator CheckpointFeedbackTimer() {
        TempAudio.PlayOneShot(TempAudioC, TempAudCVol);
        GameManager.instance.checkpointFeedback.SetActive(true);       
        yield return new WaitForSeconds(0.5f);       
        GameManager.instance.checkpointFeedback.SetActive(false);
    }

}
