using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    int health;

    private void SetGravtiy(bool status){
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidBodies){
            rb.useGravity = status;
            rb.isKinematic = !status;
        }       
    }
    private void Start(){
        SetGravtiy(false);
        anim = GetComponent<Animator>();
        health = 100;
    }

    void Die(){
        SetGravtiy(true);
        anim.enabled = false;
    }
    public void TakeDamage(Vector3 force, Rigidbody rb){
        anim.SetTrigger("Hit");
        health -= 30;
        if(health <= 0) Die();
        rb.AddForce(force * 10000f);
    }
}
