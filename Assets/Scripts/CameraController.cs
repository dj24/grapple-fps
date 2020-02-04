﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    float originalFov, adsFov = 40;
    public bool ads = false;
    void Start()
    {
        cam = GetComponent<Camera>();
        originalFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if(ads){
            cam.fieldOfView = adsFov;
            return;
        }
        cam.fieldOfView = originalFov;
    }
}
