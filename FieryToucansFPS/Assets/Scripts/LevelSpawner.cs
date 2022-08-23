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
    [SerializeField] Vector2[] doorLocations;
    [SerializeField] bool[] doorFaceNorthSouth;
    GameObject[] doors;

    [Header("----- Skeleton Spawn Settings -----")]
    [SerializeField] Vector2[] skeletonLocations;

    [Header("----- Spider Spawn Settings -----")]
    [SerializeField] Vector2[] spiderLocations;

    [Header("----- DragonBoss Spawn Settings -----")]
    [SerializeField] Vector2[] dragonBossLocations;


    bool roomEntered = false;
    public bool roomCleared = false;
    public int enemiesAlive;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !roomEntered && !roomCleared) {
            roomEntered = true;
            GameManager.instance.currentRoom = gameObject;
            for (int i = 0; i < doorLocations.Length; i++) {
                if (doorFaceNorthSouth[i])
                    GameObject.Instantiate(door, new Vector3(transform.position.x + doorLocations[i].x, 1.5f, transform.position.z + doorLocations[i].y), door.transform.rotation);
                else
                    GameObject.Instantiate(door, new Vector3(transform.position.x + doorLocations[i].x, 1.5f, transform.position.z + doorLocations[i].y), Quaternion.Euler(new Vector3(0, 90, 0)));
            }
            for (int i = 0; i < skeletonLocations.Length; i++) {
                Instantiate(skeleton, new Vector3(transform.position.x + skeletonLocations[i].x, 1.5f, transform.position.z + skeletonLocations[i].y), skeleton.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < spiderLocations.Length; i++) {
                Instantiate(spider, new Vector3(transform.position.x + spiderLocations[i].x, 1.5f, transform.position.z + spiderLocations[i].y), spider.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < dragonBossLocations.Length; i++) {
                Instantiate(dragonBoss, new Vector3(transform.position.x + dragonBossLocations[i].x, 1.5f, transform.position.z + dragonBossLocations[i].y), dragonBoss.transform.rotation);
                enemiesAlive++;
            }

        }
    }

    public void EnemyKilled() {
        enemiesAlive--;
        if (enemiesAlive == 0) { 
            roomCleared = true;
            StartCoroutine(GameManager.instance.ClearRoom());
            doors = GameObject.FindGameObjectsWithTag("Door");
            foreach(GameObject door in doors) {
                Destroy(door);
            }
        }
    }

}
