using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float jumpHeight, walkSpeed, sprintSpeed, turnSpeed, velocity, gravity, drag;
    public bool forward, back, right, left, jumping, walking, sprinting, crouching, grounded = false, sliding = false, grapple = false;
    public Vector3 runDirection,yRotation,xRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        drag = rb.drag;
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
                GameManager.CurrentWeapon.anim.SetTrigger("land");
                GameManager.Audio.playHitGround();
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.contacts.Length > 0 && !grounded)
        {
            ContactPoint contact = collision.contacts[0];
            grounded = !(Vector3.Dot(contact.normal, Vector3.up) > 0.5);
        }  
    }

    void RotateCamera()
    {
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
            gravity = 20f;
        }
        else{
            gravity = 10f;
        }
    }
    
    public Vector3 FindVelRelativeToLook() {
        float lookAngle = transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float zMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector3(xMag,0,zMag);
    }

    void Accelerate()
    {
        //TODO: preserve velocity when jumping
        if(!grounded){
            rb.AddForce(runDirection * 10f);
            return;
        }
        float maxSpeed = sprinting ? sprintSpeed : walkSpeed;
        GameManager.Audio.playWalkSound();

        if(velocity < maxSpeed){
            rb.AddForce(runDirection * 100f);
        }
        
        // rb.MovePosition(transform.position + runDirection * maxSpeed * Time.fixedDeltaTime);
    }

    void CheckForJump()
    {
        if (jumping && grounded)
        {
            // TODO: add direction that player is moving on jump
            GameManager.CurrentWeapon.anim.SetTrigger("jump");
            // GameManager.Audio.playJump();
            rb.AddForce(transform.up * jumpHeight * 100f);
        }
    }

    void CheckForCrouch()
    {
        if(crouching && grounded)
        {
            GameManager.CurrentWeapon.crouch = true;
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            return;
        }
        GameManager.CurrentWeapon.crouch = false;
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

        if (sliding)
        {
            //we need to accelerate to counter act drag
            rb.drag = 0;
        }
        else{
            rb.drag = drag;
        }

        if (!CheckForSlide())
        {
            Accelerate();
        }

        CheckForJump();
    }

    void DisplayVelocity()
    {
        var text = GameManager.DebugText;
        if(text != null){
            text.text = velocity.ToString();
        }
        
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

        if((forward || right || left || back) && grounded){
            walking = true;
        }
        else{
            walking = false;
        }
    }
}
