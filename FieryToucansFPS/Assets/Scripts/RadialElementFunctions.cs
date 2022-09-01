using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialElementFunctions : MonoBehaviour
{
    public Color baseColor;
    public Color selectedColor;
    public void Select() {
        gameObject.GetComponent<Image>().color = selectedColor;
    }

    public void Deselect() {
        gameObject.GetComponent<Image>().color = baseColor;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
