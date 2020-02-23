using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Status { idle, moving, crouching, sliding, wallRunning, grabbedLedge, climbingLedge }
    public Status status;
    [SerializeField]
    private LayerMask ledgeLayer;
    [SerializeField]
    private LayerMask wallrunLayer;
    Rigidbody rb;
    [HideInInspector]
    public bool grapple = false, isGrounded, walking, prevGrounded;
    [HideInInspector]
    public Vector3 yRotation,xRotation;
    CharacterController characterController;

    public float speed = 100.0f;
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
        if(xRotation.x == 0) return;
        var cam = Camera.main.transform;
        var currentX = Camera.main.transform.eulerAngles.x;
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

    void CheckForJump(){
        if (GameManager.Input.jump && isGrounded)
        {
            GameManager.CurrentWeapon.anim.SetTrigger("jump");
            moveDirection.y = jumpSpeed;
        }
    }

    void MovePlayer(){
        characterController.Move(moveDirection * Time.deltaTime);
    }
    void Update(){
        RotateCamera();
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f, 1 << LayerMask.NameToLayer("Ground"));

        SetDirection();

        CheckForJump();

        CheckForLanding();

        ApplyGravity();

        MovePlayer();
        
    }
}
