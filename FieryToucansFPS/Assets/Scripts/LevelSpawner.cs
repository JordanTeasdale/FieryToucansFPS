using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    [Header("----- GameObject Prefabs -----")]
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject spider;
    [SerializeField] GameObject dragonWelp;
    [SerializeField] GameObject dragonBoss;

    [Header("----- Door Spawn Settings -----")]
    [SerializeField] GameObject[] doors;

    [Header("----- Skeleton Spawn Settings -----")]
    [SerializeField] Vector2[] skeletonLocations;

    [Header("----- Spider Spawn Settings -----")]
    [SerializeField] Vector2[] spiderLocations;

    [Header("----- DragonWelp Spawn Settings -----")]
    [SerializeField] Vector2[] dragonWelpLocations;

    [Header("----- DragonBoss Spawn Settings -----")]
    [SerializeField] Vector2[] dragonBossLocations;


    bool roomEntered = false;
    public bool roomCleared = false;
    public int enemiesAlive;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !roomEntered && !roomCleared) {
            roomEntered = true;
            GameManager.instance.currentRoom = gameObject;
            foreach (GameObject door in doors) {
                door.GetComponent<DoorScript>().isUnlocked = false;
                door.GetComponent<DoorScript>().DoorLocking();
            }
            for (int i = 0; i < skeletonLocations.Length; i++) {
                Instantiate(skeleton, new Vector3(transform.position.x + skeletonLocations[i].x, skeleton.transform.position.y, transform.position.z + skeletonLocations[i].y), skeleton.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < spiderLocations.Length; i++) {
                Instantiate(spider, new Vector3(transform.position.x + spiderLocations[i].x, spider.transform.position.y, transform.position.z + spiderLocations[i].y), spider.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < dragonWelpLocations.Length; i++) {
                Instantiate(dragonWelp, new Vector3(transform.position.x + dragonWelpLocations[i].x, dragonBoss.transform.position.y, transform.position.z + dragonWelpLocations[i].y), dragonWelp.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < dragonBossLocations.Length; i++) {
                Instantiate(dragonBoss, new Vector3(transform.position.x + dragonBossLocations[i].x, dragonBoss.transform.position.y, transform.position.z + dragonBossLocations[i].y), dragonBoss.transform.rotation);
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
            foreach (GameObject door in doors) {
                door.GetComponent<DoorScript>().isUnlocked = true;
                door.GetComponent<DoorScript>().DoorUnlocking();
            }
            StartCoroutine(RoomClearedFeedback());
        }
    }

    IEnumerator RoomClearedFeedback() {
        GameManager.instance.roomClearedFeedback.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.instance.roomClearedFeedback.SetActive(false);
    }

}
