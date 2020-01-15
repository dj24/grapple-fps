using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float jumpHeight, walkSpeed, crouchSpeed, sprintSpeed, turnSpeed, velocity;
    public bool forward, back, right, left, jumping, sprinting, crouching, grounded = false, sliding = false, grapple = false;
    public Vector3 runDirection,yRotation,xRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpHeight = 200f;
        walkSpeed = 7.5f;
        crouchSpeed = 2.5f;
        sprintSpeed = 15f;
        turnSpeed = 200f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == null)
        {
            return;
        }
        bool touchingFloor = collision.gameObject.tag == "Floor" || collision.gameObject.transform.parent.tag == "Floor";
        if (touchingFloor && !grounded)
        {
            grounded = true;
        }
    }

    void RotateCamera()
    {
        transform.Rotate(yRotation * turnSpeed * Time.deltaTime);
        Camera.main.transform.Rotate(xRotation * turnSpeed * Time.deltaTime);
    }

    void UpdateRunDirection()
    {
        //TODO: make it so that velocity isnt preserved when changin direction
        //maybe need to study game movement
        Vector3 dir = new Vector3(0, 0, 0);

        if (forward) dir += transform.forward;
        if (right) dir += transform.right;
        if (left) dir -= transform.right;
        if (back) dir -= transform.forward;

        runDirection = dir.normalized;
    }

    void UpdateVelocity()
    {
        velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
    }
    
    void Accelerate()
    {
        float maxSpeed = sprinting ? sprintSpeed : walkSpeed;

        if (velocity < maxSpeed)
        {
            rb.AddForce(runDirection * 100f);
        }
        
    }

    void Deccelerate()
    {
        float deccelSpeed = 10f;

        if (!grounded) deccelSpeed = 5f;

        float slideSpeed = 0.75f;

        if (sliding)
        {
            //we need to accelerate to counter act drag
            Vector3 accelDirection = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(accelDirection * slideSpeed);
            return;
        }

        if (!crouching)
        {
            Vector3 deccelDirection = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);
            rb.AddForce(deccelDirection * deccelSpeed);
            return;
        }

    }

    bool CheckForGrounded()
    {
        if (jumping && grounded)
        {
            rb.AddForce(transform.up * jumpHeight);
            return grounded = false;
        }
        return grounded;
    }

    void CheckForCrouch()
    {
        if(crouching && grounded)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            return;
        }
        Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    bool CheckForSlide()
    {
        if(crouching && sprinting && grounded && velocity > 0.1f)
        {
            sprinting = false;
            return sliding = true;
        }
        return sliding = false;
    }

    void MoveCharacter()
    {
        //TODO: use add force in mid air to allow grapple to moveqq
        UpdateRunDirection();

        //TODO: add crouching speed
        CheckForCrouch();

        if (CheckForGrounded())
        {
            Deccelerate();
        }

        if (!CheckForSlide())
        {
            Accelerate();
        }
    }

    void DisplayVelocity()
    {
        GameManager.DebugText.text = velocity.ToString();
    }

    void FixedUpdate()
    {
        UpdateVelocity();

        DisplayVelocity();

        RotateCamera();

        MoveCharacter();
    }
}
