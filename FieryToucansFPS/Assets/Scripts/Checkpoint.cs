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
            TempAudio.PlayOneShot(TempAudioC, TempAudCVol);
            GameManager.instance.RespawnPos.transform.position = transform.position;           
            GameManager.instance.checkpointFeedback.SetActive(false);
            StartCoroutine(CheckpointFeedbackTimer());
        }
    }

    IEnumerator CheckpointFeedbackTimer() {
        GameManager.instance.checkpointFeedback.SetActive(true);       
        yield return new WaitForSeconds(2.5f);
        GameManager.instance.checkpointFeedback.SetActive(false);
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

}
