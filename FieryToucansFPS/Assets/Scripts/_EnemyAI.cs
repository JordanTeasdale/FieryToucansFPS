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
    bool alreadyAttacked;
    public int chaseSpeed;
    [SerializeField] GameObject attackBox;


    [Header("-----States------")]

    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        attackBox.GetComponent<Collider>().enabled = false;
        walkPoint = transform.position;
        Patrolling();
        StartCoroutine(FOVRoutine());



    }

    private void Update()
    {
        //Check for sight and attack range
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (anim.GetBool("Dead") == false)
        {
            StartCoroutine(FOVRoutine());
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));


            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrolling();
            }


            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
        }
        //if (playerInSightRange && playerInAttackRange && agent.remainingDistance == attackRange)
        //{
        //    StartCoroutine(AttackPlayer());
        //}
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
                    }   //Patrolling();
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
        alreadyAttacked = true;
        anim.SetTrigger("Melee");
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
        HitBoxOff();
    }
    private void HitBoxOn()
    {
        attackBox.GetComponent<Collider>().enabled = true;
    }

    private void HitBoxOff()
    {
        attackBox.GetComponent<Collider>().enabled = false;
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
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
                anim.SetBool("Dead", true);
                agent.enabled = false;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);

        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {

    //    }
    //}


}