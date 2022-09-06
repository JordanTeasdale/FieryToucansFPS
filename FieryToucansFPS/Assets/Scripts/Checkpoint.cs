using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] AudioSource TempAudio;
    [SerializeField] AudioClip TempAudioC;
    float timer;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            TempAudio.PlayOneShot(TempAudioC, 1);
            GameManager.instance.RespawnPos.transform.position = transform.position;           
            Destroy(gameObject);
            StartCoroutine(CheckpointFeedbackTimer());
        }
    }

    IEnumerator CheckpointFeedbackTimer()
    {
        GameManager.instance.checkpointFeedback.SetActive(true);
        Debug.Log("Into IEnum");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Sucess?");
        GameManager.instance.checkpointFeedback.SetActive(false);
    }

}
