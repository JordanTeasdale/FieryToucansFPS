using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuScript : MonoBehaviour
{
    Vector2 normalisecMousePosition;
    float currentAngle;
    public int selection;
    int previousSelection;

    public GameObject[] elements;
    public RadialElementFunctions currentElement;
    public RadialElementFunctions previousElement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        previousSelection = selection;
        normalisecMousePosition = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
        currentAngle = Mathf.Atan2(normalisecMousePosition.x, normalisecMousePosition.y)*Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;
        selection = (int)currentAngle / 60;
        currentElement = elements[selection].GetComponent<RadialElementFunctions>();
        currentElement.Select();
        if (previousSelection != selection) {
            previousElement = elements[previousSelection].GetComponent<RadialElementFunctions>();
            previousElement.Deselect();
        }
        //Debug.Log(selection);
    }
}
