using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : Object
{
    public float offset;

    void Update()
    {
        offset = Mathf.Sin(Time.time) * 10f;
        transform.position = new Vector3(offset, transform.position.y);
        globalPoint = transform.TransformPoint(localPoint);
    }
}
