using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    Renderer m_renderer;
    public GameObject explosion;
    public Main mainScript;
    private Rigidbody rb;
    private Vector3 playerVelocity;
    public float playerSpeed = 5.0f;
    private LineRenderer _lineRenderer;

    private Vector3 next_direction = new Vector3(0f,1f,0);
    private Vector3 current_direction = new Vector3(1f,1f,0);

    private float angle = 0;
    private Vector3 up = new Vector3(0f,1f,0);
    private Vector3 down = new Vector3(0f,-1f,0);


    private bool allowed_to_turn = true;

    private void Start()
    {
        m_renderer = GetComponent<Renderer>();
        mainScript = GameObject.FindWithTag("main").GetComponent<Main>();
        rb = gameObject.GetComponent<Rigidbody>();
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.SetWidth(0.2f, 0.2f);
        _lineRenderer.enabled = false;

        Vector3 _objectPosition = gameObject.transform.position;
        Vector3 middle = new Vector3(0,0,0); 
        next_direction = middle - _objectPosition;

        current_direction = Vector3.Normalize(next_direction);
        angle = Vector3.Angle(up, current_direction);
        if (current_direction.x < 0) {
            angle = -angle;
        }

    }

    void rotateLeft() {
        Vector3 rotation = new Vector3(0, 0, 3f);
        transform.Rotate(rotation * Time.deltaTime * 10f, Space.Self);
    }
    void rotateRight() {
        Vector3 rotation = new Vector3(0, 0, -3f);
        transform.Rotate(rotation * Time.deltaTime * 10f, Space.Self);

    }

    void rotateDown() {
        rb.velocity = transform.forward * 2;
        Vector3 rotation = new Vector3(60f, 0, 0);
        transform.Rotate(rotation * Time.deltaTime * 10f, Space.Self);
    }


    void selfDestroy() {
        Destroy(gameObject);
    }

    void Update()
    {
        float current_angle = Vector3.Angle(up, transform.forward);
        if (transform.forward.x < 0) {
            current_angle = -current_angle;
        }



        rb.velocity = transform.forward * playerSpeed;

        float rotation_angle = 0;

        if (current_angle-angle > 180)
        {
            angle -= 180;
            rotation_angle = current_angle + angle;
            angle += 180;

        }
        // 
        else if (current_angle-angle < -180) {
            current_angle += 180;
            rotation_angle = current_angle + angle;
            current_angle -= 180;
        }
        else {
            rotation_angle = current_angle - angle;
        }

        if (rotation_angle > 20) {
            rotateLeft();
            Invoke("rotateRight", 1f);
        }
         if (rotation_angle < -20)
        {
            rotateRight();
            Invoke("rotateLeft", 1f);
        }
        Vector3 rotation = new Vector3(0, 0, rotation_angle);
        

        transform.Rotate(rotation * Time.deltaTime * 1f, Space.World);

        }


    void OnMouseDrag()
    {
        if (_lineRenderer.enabled == true) {
            Vector3 mouse = Input.mousePosition;
            mouse.z = 100;
            Vector3 CursorPosition = Camera.main.ScreenToWorldPoint(mouse);
            Vector3 _objectPosition = gameObject.transform.position;
            _lineRenderer.SetPosition(0, _objectPosition);
            _lineRenderer.SetPosition(1, CursorPosition);

            next_direction = CursorPosition - _objectPosition;
        }
     }
        
        

    void OnMouseDown(){
        _lineRenderer.enabled = true;
    }

    void OnMouseUp(){

        _lineRenderer.enabled = false;
        current_direction = Vector3.Normalize(next_direction);
        angle = Vector3.Angle(up, current_direction);
        if (current_direction.x < 0) {
            angle = -angle;
        }
        // angle = Mathf.Atan2(current_direction.x, current_direction.y)* Mathf.Rad2Deg;
        // if (current_direction.x < 0) {
        //     angle = -angle;
        // }
        // if (current_direction.y < 0) {
        //     angle = Vector3.Angle(down, current_direction);
        // }
        // Debug.Log(angle);

    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collisiuon");
        if (collision.gameObject.tag == "airport")
        {   
            Debug.Log("Airport Color: " + collision.gameObject.GetComponent<AirPort>().color);
            Debug.Log("Plane Color: " + gameObject.GetComponent<Plane>().color);


            if (collision.gameObject.GetComponent<AirPort>().color == gameObject.GetComponent<Plane>().color)
            {
                FindObjectOfType<Main>().IncreaseScore();
            } else {
                FindObjectOfType<Main>().WrongAirport();
            }
            rotateDown();
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "plane")
        {
            if (m_renderer.isVisible) {

                FindObjectOfType<Main>().Crash();
                rotateDown();
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);

        }
       
    }
    
}