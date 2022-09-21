using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{
    public float magnetSpeed;
    [SerializeField] GameObject item;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, Time.deltaTime * magnetSpeed);
        }

    }

    private void Update()
    {
        if (transform.childCount < 1)
        {

            Destroy(item);
        }
    }
}
