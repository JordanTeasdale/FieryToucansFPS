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
    [SerializeField] public LayerMask isKnockbackable;

    [Header("------ Damage -----")]
    //Damage 

    [Range(0, 35)] public int damage;
    [Range(0, 35)] public float damageRange;

  

    [Header("------ Projectile Behaviors -----")]
    // Behaviors 

    [Range(0, 1f)] [SerializeField] float bounciness;
    [Range(0, 35)] [SerializeField] float knockbackForce;
    [SerializeField] public bool usesGravity;
    public int speed;
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;
    [SerializeField] bool isOrdenance = false;
    

    int collisions;
    PhysicMaterial physicsMaterial;
    int weaponFiredFrom;

    // Start is called before the first frame update
    void Start() {
        Setup();
        Destroy(gameObject, maxLifetime);
    }

    public void Update() {
        //When bullet explodes due to collisions or time
        maxLifetime -= Time.deltaTime;
        weaponFiredFrom = GameManager.instance.playerScript.weaponIndex;

        if (isOrdenance && maxLifetime <= 0) 
            Explode();
        else {

            if (collisions > maxCollisions)
                Explode();

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

        foreach (Collider enemy in enemiesHit) {
            if (enemy.TryGetComponent<IDamageable>(out IDamageable isDamageable)) {
                if (enemy.GetComponent<SphereCollider>()) {
                    isDamageable.TakeDamage(damage * 2);
                    WeaponBase weaponGainingExperience = GameManager.instance.playerScript.gunsList[weaponFiredFrom];
                    weaponGainingExperience.GainExperience(damage * 2);

                } else {
                    isDamageable.TakeDamage(damage);
                    WeaponBase weaponGainingExperience = GameManager.instance.playerScript.gunsList[weaponFiredFrom];
                    weaponGainingExperience.GainExperience(damage * 2);
                }
                    
            }
            if (enemy.TryGetComponent<KnockbackScript>(out KnockbackScript knockback)){
                StartCoroutine(knockback.Knockback(knockbackForce, enemy.gameObject, _collision));
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
            Explode(_collision);

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
