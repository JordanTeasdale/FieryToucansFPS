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

    //additions
    public Animator animator;


    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate() {
        //get the input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHori;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;

        //if false its normal, if true the sens would be inverted
        if (invert)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        //Clamp the rotation
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

        //Rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player     by putting the child camera with the parent
        transform.parent.Rotate(Vector3.up * mouseX);
    }


    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = gameObject.transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Debug.Log("hello");
            gameObject.transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        gameObject.transform.position = originalPos;
    }



    void Update() {
        if (GameManager.instance.playerScript.HP <= 0)
            animator.SetBool("isDead", true);
        else if (GameManager.instance.playerScript.HP > 0)
            animator.SetBool("isDead", false);

    }
}
