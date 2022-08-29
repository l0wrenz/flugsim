using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPort : MonoBehaviour
{
    GameObject child;
    Renderer renderer;
    public Color color = Color.white;
    
    void Start() {
    }

    public void SetColor(Color color_in) {
        child = transform.GetChild(0).gameObject;
        renderer = child.GetComponent<Renderer>();
        color = color_in;
        renderer.material.color = color;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
