using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invert;

    float xRotation;
    bool isShaking;
    float mouseX;
    float mouseY;

    //additions
    public Animator animator;
    public Animation animationClip;


    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate() {

        //get the input
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHori;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        if (!isShaking) {
            //if false its normal, if true the sens would be inverted
            if (invert)
                xRotation += mouseY;
            else
                xRotation -= mouseY;

            //Clamp the rotation
            xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

            //Rotate the camera on the x-axis
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
        //rotate the player     by putting the child camera with the parent
        transform.parent.Rotate(Vector3.up * mouseX);
    }


    public IEnumerator Shake(float duration, float magnitude) {

        float elapsed = 0.0f;
        isShaking = true;
        while (elapsed < duration) {
            float z = Random.Range(-2f, 2f) * magnitude;
            float x = Random.Range(-1f, 1f) * magnitude;
            Debug.Log("hello");
            transform.localRotation = Quaternion.Euler((Vector3.forward * z) + (new Vector3(xRotation + x, 0, 0)));

            elapsed += Time.deltaTime;

            yield return null;
        }
        isShaking = false;
    }



    void Update() {

        if (GameManager.instance.playerScript.HP <= 0)
            animator.SetBool("isDead", true);
        else if (GameManager.instance.playerScript.HP > 0)
            animator.SetBool("isDead", false);


    }
}
