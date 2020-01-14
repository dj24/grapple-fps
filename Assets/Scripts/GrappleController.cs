using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    PlayerController player;
    Transform rope;
    Transform grappleTarget;
    Rigidbody rb;
    Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rope = GameObject.Find("Grapple").transform;
        grappleTarget = GameObject.FindGameObjectWithTag("GrappleTarget").transform;
        player = GameManager.Player;
    }

    void ResetGrapple()
    {
        rope.rotation = new Quaternion(0,0,0,0);
        rope.localScale = Vector3.zero;
    }

    void CalculateRope()
    {
        Vector3 start = transform.position;
        Vector3 end = grappleTarget.position;

        direction = start - end;

        float distance = Vector3.Distance(end, start);
        rope.localScale = new Vector3(0.1f,distance / 2,0.1f);


        rope.position = (end - start) / 2.0f + start;
        rope.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
    }

    void FixedUpdate()
    {
        if (!player.grapple)
        {
            ResetGrapple();
            return;
        }
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 10, Color.yellow);
        rope.LookAt(grappleTarget, Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, 10))
        {
            print("There is something in front of the object!");
        }

        rb.AddForce(-direction);

        CalculateRope();
    }
}
