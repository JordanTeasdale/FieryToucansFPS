using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFeedback : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if (gameObject.activeSelf)
            StartCoroutine(StartSaveFeedback());
    }

    public IEnumerator StartSaveFeedback() {

        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}
