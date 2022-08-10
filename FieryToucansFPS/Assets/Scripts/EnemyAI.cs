using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [Range(0, 10)] [SerializeField] int HP;
    [Range(0, 10)] [SerializeField] int playerFaceSpeed;

    [Header("----- Weapons Stats -----")]
    [Range(0.1f, 5)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int damage;
    [Range(1, 10)] [SerializeField] int speed;
    [Range(1, 10)] [SerializeField] int bulletDestroyTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawnPos;

    Vector3 playerDir;
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
        playerDir = GameManager.instance.player.transform.position - transform.position;

        facePlayer();

    }

    void facePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    public void TakeDamage(int _damage) {
        HP -= _damage;
        if (HP <= 0) {
            Destroy(gameObject);
        }
    }

   
}
