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
    MovingObject grappledObject;
    bool active;

    public GameObject ropeObj;

    public float smoothTime = 0.3f;

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
            // Sets grapple end point before activating to stop weird jitter when adjusting.
            if (!player.grapple)
            {
                grappledObject = hit.collider.gameObject.GetComponent<MovingObject>();
                if (grappledObject)
                {
                    grappledObject.SetLocalPoint(hit.point);
                }
            }
            if (rope && !rope.active && player.grapple)
            {
                rope.active = true;
                //TODO: make this extend a base object class and remove this if              
            }
            return;
        }
        crosshair.color = new Color32(255, 255, 255, 50);
    }

    void MoveTowardsPoint()
    {
        float speed = Mathf.Pow((maxDistance - rope.length), 1);

        //float speed = 0;

        rb.AddForce(-rope.direction * Time.fixedDeltaTime * speed);
    }

    void FixedUpdate()
    {
        GetTarget();

        if (!rope)
        {
            return;
        }

        if (!player.grapple || rope.length > maxDistance)
        {
            rope.active = false;
            grappledObject = null;
        }

        if (!rope.active)
        {
            return;
        }

        player.grounded = false;

        MoveTowardsPoint();
    }

    private void Update()
    {
        //rope.start = transform.position;

        if (grappledObject)
        {
            //rope.end = grappledObject.globalPoint;
            
            if (player.grapple)
            {
                Instantiate(ropeObj, grappledObject.globalPoint, Quaternion.identity);
            }
        }
    }
}
