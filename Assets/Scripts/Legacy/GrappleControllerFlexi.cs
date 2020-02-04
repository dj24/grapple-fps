using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleControllerFlexi : MonoBehaviour
{
    PlayerController player;
    Transform grappleTarget;
    Rigidbody rb;
    float maxDistance = 100f;
    Vector3 target;
    Object grappledObject;
    public GameObject rope;
    GameObject newRope; // The instantianted rope instance
    public float smoothTime = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grappleTarget = GameObject.FindGameObjectWithTag("GrappleTarget").transform;
        player = GameManager.Player;
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
            }
            if (grappledObject)
            {   
                //TODO: fix this to only be set on initial fire
                if (player.grapple){
                    grappledObject.SetLocalPoint(hit.point);
                }    
            }
            return;
        }
        crosshair.color = new Color32(255, 255, 255, 50);
    }

    void FixedUpdate()
    {
        GetTarget();
        player.grounded = false;
    }

    private void Update()
    {
        if (grappledObject)
        {
            if (player.grapple)
            {
                GameManager.Audio.playShoot();
                newRope = Instantiate(rope, grappledObject.globalPoint, Quaternion.identity);
            }
            if(newRope!= null){
                newRope.GetComponent<RopeGenerator>().anchor = grappledObject.globalPoint;    
            }
            
        }
    }
}
