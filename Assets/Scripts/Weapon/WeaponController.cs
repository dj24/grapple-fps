using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public bool firing, ads, jump, reload;
    Vector3 hipPos, adsPos, crouchPos;
    [HideInInspector]
    public int bulletsFired, ammoCapacity, reserveAmmo;
    int ammoRemaining;
    ParticleSystem smoke;
    float smokeDuration = 5;   
    float smokeStartTime;
    float maxDistance = 500f;
    public GameObject bullet;
    Text ammoCounter;
    [HideInInspector]
    public AudioSource source;
    public AudioClip magOutSound, magInSound;
    public GameObject explosionPrefab;

    private float velocity = 0;

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
        hipPos = transform.localPosition;
        adsPos = new Vector3(0,-0.095f,0.8f);
        crouchPos = new Vector3(0,-0.4f,0.9f);
        anim = GetComponentInChildren<Animator>();
        smoke = GetComponentInChildren<ParticleSystem>();
        if(smoke == null){
            return;
        }
        smoke.Stop();
    }

    public void Fire()
    {
        if(ammoRemaining <= 0) return;
        ammoRemaining --;

        Vector3 spawnPos =  GameObject.FindWithTag("Bullet Spawn").transform.position;
        Instantiate(bullet, spawnPos, transform.rotation);

        RaycastHit hit;
        Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);
        bool castHitTarget = Physics.Raycast(Camera.main.transform.position, fwd, out hit, maxDistance);
        if(!castHitTarget)
        {
            return;
        }
        if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Quaternion rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);
            GameObject explosion = Instantiate(explosionPrefab, hit.point, rotation);
            Destroy (explosion, 0.5f);
            Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
            hit.transform.gameObject.GetComponentInParent<EnemyController>().TakeDamage(fwd,rb);
        }
        
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

        // change to use player direction
        // if(player.forward) z += 0.1f;
        // if(player.back) z -= 0.1f;
        // if(player.left) x -= 0.1f;
        // if(player.right) x += 0.1f;

        var newPos = new Vector3(x, transform.localPosition.y, z);
        // if(player.walking){
        //     transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, smoothTime * Time.deltaTime);
        // }
        
    }

    void HandleSmoke(){
        if(!smoke){
            return;
        }
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
        //TODO: Use status instead
        anim.SetBool("walking", GameManager.Player.walking);
        anim.SetBool("spriting", GameManager.Player.sprinting);
        // anim.SetBool("sprinting", GameManager.Player.sprinting);
    }

    void Update()
    {
        HandleSmoke();

        HandleFiring();

        HandleMovement();

        Vector3 currentAngle = transform.localEulerAngles;
        Vector3 targetPos;
        GameManager.PlayerCamera.ads = ads;
        float targetAngle;

        if(ads){
            targetPos = adsPos;
            targetAngle = 0;
        }
        else if(GameManager.Player.crouching){
            targetPos = crouchPos;
            targetAngle = 60f;
        }
        else{
            targetPos = hipPos;
            targetAngle = 0;
        }
        float smoothedRotation = Mathf.SmoothDamp(currentAngle.z, targetAngle, ref velocity, Time.deltaTime * GameManager.adsSpeed);
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0,0,currentAngle.z), Quaternion.Euler(0,0,targetAngle), Time.deltaTime * GameManager.adsSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * GameManager.adsSpeed);
        // transform.localEulerAngles = new Vector3(0, 0, smoothedRotation);
    }

    void LateUpdate(){
        TiltWeapon();
        MoveWeapon();
    }
}
