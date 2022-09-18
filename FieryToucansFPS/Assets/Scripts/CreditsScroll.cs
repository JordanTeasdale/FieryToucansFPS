using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour {
    [SerializeField] float timer;
    LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start() {
        gameObject.transform.localPosition = Vector3.zero;
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        timer -= Time.deltaTime;
        gameObject.transform.position += new Vector3(0, 5, 0);
        if (timer <= 0) {
            levelLoader.CoRoutRun();
            SceneManager.LoadScene("Main Menu");
        }

    }
}
