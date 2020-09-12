using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip groundSound;
    public AudioClip walkingSound;
    public AudioClip sprintingSound;
    public AudioClip stepSound;
    public AudioSource source;
    public AudioClip impactSound;
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
        source.loop = true;
        if (!source.isPlaying)
        {
            source.clip = clip;
            source.Play();
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
    public void playHit()
    {
        playSound(impactSound);
    }

    public void playHitGround()
    {
        playSound(groundSound);
    }

    public void playWalkSound()
    {
        playLoopingSound(walkingSound);
    }

    public void playSprintSound()
    {
        playLoopingSound(sprintingSound);
    }


    public void stopLooping(){
        source.loop = false;
        source.Stop();
    }

}