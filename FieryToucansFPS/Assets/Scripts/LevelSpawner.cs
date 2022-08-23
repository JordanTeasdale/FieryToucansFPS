using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("----- GameObject Prefabs -----")]
    [SerializeField] GameObject door;
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject spider;
    [SerializeField] GameObject dragonBoss;

    [Header("----- Door Spawn Settings -----")]
    [SerializeField] int numOfDoors;
    [SerializeField] Vector2[] doorLocations;
    [SerializeField] bool[] doorFaceNorthSouth;

    [Header("----- Skeleton Spawn Settings -----")]
    [SerializeField] int numOfSkeletons;
    [SerializeField] Vector2[] skeletonLocations;

    [Header("----- Spider Spawn Settings -----")]
    [SerializeField] int numOfSpiders;
    [SerializeField] Vector2[] spiderLocations;

    [Header("----- DragonBoss Spawn Settings -----")]
    [SerializeField] int numOfDragonBosses;
    [SerializeField] Vector2[] dragonBossLocations;


    bool roomEntered = false;
    bool roomCleared = false;
    int enemiesAlive;

    private void OnTriggerEnter(Collider other) {
        if (!roomEntered) {
            roomEntered = true;
            for (int i = 0; i < numOfDoors; i++) {
                if (doorFaceNorthSouth[i])
                    Instantiate(door, new Vector3(transform.position.x + doorLocations[i].x, 1.5f, transform.position.z + doorLocations[i].y), door.transform.rotation);
                else
                    Instantiate(door, new Vector3(transform.position.x + doorLocations[i].x, 1.5f, transform.position.z + doorLocations[i].y), Quaternion.Euler(new Vector3(0, 90, 0)));
            }
            for (int i = 0; i < numOfSkeletons; i++) {
                Instantiate(skeleton, new Vector3(transform.position.x + skeletonLocations[i].x, 1.5f, transform.position.z + skeletonLocations[i].y), skeleton.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < numOfSpiders; i++) {
                Instantiate(spider, new Vector3(transform.position.x + spiderLocations[i].x, 1.5f, transform.position.z + spiderLocations[i].y), spider.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < numOfDragonBosses; i++) {
                Instantiate(dragonBoss, new Vector3(transform.position.x + dragonBossLocations[i].x, 1.5f, transform.position.z + dragonBossLocations[i].y), dragonBoss.transform.rotation);
                enemiesAlive++;
            }

        }
    }

}
