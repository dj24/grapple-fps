using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public bool firing, ads, jump, reload, inspect;
    Vector3 hipPos, adsPos;
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
        var ammoCounterUI = GameObject.FindWithTag("AmmoCount");
        if(ammoCounter){
            ammoCounter = ammoCounterUI.GetComponent<Text>();
        }
        ammoRemaining = ammoCapacity;
        source = gameObject.GetComponent<AudioSource>();
        bulletsFired = 0;
        hipPos = transform.localPosition;
        adsPos = new Vector3(0,-0.095f,0.8f);
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

        GameObject spawn =  GameObject.FindWithTag("Bullet Spawn");
        if(spawn){
            Vector3 spawnPos = spawn.transform.position;
            Instantiate(bullet, spawnPos, transform.rotation);
        }

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

    private void HandleSmoke(){
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
        if(ammoCounter){
            ammoCounter.text = ammoRemaining.ToString() + "/" + reserveAmmo.ToString();
        }
        if(ammoRemaining <=  0){
            anim.SetBool("firing", false);
            return;
        }  
        anim.SetBool("firing", firing);
    }

    void HandleMovement(){
        //TODO: Use status instead
        anim.SetBool("walking", GameManager.Player.walking);
        anim.SetFloat("walk/sprint",GameManager.Player.sprinting ? 1f : 0f, 1f, Time.deltaTime * 40f);
    }

    void ControlAds(){
        foreach(var obj in GameObject.FindGameObjectsWithTag("HideInAds")){
            foreach(var component in obj.GetComponents<Renderer>()){
                component.enabled = !ads;
            }
            foreach(var component in obj.GetComponents<MonoBehaviour>()){
                component.enabled = !ads;
            }
        }
        foreach(var obj in GameObject.FindGameObjectsWithTag("ShowInAds")){
            foreach(var component in obj.GetComponents<Renderer>()){
                component.enabled = ads;
            }
            foreach(var component in obj.GetComponents<MonoBehaviour>()){
                component.enabled = ads;
            }
        }

        anim.SetBool("ads",ads);
        Tween.EaseLayerWeight(anim, "ADS", ads);
        Tween.EaseLayerWeight(anim, "Weapon Tilt", ads, 1f, 0.25f);
        Tween.EaseLayerWeight(anim, "Weapon Move", ads, 0.25f, 0.025f);
    }


    void CheckForInspect(){
        var inspectProgress = Tween.EaseLayerWeight(anim, "Inspect", inspect);

        float nearStart = 30f;
        float nearEnd = 1f; 
        float nearCurrent = (nearStart + (nearEnd - nearStart))*inspectProgress;
        float near = Tween.Ease(nearCurrent,nearEnd);

        float farStart = 500f;
        float farEnd = 4f;
        float farCurrent = (farStart + (farEnd - farStart))*inspectProgress;
        float far = Tween.Ease(farCurrent,farEnd);

        Volume volume = GameObject.Find("Post Process Volume").GetComponent<Volume>();
        DepthOfField dof;
        volume.profile.TryGet<DepthOfField>( out dof );
    }
    void Update()
    {
        HandleSmoke();

        HandleFiring();

        HandleMovement();

        Vector3 currentAngle = transform.localEulerAngles;
        Vector3 targetPos;
        
        float targetAngle;

        ads = reload ? false : ads;
        GameManager.PlayerCamera.ads = ads;
        
       
        targetPos = ads ? adsPos : hipPos;
        targetAngle = 0;

        ControlAds();

        CheckForInspect();
        

        float smoothedRotation = Mathf.SmoothDamp(currentAngle.z, targetAngle, ref velocity, Time.deltaTime * GameManager.adsSpeed);

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0,0,currentAngle.z), Quaternion.Euler(0,0,targetAngle), Time.deltaTime * GameManager.adsSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * GameManager.adsSpeed);
        // transform.localEulerAngles = new Vector3(0, 0, smoothedRotation);
    }
}
