using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour {
    
    public GameObject mainCamera;
    public float scopedFOV = 50f;
    private float normalFOV;
    int gunIndex = 2;

    public void OnUnscoped() {
        if (mainCamera.TryGetComponent<Camera>(out Camera _camera))
            _camera.fieldOfView = normalFOV;
    }
    //if clicked somethimes it double clicks and the animation doubles or it repeats itself without the intention to it
    //this is for wait time for this not to happen
    // IEnumerator OnScoped() { 
    //yield return new WaitForSeconds(0.5f);
    public void OnScoped() {
        if(mainCamera.TryGetComponent<Camera>(out Camera _camera))
        normalFOV = _camera.fieldOfView;
        _camera.fieldOfView = scopedFOV;
    }

}
