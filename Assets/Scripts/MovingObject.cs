using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float offset;
    public Vector3 localPoint, globalPoint;

    public void SetLocalPoint(Vector3 point)
    {
        //TODO: fix local point not being set
        localPoint = transform.InverseTransformPoint(point);
    }

    void Update()
    {
        //offset = Mathf.Sin(Time.time * 2) * 40f;
        //transform.position = new Vector3(offset, transform.position.y);
        globalPoint = transform.TransformPoint(localPoint);
    }
}
