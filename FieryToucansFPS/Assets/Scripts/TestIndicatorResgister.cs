using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicatorResgister : MonoBehaviour
{
    [SerializeField] float destroyTimer = 20.0f;

    void Start()
    {
        Invoke("Register", Random.Range(0, 5));
    }
    void Register()
    {
        if (DI_system.CheckIfObjectInSight(this.transform)) {
            DI_system.CreateIndicator(this.transform);
        }
        Destroy(this.gameObject, destroyTimer);
    }
 
}
