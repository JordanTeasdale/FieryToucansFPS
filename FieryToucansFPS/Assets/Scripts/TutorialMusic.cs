using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMusic : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Fun With Guns");
    }
}
