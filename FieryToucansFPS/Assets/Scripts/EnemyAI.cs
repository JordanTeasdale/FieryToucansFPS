using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;
    [SerializeField] Animator anim;
    [SerializeField] GameObject healthDrop;
    [SerializeField] GameObject ammoDrop;

    [Header("----- Enemy Stats -----")]
    [Range(0, 100)] public int HP;
    [Range(0, 10)] [SerializeField] int playerFaceSpeed;
    [Range(1, 180)] [SerializeField] int fieldOfView;
    [Range(1, 180)] [SerializeField] int fieldOfViewShoot;
    [Range(1, 180)] [SerializeField] int roamRadius;
    [Range(1, 20)] [SerializeField] float speedRoam;
    [Range(1, 20)] [SerializeField] float speedChase;
 

    [Header("----- Weapons Stats -----")]
    [Range(0.1f, 5)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int damage;
    [Range(1, 10)] [SerializeField] int speed;
    [Range(1, 5)] [SerializeField] int bulletDestroyTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawnPos;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource enemyAud;
    public AudioClip soundExecute;
    [Range(0,1)] [SerializeField] float soundExecuteVol;

    [Header("----- Effects -----")]
    [SerializeField] GameObject executeEffect;

    Vector3 playerDir;
    bool isShooting = false;
    bool playerInRange = false;
    public bool isExecutable = false;
    private int HPOrig;
    

    float stoppingDistanceOrig;

    Vector3 startingPos;
    Vector3 raycastPos;


    // Start is called before the first frame update
    void Start() {
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        HPOrig = HP;
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
            if (!agent.pathPending && agent.remainingDistance == 0)
                Roam();
            //Debug.Log(agent.remainingDistance);
        }     
    }

    void Roam() {
        agent.stoppingDistance = 0;
        agent.speed = speedRoam;
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, roamRadius, 1);
        if (hit.hit) {
            NavMeshPath path = new NavMeshPath();

            agent.SetDestination(hit.position);
            agent.CalculatePath(hit.position, path);
            agent.SetPath(path);

        }
    }

    void FacePlayer() {
        if (agent.remainingDistance <= agent.stoppingDistance) {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    public void TakeDamage(int damage) {

        if (anim.GetBool("Dead") == false)  {
            if (isExecutable && GameManager.instance.playerScript.isMeleeing == true)
            {
                Execute();
            }
            else
            {
                HP -= damage;
                if (HP > 0)  {
                    anim.SetTrigger("Damage");
                    StartCoroutine(FlashColor());
                    StartCoroutine(Executable());
                }
                else  {
                    GameManager.instance.currentRoom.GetComponent<LevelSpawner>().EnemyKilled();
                    anim.SetBool("Dead", true);
                    agent.enabled = false;

                    foreach (Collider col in GetComponents<Collider>())
                        col.enabled = false;
                }
            }
        }
    }

    private void Execute()
    {
        //add code for health and amo drops
        Instantiate(executeEffect, gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(healthDrop, gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(ammoDrop, gameObject.transform.position, gameObject.transform.rotation);
        enemyAud.PlayOneShot(soundExecute, soundExecuteVol);
        Destroy(gameObject);
    }

    IEnumerator Executable()
    {
        while(HP <= HPOrig/4)
        {
            isExecutable = true;
            rend.material.color = Color.yellow;
            yield return new WaitForSeconds(1);
            rend.material.color = Color.cyan;
            yield return new WaitForSeconds(1);
        }
        isExecutable = false;
    }

    IEnumerator FlashColor() {
        rend.material.color = Color.red;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        agent.speed = speedChase;
        agent.SetDestination(GameManager.instance.player.transform.position);
        agent.stoppingDistance = 0;
        rend.material.color = Color.white;
    }

    IEnumerator Shoot() {
        isShooting = true;

        anim.SetTrigger("Shoot");
        
        GameObject bulletClone = Instantiate(bullet, bulletSpawnPos.transform.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
        yield return new WaitForSeconds(shootRate);
        
        isShooting = false;
    }

    void CanSeePlayer() {


        float angle = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), new Vector3(transform.forward.x, 0, transform.forward.z));
        //Debug.Log(angle);

        RaycastHit hit;
        if (Physics.Raycast(raycastPos, playerDir, out hit))  {
            Debug.DrawRay(raycastPos, playerDir);

            if (hit.collider.CompareTag("Player") && angle <= fieldOfView)   {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;
                agent.speed = speedChase;
                FacePlayer();

                if (!isShooting && angle <= fieldOfViewShoot)
                {
                    StartCoroutine(Shoot());
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)  {

        if (other.CompareTag("Player"))  {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)  {
        if (other.CompareTag("Player"))
            playerInRange = false;
        Roam();
    }
}
