using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleController : MonoBehaviour
{
    PlayerController player;
    Transform grappleTarget;
    Rigidbody rb;
    float maxDistance = 100f;
    RopeController rope;
    Vector3 target;
    bool active;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rope = GameManager.Rope;
        grappleTarget = GameObject.FindGameObjectWithTag("GrappleTarget").transform;
        player = GameManager.Player;
    }

    void ResetGrapple()
    {
        rope.transform.rotation = new Quaternion(0,0,0,0);
        rope.transform.localScale = Vector3.zero;
    }

    void GetTarget()
    {
        RaycastHit hit;

        Image crosshair = GameObject.Find("Crosshair").GetComponent<Image>();

        Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, maxDistance) && hit.collider.gameObject.tag == "GrappleTarget")
        {
            crosshair.color = new Color32(255, 0, 0, 100);
            if (!active && player.grapple)
            {
                active = true;
                target = hit.point;
            }
            return;
        }
        crosshair.color = new Color32(255, 255, 255, 50);
    }

    void FixedUpdate()
    {
        if (!player.grapple || rope.length > maxDistance)
        {
            active = false;
            ResetGrapple();
            return;
        }

        GetTarget();

        rope.start = transform.position;
        rope.end = target;

        player.grounded = false;

        float speed = Mathf.Pow((maxDistance - rope.length),1.1f);

        rb.AddForce(-rope.direction * Time.fixedDeltaTime * speed);
    }
}
