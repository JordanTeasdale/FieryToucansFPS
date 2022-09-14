using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Music : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        FindObjectOfType<AudioManager>().Play("Risen");
    }

}
