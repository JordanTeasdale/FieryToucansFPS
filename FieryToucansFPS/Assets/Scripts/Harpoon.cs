using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] public float grabSpeed;
    [SerializeField] public float maxGrabDist;
    [SerializeField] public float xOffset;
    [SerializeField] public float zOffset;

    private GameObject player;
    private GameObject enemy;

    LineRenderer lineRend;

    bool isGrabbing = false;
    bool wasEnemyGrabbed = false;

    float grabDistance;
     Vector3 origPos;

    Rigidbody rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRend = GetComponent<LineRenderer>();
        wasEnemyGrabbed = false;
        grabDistance = 0;
        rb = GetComponent<Rigidbody>();
        origPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    private void Update()
    {
        origPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        lineRend.SetPosition(0, origPos);
        lineRend.SetPosition(1, transform.position);

        if (Input.GetMouseButtonDown(0) && isGrabbing == false && wasEnemyGrabbed == false)
        {
            StartGrab();
        }
        comeBack();
        ReturnEnemy();
    }

    private void StartGrab()
    {
        isGrabbing = true;
        GetComponent<Rigidbody>().isKinematic = false;
        rb.AddForce(transform.forward * grabSpeed);
    }

    private void comeBack()
    {
        if (isGrabbing)
        {
            grabDistance = Vector3.Distance(transform.position, origPos);
            if(grabDistance > maxGrabDist || wasEnemyGrabbed)
            {
                rb.isKinematic = true;
                transform.position = origPos;
                isGrabbing = false;
            }
        }
    }

    private void ReturnEnemy()
    {
        if(wasEnemyGrabbed == true)
        {
            Vector3 finalPos = new Vector3(origPos.x, enemy.transform.position.y, origPos.z + zOffset);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, finalPos, maxGrabDist);
            wasEnemyGrabbed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            wasEnemyGrabbed = true;
            enemy = other.gameObject;
        }
    }
}
