using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] GameObject pickUp;

    public int ammo = 0;
    public int ammoIndex = -1;
    public int health;

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
        if(other.CompareTag("Player"))
        {
            Destroy(pickUp);
            GameManager.instance.playerScript.TakeDamage(-health);
            GameManager.instance.playerScript.AmmoPickup(ammoIndex, ammo);
            
        }
    }
}
