using UnityEngine;

public class InputHandler : MonoBehaviour
{
    PlayerController player;
    RopeController rope;
    WeaponController weapon;

    public float forward, right;
    public bool jump, sprinting, crouching;

    private void Awake()
    {
        rope = GameManager.Rope;
        player = GameManager.Player;
        weapon = GameManager.CurrentWeapon;
    }

    void Update()
    {
        forward = Input.GetAxis("Vertical");
        right = Input.GetAxis("Horizontal");
        weapon.anim.SetFloat("xMove",right, 1f, Time.deltaTime * 10f);
        weapon.anim.SetFloat("zMove",forward, 1f, Time.deltaTime * 10f);
        // player.forward = Input.GetKey(KeyCode.W);
        // player.back = Input.GetKey(KeyCode.S);
        // player.right = Input.GetKey(KeyCode.D);
        // player.left = Input.GetKey(KeyCode.A);
        jump = Input.GetButtonDown("Jump");
        sprinting = Input.GetKey(KeyCode.LeftShift);
        crouching = Input.GetKey(KeyCode.C);
        player.yRotation = new Vector3(0, Input.GetAxis("Mouse X"));
        player.xRotation = new Vector3(-Input.GetAxis("Mouse Y"), 0);
        player.grapple = Input.GetKeyDown(KeyCode.Q);
        rope.active = Input.GetKey(KeyCode.Q);
        weapon.firing = Input.GetMouseButton(0);
        weapon.ads = Input.GetMouseButton(1);
        weapon.anim.SetBool("ads",Input.GetMouseButton(1));
        if(Input.GetKeyDown(KeyCode.R)){
            weapon.StartReload();
        }
    }
}
