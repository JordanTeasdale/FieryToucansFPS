using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingMissile : EnemyBulletBase {
    [Header("Setup")]
    public Rigidbody RocketBody;
    public GameObject explosion;
    [SerializeField] public int targetLayerValue;
    [SerializeField] public LayerMask target;



    [Header("------ Damage -----")]
    public int damage;
    public float damageRange;

    public float turnSpeed = 1f;
    public float rocketFlySpeed = 10f;
    int collisions;

    [Header("------ Projectile Behaviors -----")]
    public bool explodeOnTouch = true;
    bool isColliding = false;
    public float maxLifetime;



    private Transform rocketLocalTrans;

    //start is called before the the first frame update
    void Start() {
        rocketLocalTrans = GetComponent<Transform>();


        Destroy(gameObject, maxLifetime);
    }

    private void FixedUpdate() {
        if (!RocketBody) // if we do not have a rigid body set, do nothing
            return;

        RocketBody.velocity = rocketLocalTrans.forward * rocketFlySpeed;

        //turning the rocket towards the target (player)
        var rocketTargetRot = Quaternion.LookRotation(GameManager.instance.player.transform.position - rocketLocalTrans.position);
        RocketBody.MoveRotation(Quaternion.RotateTowards(rocketLocalTrans.rotation, rocketTargetRot, turnSpeed));
    }

    private void OnCollisionEnter(Collision _collision) {
        if (!isColliding) {
            isColliding = true;
            collisions++;

            if (_collision.gameObject.layer == targetLayerValue)
                Explode();
            else if (explodeOnTouch)
                Explode();
        }
    }

    private void OnCollisionExit(Collision collision) {
        isColliding = false;
    }

    void Delay() {
        Destroy(gameObject);
    }
    void Explode() {

        //Instantiate explosion 
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, damageRange, target);
        if (enemiesHit.Length > 0)
            GameManager.instance.Create(shooter);
        foreach (Collider enemy in enemiesHit) {
            if (enemy.TryGetComponent<IDamageable>(out IDamageable isDamageable)) {
                if (enemy.GetComponent<SphereCollider>())
                    isDamageable.TakeDamage(damage * 2);
                else
                    isDamageable.TakeDamage(damage);
            }
        }
        Delay();

    }
}


