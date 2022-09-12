using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RifileScript : WeaponBase
{
    public Animator animator;
    //public GameObject mainCamera = GameManager.instance.playerScript.cameraMain.GetComponent<Camera>();
    private float normalFOV = 60f; //needs to be set to play fov pref once implemented
    public float scopedFOV = 20f;

    public void OnUnscoped()
    {
        GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = normalFOV;
    }
    public void OnScoped()
    {

        GameManager.instance.playerScript.cameraMain.GetComponent<Camera>().fieldOfView = scopedFOV;
    }


    public override void SecondaryFireMode()
    {
        base.SecondaryFireMode();
        OnScoped();
    }
}
