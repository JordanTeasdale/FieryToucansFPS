using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Music : MonoBehaviour {
    bool isPlaying;
    
    void Update()
    {
        if (FindObjectOfType<AudioManager>().percentOfListComplete == 1f && isPlaying == false) {
            FindObjectOfType<AudioManager>().Play("Risen");
            isPlaying = true;
        }
            
    }

}
