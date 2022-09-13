using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _EnemyAI : MonoBehaviour, IDamageable
{
    [Header("-----Components-----")]
    public NavMeshAgent agent;
    public Animator anim;
    public Renderer rend;
    public int HP;
    public GameObject player;
    public GameObject[] itemDrop;

    [Header("-----Field of View-----")]
    public LayerMask playerMask, obstuctionMask;
    [Range(1, 360)] public float angle;
    public float sightRange, attackRange;

    [Header("-----Patrolling-----")]
    Vector3 walkPoint;
    public float walkPointRange;
    public int patrolSpeed;
    

    [Header("-----Attack Stats-----")]
    public float timeBetweenAttacks;
    public int damage;
    bool isAttacking;
    public int chaseSpeed;
    [SerializeField] GameObject attackBox;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource enemyAud;
    public AudioClip hurtSoundClip, deathSoundClip;
    [Range(0, 1)] [SerializeField] float soundVol;
    public AudioClip[] attackSoundClips;
    [Range(0, 1)] [SerializeField] float soundVol2;

    [Header("-----States------")]

    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        walkPoint = transform.position;
        Patrolling();
        StartCoroutine(FOVRoutine());



    }

    private void Update()
    {
        
        if (anim.GetBool("Dead") == false)
        {
            StartCoroutine(FOVRoutine());
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));


            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrolling();
            }


            else if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
        }
        
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        if (anim.GetBool("Dead") == false)
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sightRange, playerMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 playerDir = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, playerDir) < angle / 2)
                {
                    float playerDist = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, playerDir, playerDist, obstuctionMask))
                    {
                        playerInSightRange = true;
                        if (playerInSightRange && agent.remainingDistance <= attackRange)
                        {
                            playerInAttackRange = true;
                            agent.stoppingDistance = attackRange;
                            attackBox.GetComponent<Collider>().enabled = true;
                            StartCoroutine(AttackPlayer());
                        }
                        else
                        {
                            playerInAttackRange = false;
                            //Patrolling();
                        }
                    }
                    else
                    {
                        playerInSightRange = false;
                        playerInAttackRange = false;
                    }   
                }
                else
                {
                    playerInSightRange = false;
                    playerInAttackRange = false;
                }
            }
            else if (playerInSightRange)
            {
                playerInSightRange = false;
                playerInAttackRange = false;
            }
        }
    }
    private void Patrolling()
    {

        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0;
        Vector3 randomDir = Random.insideUnitSphere * walkPointRange;
        randomDir += walkPoint;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, walkPointRange, NavMesh.AllAreas);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(hit.position, path);
        if (agent.remainingDistance < 0.1f)
            agent.SetPath(path);
    }


    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    IEnumerator AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        isAttacking = true;
        anim.SetTrigger("Melee");
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
        
    }
    private void HitBoxOn()
    {
        attackBox.GetComponent<Collider>().enabled = true;
    }

    private void HitBoxOff()
    {
        attackBox.GetComponent<Collider>().enabled = false;
    }

    public void TakeDamage(int dmg)
    {

        if (anim.GetBool("Dead") == false)
        {
            HP -= dmg;

            if (HP > 0)
            {
                anim.SetTrigger("Damage");
                StartCoroutine(flashColor());
            }
            else
            {
                GameManager.instance.currentRoom.GetComponent<LevelSpawner>().EnemyKilled();
                anim.SetBool("Dead", true);
                agent.enabled = false;
                Instantiate(itemDrop[Random.Range(0, itemDrop.Length)], gameObject.transform.position, gameObject.transform.rotation);

                foreach (Collider col in GetComponents<Collider>())
                    col.enabled = false;
            }

        }


    }
    IEnumerator flashColor()
    {

        rend.material.color = Color.red;
        agent.speed = 0;
        yield return new WaitForSeconds(0.5f);
        ChasePlayer();
        rend.material.color = Color.white;
    }

    private void PlayHurtSound()
    {
        enemyAud.PlayOneShot(hurtSoundClip, soundVol);
    }

    private void PlayAttackSound()
    {
        enemyAud.PlayOneShot(attackSoundClips[Random.Range(0, attackSoundClips.Length)], soundVol2);
    }
    private void PlayDeathSound()
    {
        enemyAud.PlayOneShot(deathSoundClip, soundVol);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);

        }
    }

    

}
