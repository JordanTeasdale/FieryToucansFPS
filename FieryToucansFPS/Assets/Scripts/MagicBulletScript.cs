using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBulletScript : MonoBehaviour {
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
    //public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;
    [SerializeField] bool isOrdenance = false;

    [Header("------ Magic -----")]
    private bool returning = false;
    MagicSMGSript parentScript;
    int myID;

    //int collisions;
    PhysicMaterial physicsMaterial;

    // Start is called before the first frame update
    void Start() {
        Setup();
        //Destroy(gameObject, maxLifetime);
    }

    public void Update() {
        //When bullet explodes due to collisions or time
        maxLifetime -= Time.deltaTime;

        if (maxLifetime <= 0)
            Explode();
        else
        {
            if (returning)           
                transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, speed * Time.deltaTime);           
        }
    }

    void Explode() {

        //parentScript.Remove(myID);

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
        parentScript.Remove(myID);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision _collision) {

        GameObject hit = _collision.gameObject;
        if (!hit.CompareTag("Player"))
        {
            if (hit.GetComponent<IDamageable>() != null)
                hit.GetComponent<IDamageable>().TakeDamage(damage);
            else            
                rb.velocity = Vector3.zero;         
        }
        else if (hit.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        #region oldstuff
        //collisions++;

        //if (_collision.gameObject.layer == targetLayerValue)
        //Explode();

        //if (explodeOnTouch)
        //Explode();
        #endregion
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

    public void Return() {
        returning = true;
    }

    public void Gimmy(MagicSMGSript skr, int id)
    {
        parentScript = skr;
        myID = id;
    }

    public void UpdateID()
    {
        myID--;
    }
}