using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Vector3 localPoint, globalPoint;

    public void SetLocalPoint(Vector3 point)
    {
        //TODO: fix local point not being set
        localPoint = transform.InverseTransformPoint(point);
    }

    void Start()
    {
        globalPoint = transform.TransformPoint(localPoint);
    }
}
