using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemyAI : MonoBehaviour
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
    public float sightRange, meleeRange, rangeAttackRange;

    [Header("-----Patrolling-----")]
    Vector3 walkPoint;
    public float walkPointRange;
    public int patrolSpeed;


    [Header("-----Attack Stats-----")]
    public float timeBetweenAttacks;
    public int damage;
    bool isAttacking, isShooting;
    public int chaseSpeed;
    [SerializeField] GameObject meleeAttackBox, rangeAttackBox;

    [Header("----- Weapons Stats -----")]
    [Range(0.1f, 5)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int shootDamage;
    [Range(1, 10)] [SerializeField] int RateOfFire;
    [Range(1, 5)] [SerializeField] int bulletDestroyTime;
    [SerializeField] GameObject bullet;
    

    [Header("-----States------")]

    public bool playerInSightRange, playerInMeleeAttackRange, playerInRangeAttackRange;

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
        //Check for sight and attack range
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (anim.GetBool("Dead") == false)
        {
            StartCoroutine(FOVRoutine());
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));


            if (!playerInSightRange && !playerInRangeAttackRange)
            {
                isAttacking = false;
                isShooting = false;
                Patrolling();
            }


            if (playerInSightRange && !playerInRangeAttackRange)
            {
                isAttacking = false;
                isShooting = false;
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
                        if (playerInSightRange && agent.remainingDistance <= rangeAttackRange && agent.remainingDistance > meleeRange)
                        {
                            playerInRangeAttackRange = true;
                            agent.stoppingDistance = rangeAttackRange;
                            StartCoroutine(RangeAttackPlayer());
                            
                        }
                        else
                        {
                            playerInRangeAttackRange = false;
                            
                        }
                        if(playerInSightRange && agent.remainingDistance <= meleeRange)
                        {
                            playerInMeleeAttackRange = true;
                            agent.stoppingDistance = 2.5f;
                            StartCoroutine(MeleeAttackPlayer());
                        }
                    }
                    else
                    {
                        playerInSightRange = false;
                        playerInRangeAttackRange = false;
                        playerInMeleeAttackRange = false;
                    }   //Patrolling();
                }
                else
                {
                    playerInSightRange = false;
                    playerInRangeAttackRange = false;
                    playerInMeleeAttackRange = false;
                }
            }
            else if (playerInSightRange)
            {
                playerInSightRange = false;
                playerInRangeAttackRange = false;
                playerInMeleeAttackRange = false;
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

    IEnumerator MeleeAttackPlayer()
    {
        //Make sure enemy doesn't move
        if (playerInMeleeAttackRange && agent.remainingDistance <= meleeRange)
        {
            isShooting = false;
            isAttacking = true;
            
            agent.SetDestination(transform.position);

            anim.SetTrigger("Melee");
            yield return new WaitForSeconds(timeBetweenAttacks);
            isAttacking = false;

        }
    }
    IEnumerator RangeAttackPlayer()
    {
        if (playerInRangeAttackRange && agent.remainingDistance > meleeRange && agent.remainingDistance <= rangeAttackRange)
        {
            isAttacking = false;
            isShooting = true;

            anim.SetTrigger("Shoot");

            GameObject bulletClone = Instantiate(bullet, rangeAttackBox.transform.position, gameObject.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = (GameManager.instance.player.transform.position - transform.position).normalized * RateOfFire;
            yield return new WaitForSeconds(shootRate);

            isShooting = false;
        }
    }
        private void HitBoxOn()
    {
        meleeAttackBox.GetComponent<Collider>().enabled = true;
    }

    private void HitBoxOff()
    {
        meleeAttackBox.GetComponent<Collider>().enabled = false;
    }
    private void ResetAttack()
    {
        isAttacking = false;
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
}
