using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMusic : MonoBehaviour
{
    void Start()
    {
        if(FindObjectOfType<AudioManager>().percentOfListComplete == 1f) {
            FindObjectOfType<AudioManager>().Play("Fun With Guns");
        }
       
    }
}
