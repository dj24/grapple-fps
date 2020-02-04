using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Animator anim;
    public bool firing, ads, crouch;
    Vector3 hipPos, adsPos;

    void Start()
    {
        hipPos = transform.localPosition;
        adsPos = new Vector3(0,-0.09f,0.8f);
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        anim.SetBool("firing", firing);
        if(ads){
            transform.localPosition = adsPos;
            GameManager.PlayerCamera.ads = true;
            return;
        }
        GameManager.PlayerCamera.ads = false;
        transform.localPosition = hipPos;
    }
}
