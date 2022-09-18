using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Music : MonoBehaviour
{
    bool isPlaying;
    
    void Update()
    {
        if (FindObjectOfType<AudioManager>().percentOfListComplete == 1f && isPlaying == false) {
            FindObjectOfType<AudioManager>().Play("In The Abyss");
            isPlaying = true;
        }

    }

}
