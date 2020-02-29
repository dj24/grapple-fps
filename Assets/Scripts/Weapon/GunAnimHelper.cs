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

    IEnumerator ToggleLight()
    {
        GameObject light = flash.transform.Find("Light").gameObject;
        light.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        light.SetActive(false);
    }

    public void muzzleFlash(){
        if(flash != null) flash.Play();
        if(audio != null) audio.Play();
        if(GetComponent<Light>() != null) StartCoroutine(ToggleLight());
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
