﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    PlayerController player;
    RopeController rope;
    WeaponController weapon;

    private void Awake()
    {
        rope = GameManager.Rope;
        player = GameManager.Player;
        weapon = GameManager.CurrentWeapon;
    }

    void Update()
    {
        player.forward = Input.GetKey(KeyCode.W);
        player.back = Input.GetKey(KeyCode.S);
        player.right = Input.GetKey(KeyCode.D);
        player.left = Input.GetKey(KeyCode.A);
        player.jumping = Input.GetKeyDown(KeyCode.Space);
        player.sprinting = Input.GetKey(KeyCode.LeftShift);
        player.crouching = Input.GetKey(KeyCode.C);
        player.yRotation = new Vector3(0, Input.GetAxis("Mouse X"));
        player.xRotation = new Vector3(-Input.GetAxis("Mouse Y"), 0);
        player.grapple = Input.GetKeyDown(KeyCode.Q);
        rope.active = Input.GetKey(KeyCode.Q);
        weapon.firing = Input.GetMouseButton(0);
        weapon.ads = Input.GetMouseButton(1);
    }
}
