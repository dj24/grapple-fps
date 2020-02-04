using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float jumpHeight, walkSpeed, sprintSpeed, turnSpeed, velocity, gravity;
    public bool forward, back, right, left, jumping, sprinting, crouching, grounded = false, sliding = false, grapple = false;
    public Vector3 runDirection,yRotation,xRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpHeight = 10f;
        walkSpeed = 15f;
        sprintSpeed = 30f;
        turnSpeed = 200f;
        gravity = 20f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.contacts.Length > 0 && !grounded)
        {
            ContactPoint contact = collision.contacts[0];
            grounded = Vector3.Dot(contact.normal, Vector3.up) > 0.5;
            if (grounded)
            {
                GameManager.Audio.playHitGround();
            }
        }
    }

    void RotateCamera()
    {
        //Weapon tilt anim
        Transform weaponTransform = GameManager.CurrentWeapon.gameObject.transform;
        float x = xRotation.x  * 5f;
        float y = yRotation.y * 5f;
        float z =  yRotation.y  * 5f + xRotation.x * 5f;
        Quaternion smoothedRotation = Quaternion.Slerp(weaponTransform.localRotation, Quaternion.Euler(x, y, z), 0.1f);
        weaponTransform.localRotation = smoothedRotation;

        transform.Rotate(yRotation * turnSpeed * Time.deltaTime);
        Camera.main.transform.Rotate(xRotation * turnSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        var gravityVector = new Vector3(0,-gravity,0);
        rb.AddForce(gravityVector, ForceMode.Acceleration);
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
        var yVelocity = new Vector3(0, rb.velocity.y, 0).magnitude;
        if(yVelocity > 10f){
            grounded = false;
        }
        if(rb.velocity.y < 0){
            gravity = 10f;
        }
        else{
            gravity = 5f;
        }
    }
    
    void Accelerate()
    {
        float maxSpeed = sprinting ? sprintSpeed : walkSpeed;
        GameManager.Audio.playWalkSound();
        if (velocity < maxSpeed)
        {
            rb.AddForce(runDirection * 300f);
        }
        
    }

    void CheckForJump()
    {
        if (jumping && grounded)
        {
            print("JUMP");
            GameManager.Audio.playJump();
            rb.AddForce(transform.up * jumpHeight * 100f);
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

        CheckForJump();

        if (sliding)
        {
            //we need to accelerate to counter act drag
            rb.drag = 0;
        }
        else{
            rb.drag = 1;
        }

        if (!CheckForSlide())
        {
            Accelerate();
        }
    }

    void DisplayVelocity()
    {
        GameManager.DebugText.text = velocity.ToString();
        ParticleSystem lines = GameManager.SpeedLines;
        var main = lines.main;
        float minSpeedToDisplayLines = 0.75f * sprintSpeed;
        if(velocity > minSpeedToDisplayLines){
            lines.Play();
            float alpha = (velocity - minSpeedToDisplayLines) / minSpeedToDisplayLines;
            main.startColor = new Color(1,1,1, alpha);
        }
        else{
            lines.Stop();
            main.startColor = new Color(1,1,1, 0);
        }
        
    }

    void FixedUpdate()
    {
        ApplyGravity();

        var rope = GameManager.Rope;
        float grappleForce = 2f;
        // if(grounded){
        //     grappleForce *= 10f;
        // }
        if(rope.active){
            rb.AddForce(-rope.direction * grappleForce);
        }

        UpdateVelocity();

        DisplayVelocity();

        RotateCamera();

        MoveCharacter();
    }
}
