using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    [Header("----- GameObject Prefabs -----")]
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject skeletonMissile;
    [SerializeField] GameObject spider;
    [SerializeField] GameObject goblin;
    [SerializeField] GameObject plagueDoctor;
    [SerializeField] GameObject dragonWelp;
    [SerializeField] GameObject dragonBoss;
    [SerializeField] GameObject terrorBringer;

    [Header("----- Door Spawn Settings -----")]
    [SerializeField] GameObject[] doors;

    [Header("----- Skeleton Spawn Settings -----")]
    [SerializeField] Vector2[] skeletonLocations;

    [Header("----- Skeleton Missile Spawn Settings -----")]
    [SerializeField] Vector2[] skeletonMissileLocations;

    [Header("----- Spider Spawn Settings -----")]
    [SerializeField] Vector2[] spiderLocations;
    
    [Header("----- Goblin Spawn Settings -----")]
    [SerializeField] Vector2[] GoblinLocations;

    [Header("----- Plague Doctor Spawn Settings -----")]
    [SerializeField] Vector2[] plagueDoctorLocations;

    [Header("----- DragonWelp Spawn Settings -----")]
    [SerializeField] Vector2[] dragonWelpLocations;

    [Header("----- DragonBoss Spawn Settings -----")]
    [SerializeField] Vector2[] dragonBossLocations;

    [Header("----- Terror Bringer Spawn Settings -----")]
    [SerializeField] Vector2[] terrorBringerLocations;


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
                Instantiate(skeleton, new Vector3(transform.position.x + skeletonLocations[i].x, transform.position.y, transform.position.z + skeletonLocations[i].y), skeleton.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < skeletonMissileLocations.Length; i++) {
                Instantiate(skeletonMissile, new Vector3(transform.position.x + skeletonMissileLocations[i].x, transform.position.y, transform.position.z + skeletonMissileLocations[i].y), skeletonMissile.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < spiderLocations.Length; i++) {
                Instantiate(spider, new Vector3(transform.position.x + spiderLocations[i].x, transform.position.y, transform.position.z + spiderLocations[i].y), spider.transform.rotation);
                enemiesAlive++;
            }
          //  for (int i = 0; i < GoblinLocations.Length; i++) {
            //    Instantiate(goblin, new Vector3(transform.position.x + GoblinLocations[i].x, transform.position.y, transform.position.z + GoblinLocations[i].y), goblin.transform.rotation);
              //  enemiesAlive++;
          //  }
            for (int i = 0; i < plagueDoctorLocations.Length; i++) {
                Instantiate(plagueDoctor, new Vector3(transform.position.x + plagueDoctorLocations[i].x, transform.position.y, transform.position.z + plagueDoctorLocations[i].y), plagueDoctor.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < dragonWelpLocations.Length; i++) {
                Instantiate(dragonWelp, new Vector3(transform.position.x + dragonWelpLocations[i].x, transform.position.y, transform.position.z + dragonWelpLocations[i].y), dragonWelp.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < dragonBossLocations.Length; i++) {
                Instantiate(dragonBoss, new Vector3(transform.position.x + dragonBossLocations[i].x, transform.position.y, transform.position.z + dragonBossLocations[i].y), dragonBoss.transform.rotation);
                enemiesAlive++;
            }
            for (int i = 0; i < terrorBringerLocations.Length; i++) {
                Instantiate(terrorBringer, new Vector3(transform.position.x + terrorBringerLocations[i].x, transform.position.y, transform.position.z + terrorBringerLocations[i].y), terrorBringer.transform.rotation);
                enemiesAlive++;
            }

        }
    }

    public void EnemyKilled() {
        enemiesAlive--;
        if (enemiesAlive == 0) {
            roomCleared = true;
            StartCoroutine(GameManager.instance.ClearRoom());
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
