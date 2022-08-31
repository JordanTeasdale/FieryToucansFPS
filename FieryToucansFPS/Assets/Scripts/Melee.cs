using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            Debug.Log(other.name);
            if (other.TryGetComponent<IDamageable>(out IDamageable obj)) {
                obj.TakeDamage(damage);
            }
        }
            
    }
}
