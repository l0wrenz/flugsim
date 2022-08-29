using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    GameObject child;
    GameObject child2;

    Renderer renderer;
    Renderer renderer2;

    public Color color;

    void Start() {

    }

    public void SetColor(Color color_in) {
        child = transform.GetChild (0).gameObject;
        child2 = transform.GetChild (1).gameObject;

        renderer = child.GetComponent<Renderer>();
        renderer2 = child2.GetComponent<Renderer>();
        color = color_in;
        renderer.material.color = color;
        renderer2.material.color = color;


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
