using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    public int health;
    float gravity =  75.0f;
    bool isGrounded;

    NavMeshAgent agent;

    // TODO: disable character controller on death
    private void SetGravtiy(bool status){
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidBodies){
            rb.useGravity = status;
            rb.isKinematic = !status;
        }       
    }

    void ApplyGravity(){
        if(!isGrounded){
            transform.Translate(Vector3.down * gravity * Time.deltaTime);
        }
    }
    private void Start(){
        SetGravtiy(false);
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = 100;
    }

    void Die(){
        SetGravtiy(true);
        anim.enabled = false;
        agent.enabled = false;
    }

    public void TakeDamage(Vector3 force, Rigidbody rb){
        anim.SetTrigger("Hit");
        int damage = 20;
        health -= damage;
        DamageNumberManager.CreateDamage(gameObject, damage);
        if(health <= 0) Die();
        if(rb){
            rb.AddForce(force * 10000f);
        }
    }

    void Update(){
        if(agent.enabled){
            agent.destination = GameManager.Player.transform.position;
            anim.SetFloat("speed", agent.velocity.magnitude);
        }
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f, 1 << LayerMask.NameToLayer("Ground"));
        ApplyGravity();
    }
}
