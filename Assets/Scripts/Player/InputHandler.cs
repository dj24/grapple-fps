﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    PlayerController player;
    RopeController rope;
    WeaponController weapon;

    public float forward, right;
    public bool jump, sprinting, crouching, inspect;

    private void Start()
    {
        rope = GameManager.Rope;
        player = GameManager.Player;
        weapon = GameManager.CurrentWeapon;
    }

    void Update()
    {
        inspect = Input.GetKeyDown(KeyCode.Tab);
        if(inspect){
           weapon.inspect = !weapon.inspect;
        }
        if(weapon.inspect){
            forward = 0;
            right = 0;
            player.yRotation = Vector3.zero;
            player.xRotation =Vector3.zero;
            return;
        } 
        
        forward = Input.GetAxis("Vertical");
        right = Input.GetAxis("Horizontal");
        weapon.anim.SetFloat("xMove",right, 1f, Time.deltaTime * 50f);
        weapon.anim.SetFloat("zMove",forward, 1f, Time.deltaTime * 50f);
        // player.forward = Input.GetKey(KeyCode.W);
        // player.back = Input.GetKey(KeyCode.S);
        // player.right = Input.GetKey(KeyCode.D);
        // player.left = Input.GetKey(KeyCode.A);
        jump = Input.GetButtonDown("Jump");
        sprinting = Input.GetKey(KeyCode.LeftShift);
        crouching = Input.GetKey(KeyCode.C);
        player.yRotation = new Vector3(0, Input.GetAxis("Mouse X"));
        player.xRotation = new Vector3(-Input.GetAxis("Mouse Y"), 0);
        // player.grapple = Input.GetKeyDown(KeyCode.Q);
        rope.active = Input.GetKey(KeyCode.Q);
        weapon.firing = Input.GetMouseButton(0);
        weapon.ads = Input.GetMouseButton(1);
        
        if(Input.GetKeyDown(KeyCode.R)){
            weapon.StartReload();
        }
    }
}
