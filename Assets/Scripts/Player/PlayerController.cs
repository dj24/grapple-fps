using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public enum Status { sliding, wallRunning, grabbedLedge, climbingLedge }
    public Status status;
    Rigidbody rb;
    //TODO: CHANGE THIS TO BE AN ENUM
    [SerializeField]
    bool isGrounded;
    public bool walking, sprinting;
    public Vector3 yRotation,xRotation;
    CharacterController characterController;

    public float speed;
    public float jumpSpeed;
    public float turnSpeed;
    public float gravity;
    private Vector3 moveDirection = Vector3.zero;
    #endregion
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isGrounded = false;
    }

    void RotateCamera()
    {
        var weapon = GetComponentInChildren<WeaponController>();
        transform.Rotate(yRotation * turnSpeed * Time.deltaTime);

        // Amount that gun is tilted by when aiming  
        float animFactor = 0.005f;

        weapon.anim.SetFloat("yRotate",xRotation.x * turnSpeed * animFactor, 1f, Time.deltaTime * 10f);
        weapon.anim.SetFloat("xRotate",yRotation.y * turnSpeed * animFactor, 1f, Time.deltaTime * 10f);

        if(xRotation.x == 0) return;

        var cam = GameObject.Find("Player Pivot").transform;
        var currentX = cam.eulerAngles.x;
        // Allow for negative angles
        currentX = (currentX > 180) ? currentX - 360 : currentX;
        var newX = currentX + xRotation.x * turnSpeed * Time.deltaTime;
        var clampedX = Mathf.Clamp(newX, -90f, 90f);
        cam.eulerAngles = new Vector3(clampedX, cam.eulerAngles.y, cam.eulerAngles.z);
    }


    void SetDirection(){
        speed = sprinting ? 10f : 5f;
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

    void GroundCheck(){
        RaycastHit hit;
        bool groundCheck = Physics.Raycast(transform.TransformPoint(characterController.center), Vector3.down, out hit,characterController.height/2, 1 << LayerMask.NameToLayer("Ground"));
        if(!isGrounded && groundCheck){
            GameManager.CurrentWeapon.anim.SetTrigger("land");
        }
        isGrounded = groundCheck;
    }

    void ApplyGravity(){
        if(!isGrounded){
            moveDirection -= new Vector3(0,gravity * Time.deltaTime,0);
        }
    }
    void MovePlayer(){
        characterController.Move(moveDirection * Time.deltaTime);
    }
    void CheckForJump(){
        if (GameManager.Input.jump && isGrounded)
        {
            GameManager.CurrentWeapon.anim.SetTrigger("jump");
            moveDirection.y = jumpSpeed;
        }
    }
    void Update(){
        RotateCamera();

        sprinting = GameManager.Input.sprinting;

        GroundCheck();

        SetDirection();

        CheckForJump();

        ApplyGravity();

        MovePlayer();

        Debug.DrawRay(transform.TransformPoint(characterController.center), Vector3.down , Color.red);

        GameManager.DebugText.text = moveDirection.ToString();
    }
}
