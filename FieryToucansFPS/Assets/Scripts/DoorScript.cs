using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    [SerializeField] GameObject doorPort;
    [SerializeField] GameObject doorStarboard;
    public bool isUnlocked = false;
    bool playerInRange = false;
    [Range(0,1)][SerializeField] float doorAudCVol;
    [SerializeField] AudioSource doorAudio;
    [SerializeField] AudioClip doorLockingAudioC;
    [SerializeField] AudioClip doorUnlockingAudioC;

    // Update is called once per frame
    void Update() {
        if (isUnlocked && playerInRange) {
            doorPort.transform.localEulerAngles = Vector3.Lerp(doorPort.transform.localEulerAngles, new Vector3(0, 80, 0), Time.deltaTime * 2);
            doorStarboard.transform.localEulerAngles = Vector3.Lerp(doorStarboard.transform.localEulerAngles, new Vector3(0, 80, 0), Time.deltaTime * 2);
        }
        if (!playerInRange) {
            doorPort.transform.localEulerAngles = Vector3.Lerp(doorPort.transform.localEulerAngles, new Vector3(0, 0, 0), Time.deltaTime * 4);
            doorStarboard.transform.localEulerAngles = Vector3.Lerp(doorStarboard.transform.localEulerAngles, new Vector3(0, 0, 0), Time.deltaTime * 4);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }

    public void DoorLocking() {
        doorAudio.PlayOneShot(doorLockingAudioC, doorAudCVol);
    }

    public void DoorUnlocking() {
        doorAudio.PlayOneShot(doorUnlockingAudioC, doorAudCVol);
    }
}
