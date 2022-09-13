using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicatorResgister : MonoBehaviour
{
    [SerializeField] float destroyTimer = 10.0f;

    void Start()
    {
        Invoke("Register", Random.Range(1, 5));
    }
    void Register() { 
        Destroy(this.gameObject, destroyTimer);
    }
 
}
