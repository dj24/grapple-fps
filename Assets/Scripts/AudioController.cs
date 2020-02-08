using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip groundSound;
    public AudioClip stepSound;
    public AudioSource source;
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    void playSound(AudioClip clip)
    {
        source.loop = false;
        source.clip = clip;
        source.Play();
    }    

    void playLoopingSound(AudioClip clip)
    {
        if (!source.isPlaying)
        {
            source.loop = true;
            playSound(clip);
        }
    }

    public void playJump()
    {
        playSound(jumpSound);
    }

     public void playStep()
    {
        playSound(stepSound);
    }

    public void playShoot()
    {
        playSound(shootSound);
    }

    public void playHitGround()
    {
        playSound(groundSound);
    }

    public void playWalkSound()
    {
        // playLoopingSound(walkingSound);
    }

    public void stopLooping(){
        source.loop = false;
        source.Stop();
    }

}