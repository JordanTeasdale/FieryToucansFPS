using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Music : MonoBehaviour
{
    bool isPlaying = false;
    void Update()
    {
        if (FindObjectOfType<AudioManager>().percentOfListComplete == 1f && isPlaying == false) {
            Debug.Log("entered");
            FindObjectOfType<AudioManager>().Play("In The Abyss");
            isPlaying = true;
        }

    }

}
