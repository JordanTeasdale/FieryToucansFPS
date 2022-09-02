using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] GameObject doorPort;
    [SerializeField] GameObject doorStarboard;
    public bool isUnlocked = false;
    bool playerInRange = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnlocked && playerInRange) {
            doorPort.transform.eulerAngles = Vector3.Lerp(doorPort.transform.eulerAngles, new Vector3(0, 80, 0), Time.deltaTime * 2);
            doorStarboard.transform.eulerAngles = Vector3.Lerp(doorStarboard.transform.eulerAngles, new Vector3(0, 80, 0), Time.deltaTime * 2);
        }
        if (isUnlocked && !playerInRange) {
            doorPort.transform.eulerAngles = Vector3.Lerp(doorPort.transform.eulerAngles, new Vector3(0, 0, 0), Time.deltaTime * 2);
            doorStarboard.transform.eulerAngles = Vector3.Lerp(doorStarboard.transform.eulerAngles, new Vector3(0, 0, 0), Time.deltaTime * 2);
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
}
