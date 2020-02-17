using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Animator anim;
    public bool firing, ads, crouch, jump, reload;
    Vector3 hipPos, adsPos, crouchPos;
    public int bulletsFired, ammoCapacity, reserveAmmo;
    int ammoRemaining;
    ParticleSystem smoke;
    float smokeDuration = 5;   
    float smokeStartTime;
    public GameObject bullet;
    Text ammoCounter;
    public AudioSource source;
    public AudioClip magOutSound;
    public AudioClip magInSound;

    void playSound(AudioClip clip)
    {
        source.loop = false;
        source.clip = clip;
        source.Play();
    }    

    public void playMagOut()
    {
        playSound(magOutSound);
    }
    public void playMagIn()
    {
        playSound(magInSound);
    }

    private IEnumerator smokeCoroutine()
    {
        smoke.Play();
        smokeStartTime = Time.time;
        yield return new WaitForSeconds(smokeDuration);
        bulletsFired = 0;
        smoke.Stop();
    }
    void Start()
    {
        ammoCounter = GameObject.Find("/Canvas/Ammo").GetComponent<Text>();
        ammoRemaining = ammoCapacity;
        source = gameObject.GetComponent<AudioSource>();
        bulletsFired = 0;
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
        hipPos = transform.localPosition;
        adsPos = new Vector3(0,-0.095f,0.8f);
        crouchPos = new Vector3(0,-0.4f,0.9f);
        anim = GetComponentInChildren<Animator>();
    }

    public void Fire()
    {
        if(ammoRemaining <= 0) return;
        ammoRemaining --;
        Vector3 spawnPos =  GameObject.FindWithTag("Bullet Spawn").transform.position;
        Instantiate(bullet, spawnPos, transform.rotation);
    }

    void TiltWeapon(){
        //tilts weapon according to aim
        float tiltAmount = ads ? 1f : 5f;
        float smoothTime = ads ? 5f : 10f;

        float x = GameManager.Player.xRotation.x  * tiltAmount;
        float y = GameManager.Player.yRotation.y * tiltAmount;
        float z =  x + y;
        Quaternion smoothedRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(x, y, z), smoothTime * Time.deltaTime);
        transform.localRotation = smoothedRotation;
    }

    void MoveWeapon(){
        //moves weapon according to player movement
        float smoothTime = 2.5f;
        var player = GameManager.Player;
        float x = transform.localPosition.x, z = transform.localPosition.z;

        if(player.forward) z += 0.1f;
        if(player.back) z -= 0.1f;
        if(player.left) x -= 0.1f;
        if(player.right) x += 0.1f;

        var newPos = new Vector3(x, transform.localPosition.y, z);
        if(player.walking){
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, smoothTime * Time.deltaTime);
        }
        
    }

    void HandleSmoke(){
        if(!firing && !smoke.isPlaying && bulletsFired > 20){
            StartCoroutine(smokeCoroutine());
        }

        if(smoke.isPlaying){
            float percent = 1 - (Time.time - smokeStartTime) / smokeDuration;
            smoke.startColor = new Color(1, 1, 1, Mathf.Clamp(percent, 0, 0.5f));
        }
    }

    public void FinishReload(){
        reload = false;
        if(reserveAmmo < ammoCapacity){
            ammoRemaining = reserveAmmo;
            reserveAmmo = 0;
        }
        else{
            int bulletsToReload = ammoCapacity - ammoRemaining;
            reserveAmmo -= bulletsToReload;
            ammoRemaining = ammoCapacity;
        }
    }

    public void StartReload(){
        if(!reload && reserveAmmo > 0 && ammoRemaining != ammoCapacity){
            reload = true;
            anim.SetTrigger("reload");
        }
    }

    void HandleFiring(){
        ammoCounter.text = ammoRemaining.ToString() + "/" + reserveAmmo.ToString();
        if(ammoRemaining <=  0){
            anim.SetBool("firing", false);
            return;
        }  
        anim.SetBool("firing", firing);
    }

    void HandleMovement(){
        anim.SetBool("walking", GameManager.Player.walking);
        anim.SetBool("sprinting", GameManager.Player.sprinting);
    }

    void Update()
    {
        HandleSmoke();

        HandleFiring();

        HandleMovement();

        Vector3 currentAngle = transform.localRotation.eulerAngles;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        if(ads){
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsPos, Time.deltaTime * GameManager.adsSpeed);
            GameManager.PlayerCamera.ads = true;
            return;
        }
        else if(crouch){
            // transform.localPosition = Vector3.Lerp(transform.localPosition, crouchPos, Time.deltaTime * GameManager.adsSpeed);
            transform.localPosition = crouchPos;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 60f);
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, hipPos, Time.deltaTime * GameManager.adsSpeed);
        GameManager.PlayerCamera.ads = false;
    }

    void LateUpdate(){
        TiltWeapon();
        MoveWeapon();
    }
}
