using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MissileAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;
    [SerializeField] Animator anim;
 

    [Header("----- Enemy Stats -----")]
    [Range(0, 100)] public int HP;
    [Range(0, 10)][SerializeField] int playerFaceSpeed;
    [Range(1, 180)][SerializeField] int fieldOfView;
    [Range(1, 180)][SerializeField] int fieldOfViewShoot;
    [Range(1, 20)][SerializeField] float speedChase;

    [Header("----- Effects -----")]
    [SerializeField] GameObject executeEffect;

    Vector3 playerDir;
    bool playerInRange = false;
    public bool isExecutable = false;


    float stoppingDistanceOrig;

    Vector3 startingPos;
    Vector3 raycastPos;


    // Start is called before the first frame update
    void Start() {
        
        startingPos = transform.position;
        //Roam();
    }

    // Update is called once per frame
    void Update() {
        raycastPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (agent.isActiveAndEnabled && !anim.GetBool("Dead")) {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));
            playerDir = GameManager.instance.player.transform.position - raycastPos;

            if (playerInRange) {
                CanSeePlayer();
            }         
            //Debug.Log(agent.remainingDistance);
        }
    }

   

    void FacePlayer() {
        if (agent.remainingDistance <= agent.stoppingDistance) {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    void CanSeePlayer() {


        float angle = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), new Vector3(transform.forward.x, 0, transform.forward.z));
        //Debug.Log(angle);

        RaycastHit hit;
        if (Physics.Raycast(raycastPos, playerDir, out hit)) {
            Debug.DrawRay(raycastPos, playerDir);

            if (hit.collider.CompareTag("Player") && angle <= fieldOfView) {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;
                agent.speed = speedChase;
                FacePlayer();

            
            }    
        }
    }
    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            playerInRange = false;
      
    }

}
