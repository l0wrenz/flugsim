using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParent : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidbody;
    private Vector3 direction = new Vector3(1f,1f,0);


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(direction * 2f, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
