using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Animator anim;
    public bool firing, ads, crouch;
    Vector3 hipPos, adsPos;
    public int bulletsFired;
    ParticleSystem smoke;

    float smokeDuration = 5;   
    float smokeStart; 

    private IEnumerator smokeCoroutine()
    {
        smoke.Play();
        smokeStart = Time.time;
        yield return new WaitForSeconds(smokeDuration);
        bulletsFired = 0;
        smoke.Stop();
    }
    void Start()
    {
        bulletsFired = 0;
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
        hipPos = transform.localPosition;
        adsPos = new Vector3(0,-0.095f,0.8f);
        anim = GetComponentInChildren<Animator>();
    }

    void TiltWeapon(){
        //tilts weapon according to aim
        float tiltAmount = ads ? 1f : 5f;
        float smoothTime = ads ? 0.05f : 0.1f;

        float x = GameManager.Player.xRotation.x  * tiltAmount;
        float y = GameManager.Player.yRotation.y * tiltAmount;
        float z =  x + y;
        Quaternion smoothedRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(x, y, z), smoothTime);
        transform.localRotation = smoothedRotation;
    }

    void Update()
    {
        TiltWeapon();
        
        if(!firing && !smoke.isPlaying && bulletsFired > 0){
            StartCoroutine(smokeCoroutine());
        }

        

        if(smoke.isPlaying){
            float percent = 1 - (Time.time - smokeStart) / smokeDuration;
            smoke.startColor = new Color(1, 1, 1, Mathf.Clamp(percent, 0, 0.5f));
        }

        anim.SetBool("firing", firing);
        anim.SetBool("walking", GameManager.Player.walking);
        if(ads){
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsPos, Time.deltaTime * GameManager.adsSpeed);
            GameManager.PlayerCamera.ads = true;
            return;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, hipPos, Time.deltaTime * GameManager.adsSpeed);
        GameManager.PlayerCamera.ads = false;
    }
}
