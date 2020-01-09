using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float jumpHeight;
    float walkSpeed;
    float crouchSpeed;
    float sprintSpeed;
    float turnSpeed;
    float currentSpeed;
    Text velocityText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpHeight = 300f;
        walkSpeed = 5f;
        crouchSpeed = 2.5f;
        sprintSpeed = 10f;
        turnSpeed = 200f;
        velocityText = GameObject.FindWithTag("DebugNumber").GetComponent<Text>();
    }

    void rotateCamera()
    {
        float yRotation = Input.GetAxis("Mouse X");
        float xRotation = Input.GetAxis("Mouse Y");
        if (yRotation < 0 || yRotation > 0)
        {
            Vector3 rotation = new Vector3(0, yRotation);
            transform.Rotate(rotation * turnSpeed * Time.deltaTime);
        }
        if (xRotation < 0 || xRotation > 0)
        {
            Transform cam = Camera.main.transform;
            Vector3 rotation = new Vector3(-xRotation, 0);
            cam.Rotate(rotation * turnSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.tag);

        // example of playing sound when hitting ground hard
        //if (collision.relativeVelocity.magnitude > 2)
        //    audioSource.Play();
    }

    void FixedUpdate()
    {
        Vector3 xzVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        currentSpeed = xzVelocity.magnitude;
        velocityText.text = currentSpeed.ToString();

        rotateCamera();

        Vector3 runDirection = new Vector3(0, 0, 0);

        //TODO: abstract to class fields and set from input controller
        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool moving = forward || back || right || left;
        bool jumping = Input.GetKeyDown(KeyCode.Space);

        if (jumping) rb.AddForce(transform.up * jumpHeight);

        if (currentSpeed < walkSpeed)
        {
            if (forward) runDirection += transform.forward;
            if (right) runDirection += transform.right;
            if (left) runDirection -= transform.right;
            if (back) runDirection -= transform.forward;
        }

        if (moving)
        {
            rb.AddForce(runDirection * 10f);
        }
        else
        {
            Vector3 deccelDirection = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);
            rb.AddForce(deccelDirection * 10f);
        }
    }
}
