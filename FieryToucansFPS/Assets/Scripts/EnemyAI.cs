using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;
    [SerializeField] Animator anim;

    [Header("----- Enemy Stats -----")]
    [Range(0, 100)] public int HP;
    [Range(0, 10)] [SerializeField] int playerFaceSpeed;
    [Range(1, 180)] [SerializeField] int fieldOfView;
    [Range(1, 180)] [SerializeField] int fieldOfViewShoot;
    [Range(1, 180)] [SerializeField] int roamRadius;
    [Range(1, 5)] [SerializeField] float speedRoam;
    [Range(1, 5)] [SerializeField] float speedChase;

    [Header("----- Weapons Stats -----")]
    [Range(0.1f, 5)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int damage;
    [Range(1, 10)] [SerializeField] int speed;
    [Range(1, 5)] [SerializeField] int bulletDestroyTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawnPos;

    Vector3 playerDir;
    bool isShooting = false;
    bool playerInRange = false;

    float stoppingDistanceOrig;
    float speedOrig;
    Vector3 startingPos;



    // Start is called before the first frame update
    void Start() {
        stoppingDistanceOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        Roam();

    }

    // Update is called once per frame
    void Update() {
        if (agent.isActiveAndEnabled) {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));
            playerDir = GameManager.instance.player.transform.position - transform.position;

            if (playerInRange)
            {

                CanSeePlayer();
            }
            else if (agent.remainingDistance < 0.1f)
                Roam();
        }
    }
    void Roam() {
        agent.stoppingDistance = 0;
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, roamRadius, 1);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    void FacePlayer()  {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    public void TakeDamage(int damage) {


        HP -= damage;
        anim.SetTrigger("Damage");

        if (HP < 1) {
            anim.SetBool("Dead", true);
            agent.enabled = false;

            foreach (Collider col in GetComponents<Collider>())
                col.enabled = false;

        }
        else {
            StartCoroutine(FlashColor());

        }



    }

    IEnumerator FlashColor() {
        rend.material.color = Color.red;
        agent.enabled = false;
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
        agent.speed = speedOrig;
        agent.SetDestination(GameManager.instance.player.transform.position);
        agent.stoppingDistance = 0;
        rend.material.color = Color.white;
    }

    IEnumerator Shoot() {
        isShooting = true;

        anim.SetTrigger("Shoot");

        GameObject bulletClone = Instantiate(bullet, bulletSpawnPos.transform.position, bullet.transform.rotation);
        bulletClone.GetComponent<Bullet>().damage = damage;
        bulletClone.GetComponent<Bullet>().speed = speed;
        bulletClone.GetComponent<Bullet>().destroyTime = bulletDestroyTime;


        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void CanSeePlayer() {

        float angle = Vector3.Angle(playerDir, transform.forward);
        Debug.Log(angle);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))  {
            Debug.DrawRay(transform.position, playerDir);

            if (hit.collider.CompareTag("Player") && angle <= fieldOfView) {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;
                FacePlayer();

                if (!isShooting && angle <= fieldOfViewShoot) {
                    StartCoroutine(Shoot());
                }
            }

        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))  {
            playerInRange = true;

        }

    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            playerInRange = false;
        Roam();
    }

}
