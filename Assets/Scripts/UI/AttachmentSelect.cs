using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class AttachmentSelect : MonoBehaviour
{   
    public Transform target;
    RectTransform rect;
    GameObject sightSlot;
    CanvasScaler canvas;
    List<GameObject> sights, sightIcons;

    private Color32 activeColour =  new Color32(255,255,255,255), inactiveColour =  new Color32(255,255,255,100);

    public void SetActiveSight(string name){
        if(sights.Count == 0){
            return;
        }
        for(int i = 0; i < sights.Count; i++){
            sights[i].SetActive(sights[i].name == name);
            sightIcons[i].GetComponent<Image>().color = sights[i].name == name ? activeColour : inactiveColour;
        }
    }

    void PopulateSlots()
    {
        foreach (Transform child in sightSlot.transform){
            sights.Add(child.gameObject);
            child.gameObject.SetActive(false);
            var currentIcon = Resources.Load("UI/Sight Icon") as GameObject;
            currentIcon = Instantiate (currentIcon, transform);
            currentIcon.name = child.name;
            currentIcon.GetComponentsInChildren<Text>()[0].text = child.name;
            sightIcons.Add(currentIcon);
        }
        SetActiveSight(sights[0].name);
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<CanvasScaler>();
        sightSlot = GameObject.Find("Sight Slot");
        sights = new List<GameObject>(); 
        sightIcons = new List<GameObject>(); 
        PopulateSlots();
    }

    void Update()
    {
        if (canvas == null)
            return;

        var offset = new Vector3(rect.rect.width / 2, rect.rect.height / 2, 0);
        var anchoredPosition = Camera.main.WorldToScreenPoint(target.position) - offset;

        // Need to scale for the 1280x720 reference resolution of the UI
        var xScale = canvas.referenceResolution.x / Screen.width;
        var yScale = canvas.referenceResolution.y / Screen.height;
        anchoredPosition = new Vector3(anchoredPosition.x * xScale, anchoredPosition.y * yScale, 0);

        rect.anchoredPosition = anchoredPosition;
    }
}
