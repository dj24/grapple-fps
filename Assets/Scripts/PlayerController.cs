using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float jumpHeight, walkSpeed, crouchSpeed, sprintSpeed, turnSpeed, currentSpeed, accel, velocity;
    public bool forward, back, right, left, jumping, sprinting, crouching, grounded = false, sliding = false, grapple = false;
    public Vector3 runDirection,yRotation,xRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpHeight = 200f;
        walkSpeed = 5f;
        crouchSpeed = 2.5f;
        sprintSpeed = 10f;
        turnSpeed = 200f;
        accel = 10f;
    }

    void OnCollisionEnter(Collision collision)
    {
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

    void GetCurrentSpeed()
    {
        float maxSpeed = sprinting ? sprintSpeed : walkSpeed;

        if (currentSpeed + accel * Time.deltaTime < maxSpeed)
        {
            currentSpeed += accel * Time.deltaTime;
            return;
        }
        currentSpeed -= accel * Time.deltaTime;
    }

    void Accelerate()
    {
        GetCurrentSpeed();

        rb.velocity = new Vector3(runDirection.x * currentSpeed, rb.velocity.y, runDirection.z * currentSpeed);
    }

    void Deccelerate()
    {
        currentSpeed = velocity;

        float deccelSpeed = 5f;
        float slideSpeed = 0.5f;

        if (!grounded)
        {
            deccelSpeed = 1f;
            return;
        }
        if (!crouching)
        {
            Vector3 deccelDirection = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);
            rb.AddForce(deccelDirection * deccelSpeed);
            return;
        }
        if (sliding)
        {
            //we need to accelerate to counter act drag
            Vector3 accelDirection = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(accelDirection * slideSpeed);
            return;
        }
    }

    void CheckForJump()
    {
        if (jumping && grounded)
        {
            grounded = false;
            rb.AddForce(transform.up * jumpHeight);
        }
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

    void CheckForSlide()
    {
        if(crouching && sprinting && grounded)
        {
            sliding = true;
            sprinting = false;
            return;
        }

        if(velocity < 0.1f || !crouching)
        {
            sliding = false;
            return;
        }
    }

    void MoveCharacter()
    {
        UpdateRunDirection();

        CheckForCrouch();

        CheckForSlide();

        CheckForJump();

        bool moving = forward || back || right || left;

        if (moving && !sliding)
        {
            Accelerate();
            return;
        }

        Deccelerate();
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
