using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject crossfade;
    public float transitionRate;
    private void Start() {
        Time.timeScale = 1;
    }

    private void FixedUpdate() {
        if (crossfade.GetComponent<CanvasGroup>().alpha > 0) {
            crossfade.GetComponent<CanvasGroup>().alpha -= transitionRate;
        }
    }

    public void CoRoutRun()
    {
        //StartCoroutine(StartTransition());
        FindObjectOfType<AudioManager>().Stop("Home Run");
        FindObjectOfType<AudioManager>().Stop("Fun With Guns");
        FindObjectOfType<AudioManager>().Stop("Risen");
        FindObjectOfType<AudioManager>().Stop("In The Abyss");
        FindObjectOfType<AudioManager>().PlayOneShot("Main Menu Transition");

    }

    /*public IEnumerator StartTransition()
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Start");
        //transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }*/
}
