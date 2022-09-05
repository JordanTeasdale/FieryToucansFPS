using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [Header("------ Assignables -----")]
    //Assignables

    public Rigidbody rb;
    public GameObject explosion;
    [SerializeField] public LayerMask target;
    [SerializeField] public int targetLayerValue;

    [Header("------ Damage -----")]
    //Damage 

    public int damage;
    public float damageRange;

  

    [Header("------ Projectile Behaviors -----")]
    // Behaviors 

    [Range(0, 1f)] [SerializeField] float bounciness;
    [SerializeField] public bool usesGravity;
    public int speed;
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physicsMaterial;


    // Start is called before the first frame update
    void Start() {
        Setup();
        Destroy(gameObject, maxLifetime);
    }

    public void Update() {
        //When bullet explodes due to collisions or time
        maxLifetime -= Time.deltaTime;
        
        if (collisions > maxCollisions)
            Explode();
        if(maxLifetime == 0) {
            Explode();
        }


    }

    void Explode() {
        
        //Instantiate explosion 
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, damageRange, target);

        foreach (Collider enemy in enemiesHit) {
            if (enemy.TryGetComponent<IDamageable>(out IDamageable isDamageable)) {
                if (enemy.GetComponent<SphereCollider>())
                    isDamageable.TakeDamage(damage * 2);
                else
                    isDamageable.TakeDamage(damage);
            }
        }

        Invoke("Delay", 0.05f);

    }

    void Delay() {

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision _collision) {
       
        collisions++;

        if (_collision.gameObject.layer == targetLayerValue)
            Explode();

        if (explodeOnTouch)
            Explode();
    }

    private void Setup() {

        //Creating Physics material
        physicsMaterial = new PhysicMaterial();
        physicsMaterial.bounciness = bounciness;
        physicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicsMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        //Assign material to collider
        GetComponent<SphereCollider>().material = physicsMaterial;

        //Set Gravity
        rb.useGravity = usesGravity;
        
        
    }

    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
