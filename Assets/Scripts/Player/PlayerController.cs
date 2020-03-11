using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Status { sliding, wallRunning, grabbedLedge, climbingLedge }
    public Status status;
    [SerializeField]
    private LayerMask ledgeLayer;
    [SerializeField]
    private LayerMask wallrunLayer;
    Rigidbody rb;
    // [HideInInspector]
    public bool grapple = false, isGrounded, walking, prevGrounded, sprinting, crouching;
    [HideInInspector]
    public Vector3 yRotation,xRotation;
    CharacterController characterController;

    public float speed;
    public float jumpSpeed = 50.0f;
    public float turnSpeed = 200f;
    public float gravity =  75.0f;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isGrounded = false;
    }

    void RotateCamera()
    {
        
        transform.Rotate(yRotation * turnSpeed * Time.deltaTime);
        GameManager.CurrentWeapon.anim.SetFloat("yRotate",xRotation.x, 1f, Time.deltaTime * 10f);
        GameManager.CurrentWeapon.anim.SetFloat("xRotate",yRotation.y, 1f, Time.deltaTime * 10f);
        if(xRotation.x == 0) return;
        var cam = GameObject.Find("Player Pivot").transform;
        var currentX = GameObject.Find("Player Pivot").transform.eulerAngles.x;
        // Allow for negative angles
        currentX = (currentX > 180) ? currentX - 360 : currentX;
        var newX = currentX + xRotation.x * turnSpeed * Time.deltaTime;
        var clampedX = Mathf.Clamp(newX, -90f, 90f);
        cam.eulerAngles = new Vector3(clampedX, cam.eulerAngles.y, cam.eulerAngles.z);
    }

    void FixedUpdate()
    {

        // switch (status)
        // {
        //     case Status.sliding:
        //         SlideMovement();
        //         break;
        //     case Status.grabbedLedge:
        //         GrabbedLedgeMovement();
        //         break;
        //     case Status.climbingLedge:
        //         ClimbLedgeMovement();
        //         break;
        //     case Status.wallRunning:
        //         WallrunningMovement();
        //         break;
        //     default:
        //         DefaultMovement();
        //         break;
        // }
    }


    void SetDirection(){
        speed = sprinting ? 20f : 10f;
        if(isGrounded){
            if( GameManager.Input.right != 0 || GameManager.Input.forward != 0){
                walking = true;
            }
            else{
                walking = false;
            }
            moveDirection = (transform.right * GameManager.Input.right + transform.forward * GameManager.Input.forward) * speed; 
        }
    }

    void ApplyGravity(){
        if(!isGrounded){
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    void CheckForLanding(){
         if(!prevGrounded && isGrounded){
            GameManager.CurrentWeapon.anim.SetTrigger("land");
        }
        prevGrounded = isGrounded;
    }

    void CheckForCrouch(){
        crouching = GameManager.Input.crouching && isGrounded;
    }

    void CheckForJump(){
        if (GameManager.Input.jump && isGrounded)
        {
            if(crouching){
                crouching = false;
                return;
            }
            GameManager.CurrentWeapon.anim.SetTrigger("jump");
            moveDirection.y = jumpSpeed;
        }
    }

    void MovePlayer(){
        characterController.Move(moveDirection * Time.deltaTime);
    }
    void Update(){
        RotateCamera();

        sprinting = GameManager.Input.sprinting;
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f, 1 << LayerMask.NameToLayer("Ground"));

        CheckForCrouch();

        if(crouching){
            Camera.main.transform.localPosition =  new Vector3(0,-0.5f,0);
        }
        else{
            Camera.main.transform.localPosition =  new Vector3(0,0.5f,0);
        }

        SetDirection();

        CheckForJump();

        CheckForLanding();

        ApplyGravity();

        MovePlayer();
        
    }
}
