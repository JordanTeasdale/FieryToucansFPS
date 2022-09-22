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
    [SerializeField] Collider sightCol;


    [Header("----- Enemy Stats -----")]
    [Range(0, 500)] public int HP;
    [Range(0, 10)] [SerializeField] int playerFaceSpeed;
    [Range(1, 180)] [SerializeField] int fieldOfView;
    [Range(1, 180)] [SerializeField] int fieldOfViewMelee;
    [Range(1, 180)] [SerializeField] int fieldOfViewShoot;
    [Range(1, 180)] [SerializeField] int roamRadius;
    [Range(1, 20)] [SerializeField] float speedRoam;
    [Range(1, 20)] [SerializeField] float speedChase;


    [Header("----- Weapons Stats -----")]
    [Range(0.1f, 5)] [SerializeField] float meleeRate;
    [Range(1, 10)] [SerializeField] int damage;
    [Range(1, 10)] [SerializeField] int RateOfmelee;
    [SerializeField] public GameObject[] attackBoxes;


    [Header("----- Audio -----")]
    [SerializeField] AudioSource enemyAud;
    public AudioClip soundExecute;
    public AudioClip hurtSoundClip, deathSoundClip;
    public AudioClip[] attackSoundClips; 
    public AudioClip[]gunSoundClips;
    [Range(0, 1)] [SerializeField] float soundExecuteVol;

    [Header("----- Effects -----")]
    [SerializeField] GameObject executeEffect;

    Vector3 playerDir;
    bool isAttacking = false;
    bool playerInRange = false;
    public bool inMeleeRange = false;
    public bool isExecutable = false;
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

        anim.SetTrigger("Roar");
       
        
        
    }

    // Update is called once per frame
    void Update()
    {
        raycastPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (agent.isActiveAndEnabled && !anim.GetBool("Dead"))
        {
            if (agent.speed > 0 && agent.speed < speedChase)
                anim.SetFloat("Speed", speedRoam, agent.velocity.normalized.magnitude, Time.deltaTime * 5);
            else if (agent.speed > speedRoam)
                anim.SetFloat("Speed", speedChase, agent.velocity.normalized.magnitude, Time.deltaTime * 5);
            else if (agent.speed == 0 && !isAttacking)
                anim.SetFloat("Speed", 0, agent.velocity.normalized.magnitude, Time.deltaTime * 5);

            playerDir = GameManager.instance.player.transform.position - raycastPos;

            if (playerInRange)
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
        //SightOff();
        //SightOn();
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

        if (!anim.GetBool("Dead") && agent.isActiveAndEnabled)
        {
            HP -= damage;

            if (HP > 0)
            {
                anim.SetTrigger("Damage");
                StartCoroutine(FlashColor());

                StartCoroutine(Executable());
                if (isExecutable && GameManager.instance.playerScript.isMeleeing == true)
                {

                    Execute();
                }
            }
            else if (HP <= 0)
            {
                Die();

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
        GetComponent<Animator>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;



    }

    IEnumerator Executable()
    {
        if (HP <= HPOrig / 4 && HP > 0)
        {
            isExecutable = true;
            rend.material.color = Color.yellow;
            yield return new WaitForSeconds(1);
            rend.material.color = Color.cyan;
            yield return new WaitForSeconds(1);
        }
        isExecutable = false;
        rend.material.color = Color.white;
    }
    public void Die()
    {


        GameManager.instance.currentRoom.GetComponent<LevelSpawner>().EnemyKilled();
        anim.SetBool(("Dead"), true);
        agent.enabled = false;
        GetComponent<EnemyAI>().enabled = false;

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;


        //GetComponent<Animator>().enabled = false;
    }

    IEnumerator FlashColor()
    {
        rend.material.color = Color.red;
        agent.speed = 0;
        yield return new WaitForSeconds(0.5f);
        agent.speed = speedChase;
        agent.SetDestination(GameManager.instance.player.transform.position);
        rend.material.color = Color.white;
    }
    IEnumerator Melee()
    {
        int choice = 0;
        choice = Random.Range(1, 100);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            
            isAttacking = true;
            agent.isStopped = true;

            if (choice <= 50)
                anim.SetTrigger("Melee");
            if (choice >= 50)
                anim.SetTrigger("Bite");
            yield return new WaitForSeconds(meleeRate);
        }
        isAttacking = false;
        agent.isStopped = false;
        SightOff();
        SightOn();

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

                if (!isAttacking && angle <= fieldOfViewMelee && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(Melee());
                }

            }
        }
    }


    private void PlayHurtSound()
    {
        enemyAud.PlayOneShot(hurtSoundClip, soundExecuteVol);
    }

    private void PlayAttackSound()
    {
        enemyAud.PlayOneShot(attackSoundClips[Random.Range(0, attackSoundClips.Length)], soundExecuteVol);
    }
    private void PlayDeathSound()
    {
        enemyAud.PlayOneShot(deathSoundClip, soundExecuteVol);
    }
    private void PlayWeaponSound()
    {
        enemyAud.PlayOneShot(gunSoundClips[0], soundExecuteVol);
       
    }
    private void PlayWeaponSound2()
    {
        enemyAud.PlayOneShot(gunSoundClips[1], soundExecuteVol);

    }
    private void WingsHitBoxOn()
    {
        attackBoxes[1].GetComponent<Collider>().enabled = true;
       
    }

    private void WingsHitBoxOff()
    {
        attackBoxes[1].GetComponent<Collider>().enabled = false;
        
    }

    private void BiteHitBoxOn()
    {
        attackBoxes[0].GetComponent<Collider>().enabled = true;
    }
    private void BiteHitBoxOff()
    {
        attackBoxes[0].GetComponent<Collider>().enabled = false;
    }
    private void SightOn()
    {
        sightCol.enabled = true;
        
    }
    private void SightOff()
    {
        sightCol.enabled = false;
    }



    void OnTriggerEnter(Collider other)
    {

         if(other.CompareTag("Player") && !anim.GetBool("Dead"))
        {
            
            playerInRange = true;
            //PlayAttackSound();

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;

        if (agent.isActiveAndEnabled)
            Roam();
    }

}
