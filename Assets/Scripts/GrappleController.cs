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
    Vector3 target;
    RopeController rope;
    Object grappledObject;
    public float smoothTime = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rope = GameManager.Rope;
        grappleTarget = GameObject.FindGameObjectWithTag("GrappleTarget").transform;
        player = GameManager.Player;
    }

    void GetTarget()
    {
        if(rope.active){
            return;
        }
        RaycastHit hit;
        Image crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);

        bool castHitTarget = Physics.Raycast(transform.position, fwd, out hit, maxDistance);
        if(!castHitTarget){
            crosshair.color = new Color32(255, 255, 255, 10);
            return;
        }
        crosshair.color = new Color32(255, 0, 0, 100);

        bool objectIsMoving = hit.collider.gameObject.GetComponent<Object>() != null;

        if(objectIsMoving){
            grappledObject = hit.collider.gameObject.GetComponent<Object>();
        }
        else{
            grappledObject = null;
        }
        // Sets grapple end point before activating to stop weird jitter when adjusting.
        if(!rope.active){
            if(objectIsMoving){
                grappledObject.SetLocalPoint(hit.point);
                return;
            }
            rope.end = hit.point;
            return;
        }
    }

    void FixedUpdate()
    {
        GetTarget();
        // player.grounded = false;
    }

    private void Update()
    {
        Vector3 leftHandPos = Camera.main.transform.position + Camera.main.transform.forward - Camera.main.transform.right - Camera.main.transform.up * 0.5f;
        rope.start = leftHandPos;

        if(grappledObject){
            rope.end = grappledObject.globalPoint;
        }

        if (player.grapple)
        {
            rope.active = true;
            GameManager.Audio.playShoot();
        }

    }
}
