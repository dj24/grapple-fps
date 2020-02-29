using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberHelper : MonoBehaviour
{
    Text text;
    DamageNumbers dmg;
    void Start()
    {
         dmg = gameObject.GetComponentInParent(typeof(DamageNumbers)) as DamageNumbers;
        text = GetComponent<Text>();
    }
    void Update()
    {
        text.text = dmg.totalDamage.ToString();
    }
    public void Remove()
    {
        dmg.Remove();
    }
}
