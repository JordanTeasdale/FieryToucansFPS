using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFunctions : MonoBehaviour
{
    [SerializeField] bool checkEnemy1Death;
    [SerializeField] bool checkEnemy2Death;
    [SerializeField] bool checkEnemy3Death;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenUponDeath();
    }

    void OpenUponDeath() {
        if (GameManager.instance.enemy1Script.HP <= 0 && checkEnemy1Death) {
            Destroy(gameObject);
        }
        else if (GameManager.instance.enemy2Script.HP <= 0 && checkEnemy2Death) {
            Destroy(gameObject);
        }
        else if (GameManager.instance.enemy3Script.HP <= 0 && checkEnemy3Death) {
            Destroy(gameObject);
        }
    }
}
