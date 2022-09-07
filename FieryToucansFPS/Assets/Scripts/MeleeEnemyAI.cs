using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : MonoBehaviour, IDamageable
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
    [Range(1, 180)] [SerializeField] int fieldOfViewMelee;
    [Range(1, 180)] [SerializeField] int roamRadius;
    [Range(1, 20)] [SerializeField] float speedRoam;
    [Range(1, 20)] [SerializeField] float speedChase;

    [Header("-----Melee Stats-----")]
    [Range(0, 10)] [SerializeField] int damage;
    [Range(0.1f, 5)] [SerializeField] float meleeRate;
    [Range(0, 10)] [SerializeField] int speed;
    [Range(0, 10)] [SerializeField] int meleeDestroyTime;
    [SerializeField] GameObject invisHit;
    [SerializeField] GameObject hitSpawnPos;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource enemyAud;
    public AudioClip soundExecute;
    [Range(0, 1)] [SerializeField] float soundExecuteVol;

    [Header("----- Effects -----")]
    [SerializeField] GameObject executeEffect;

    Vector3 playerDir;
    bool isMelee = false;
    bool isInSight = false;
    public bool inMeleeRange = false;
    private int HPOrig;

    float stoppingDistanceOrig;

    Vector3 startingPos;
    Vector3 raycastPos;


    // Start is called before the first frame update
    void Start()
    {
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        raycastPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (agent.isActiveAndEnabled && !anim.GetBool("Dead"))
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));
            playerDir = GameManager.instance.player.transform.position - raycastPos;

            if (isInSight)
            {
                CanSeePlayer();

            }
            if (!agent.pathPending && agent.remainingDistance == 0)
                Roam();
            //Debug.Log(agent.remainingDistance);
        }
    }

    void Roam()
    {
        agent.stoppingDistance = 0;
        agent.speed = speedRoam;
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, roamRadius, 1);
        if (hit.hit)
        {
            NavMeshPath path = new NavMeshPath();

            agent.SetDestination(hit.position);
            agent.CalculatePath(hit.position, path);
            agent.SetPath(path);

        }
    }

    void FacePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    public void TakeDamage(int damage)
    {

        if (anim.GetBool("Dead") == false)
        {
            HP -= damage;
            if (HP > 0)
            {
                anim.SetTrigger("Damage");
                StartCoroutine(FlashColor());
            }
            else if (HP <= 0)
            {
                //GameManager.instance.currentRoom.GetComponent<LevelSpawner>().EnemyKilled();
                anim.SetBool("Dead", true);
                agent.enabled = false;


                foreach (Collider col in GetComponents<Collider>())
                    col.enabled = false;
                ItemDrop();
            }
        }
    }

    private void ItemDrop()
    {
        //add code for health and amo drops
        //Instantiate(executeEffect, gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(healthDrop, gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(ammoDrop, gameObject.transform.position, gameObject.transform.rotation);
        //enemyAud.PlayOneShot(soundExecute, soundExecuteVol);
        //Destroy(gameObject);
    }

    IEnumerator FlashColor()
    {
        rend.material.color = Color.red;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        agent.speed = speedChase;
        agent.SetDestination(GameManager.instance.player.transform.position);
        agent.stoppingDistance = 0;
        rend.material.color = Color.white;
    }
    IEnumerator Melee()
    {
        isMelee = true;

        anim.SetTrigger("Melee");

        GameObject meleeClone = Instantiate(invisHit, hitSpawnPos.transform.position, invisHit.transform.rotation);
        meleeClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
        yield return new WaitForSeconds(meleeRate);

        isMelee = false;
    }

    void CanSeePlayer()
    {


        float angle = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), new Vector3(transform.forward.x, 0, transform.forward.z));
        //Debug.Log(angle);

        RaycastHit hit;
        if (Physics.Raycast(raycastPos, playerDir, out hit))
        {
            Debug.DrawRay(raycastPos, playerDir);

            if (hit.collider.CompareTag("Player") && angle <= fieldOfView)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;
                agent.speed = speedChase;
                FacePlayer();
                if (!isMelee && angle <= fieldOfViewMelee && Vector3.Distance(GameManager.instance.player.transform.position, agent.transform.position) <= agent.stoppingDistance)
                {
                    //inMeleeRange = true;
                    StartCoroutine(Melee());
                }
            }   
            
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isInSight = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInSight = false;
        Roam();
    }

}
