using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunAnimHelper : MonoBehaviour
{
    VisualEffect flash;
    AudioSource audio;
    
    void Start(){
        flash = GetComponentInChildren<VisualEffect>();
        audio = GetComponent<AudioSource>();
    }
    public void muzzleFlash(){
        flash.Play();
        audio.Play();
        GameManager.CurrentWeapon.bulletsFired++;
        print(GameManager.CurrentWeapon.bulletsFired);
    }

    public void step(){
        GameManager.Audio.playStep();
    }
}
