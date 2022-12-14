using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : EnemyBulletBase {
    [Header("------ Assignables -----")]
    //Assignables

    public Rigidbody rb;
    public GameObject explosion;
    [SerializeField] public LayerMask target;
    [SerializeField] public int targetLayerValue;
   

    [Header("------ Damage -----")]
    //Damage 

    [Range(0, 35)] public int damage;
    [Range(0, 35)] public float damageRange;

  

    [Header("------ Projectile Behaviors -----")]
    // Behaviors 

    [Range(0, 1f)] [SerializeField] float bounciness;
    [Range(0, 35)] [SerializeField] float knockbackForce;
    [Range(0, 5f)] [SerializeField] float knockbackForceInYDirection;
    [SerializeField] public bool usesGravity;
    public int speed;
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;
    [SerializeField] bool isOrdenance;
    

    int collisions;
    PhysicMaterial physicsMaterial;
    int weaponFiredFrom;
    bool isColliding;

    // Start is called before the first frame update
    void Start() {
        Setup();
        Destroy(gameObject, maxLifetime);
        weaponFiredFrom = GameManager.instance.playerScript.weaponIndex;
    }

    public void Update() {
        //When bullet explodes due to collisions or time
        maxLifetime -= Time.deltaTime;
        
        if (isOrdenance && maxLifetime <= 0) 
            Explode();
        else {

            

            if (maxLifetime <= 0)
                Explode();
        }

       

    }

    void Explode(Collision _collision = null) {
        
        //Instantiate explosion 
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, damageRange, target);
        if (enemiesHit.Length > 0 && target == GameManager.instance.player.layer)
            GameManager.instance.Create(shooter);
        foreach (Collider enemy in enemiesHit) {
            if (enemy.TryGetComponent<IDamageable>(out IDamageable isDamageable)) {
                if (enemy.GetComponent<SphereCollider>()) {
                    isDamageable.TakeDamage(damage * 2);
                } else {
                    isDamageable.TakeDamage(damage);
                }
                    
            }
            //Possible Knockback Implementation 
            /*
            if(enemy.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent) && _collision.gameObject.layer == targetLayerValue) {


                if (enemy.TryGetComponent<Rigidbody>(out Rigidbody torso)){
                    Vector3 direction = _collision.transform.position - transform.position;
                    direction.y = knockbackForceInYDirection;

                    torso.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse); 
                }

            } 
            */
        }

        Invoke("Delay", 0.05f);

    }

    void Delay() {

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision _collision) {

        if (!isColliding) {
            isColliding = true;
            collisions++;
            if (collisions > maxCollisions)
                Explode(_collision);

            if (_collision.gameObject.layer == targetLayerValue)
                Explode(_collision);

            if (explodeOnTouch)
                Explode(_collision); 
        }
    }

    private void OnCollisionExit(Collision collision) {
        isColliding = false;
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
