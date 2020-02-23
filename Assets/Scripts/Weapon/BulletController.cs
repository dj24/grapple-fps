using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float speed = 1000f;
    public GameObject explosionPrefab;
    void Start(){
        Destroy (gameObject, 1f);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
