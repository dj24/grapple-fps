using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentHelper : MonoBehaviour
{
    AttachmentSelect select;
    void Start(){
        select = gameObject.GetComponentInParent<AttachmentSelect>();
    }
    public void SetActive(){
        select.SetActiveSight(gameObject.name);
    }
}
