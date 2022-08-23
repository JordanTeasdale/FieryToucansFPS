using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] GameObject door;
    //[SerializeField] GameObject skeleton;
    //[SerializeField] GameObject spider;
    //[SerializeField] GameObject dragonBoss;
    [SerializeField] int numOfDoors;
    [SerializeField] Vector2[] doorLocations;
    [SerializeField] bool[] doorFaceNorthSouth;
    bool roomEntered = false;
    bool roomCleared = false;
    int enemiesAlive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (!roomEntered) {
            roomEntered = true;
            for (int i = 0; i < numOfDoors; i++) {
                if (doorFaceNorthSouth[i])
                    Instantiate(door, new Vector3(transform.position.x + doorLocations[i].x, 1.5f, transform.position.z + doorLocations[i].x), door.transform.rotation);
                else
                    Instantiate(door, new Vector3(transform.position.x + doorLocations[i].x, 1.5f, transform.position.z + doorLocations[i].x), Quaternion.Euler(new Vector3(0, 90, 0)));
            }

        }
    }

}
