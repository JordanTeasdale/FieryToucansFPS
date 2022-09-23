using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    [Range(0, 500)] public int HP;
    [Range(0, 10)][SerializeField] int playerFaceSpeed;
    [Range(1, 180)][SerializeField] int fieldOfView;
    [Range(1, 180)][SerializeField] int fieldOfViewMelee;
    [Range(1, 180)][SerializeField] int fieldOfViewShoot;
    [Range(1, 180)][SerializeField] int roamRadius;
    [Range(1, 20)][SerializeField] float speedRoam;
    [Range(1, 20)][SerializeField] float speedChase;


    [Header("----- Weapons Stats -----")]
    [Range(0.1f, 5)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int damage;
    [Range(1, 100)][SerializeField] int speed;
    [Range(1, 5)][SerializeField] int bulletDestroyTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawnPos;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource enemyAud;
    public AudioClip soundExecute;
    public AudioClip hurtSoundClip, deathSoundClip, gunSoundClip;
    public AudioClip[] attackSoundClips;
    [Range(0, 1)][SerializeField] float soundExecuteVol;

    [Header("----- Effects -----")]
    [SerializeField] GameObject executeEffect;

    Vector3 playerDir;
    bool isShooting = false;
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
            else if (agent.speed == 0 && !isShooting)
                anim.SetFloat("Speed", 0, agent.velocity.normalized.magnitude, Time.deltaTime * 5);

            playerDir = GameManager.instance.player.transform.position - raycastPos;

            if (playerInRange)
            {
                CanSeePlayer();
            }
            if (!agent.pathPending && agent.remainingDistance == 0 && !anim.GetBool("Dead"))
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
        Die();
        Destroy(gameObject);
        //GetComponent<Animator>().enabled = false;
        //GetComponent<EnemyAI>().enabled = false;
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

        foreach (Collider child in GetComponentsInChildren<Collider>())
            child.enabled = false;

        //GetComponent<Animator>().enabled = false;
    }

    IEnumerator FlashColor()
    {
        rend.material.color = Color.red;
        agent.speed = 0;
        yield return new WaitForSeconds(0.5f);
        agent.speed = speedChase;
        if(HP>0)
            agent.SetDestination(GameManager.instance.player.transform.position);
        agent.stoppingDistance = 0;
        rend.material.color = Color.white;
    }
    IEnumerator Shoot()
    {
        isShooting = true;
        agent.isStopped = true;

        anim.SetTrigger("Shoot");

        GameObject bulletClone = Instantiate(bullet, bulletSpawnPos.transform.position, gameObject.transform.rotation);
        Rigidbody rig = bulletClone.GetComponent<Rigidbody>();

        //bulletClone.GetComponent<SimpleEnemyProjectile>().damage = damage;
        //bulletClone.GetComponent<SimpleEnemyProjectile>().speed = speed;
        //bulletClone.GetComponent<SimpleEnemyProjectile>().destroyTime = bulletDestroyTime;
        // bulletClone.GetComponent<SimpleEnemyProjectile>().SetShooter(gameObject.transform);
        rig.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
        rig.AddForce(transform.forward * speed, ForceMode.VelocityChange);

        if (bulletClone.TryGetComponent<EnemyBulletBase>(out EnemyBulletBase bulletBase))
            bulletBase.SetShooter(gameObject.transform);
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
        if (!isShooting && HP > 0)
            agent.isStopped = false;
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

                if (!isShooting && angle <= fieldOfViewShoot && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(Shoot());
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
        enemyAud.PlayOneShot(gunSoundClip, soundExecuteVol);
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !anim.GetBool("Dead"))
        {
            playerInRange = true;
            PlayAttackSound();

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
