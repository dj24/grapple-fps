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
        GameManager.CurrentWeapon.Fire();
        GameManager.CurrentWeapon.bulletsFired++;
    }

    public void step(){
        GameManager.Audio.playStep();
    }

    public void reloadDone(){
        GameManager.CurrentWeapon.FinishReload();
    }

    public void magOut(){
         GameManager.CurrentWeapon.playMagOut();
    }

    public void magIn(){
         GameManager.CurrentWeapon.playMagIn();
    }
}
