using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    RectTransform rect;
    Vector3 offset;
    Animator anim;
    float enemyHeight;
    public int totalDamage;
    public GameObject enemy;
    void Start()
    {
        totalDamage = 0;
        anim = GetComponentInChildren<Animator>();
        rect = GetComponent<RectTransform>();
        enemyHeight = 2f;
        offset = new Vector3(rect.rect.width / 2, rect.rect.height / 2, 0);
    }

    void Update()
    {
        Vector3 enemyPos = enemy.transform.position;
        Vector3 aboveEnemy = new Vector3(enemyPos.x, enemyPos.y + enemyHeight, enemyPos.z);
        Vector3 enemyScreenPos = Camera.main.WorldToScreenPoint(aboveEnemy) - offset;
        rect.anchoredPosition = enemyScreenPos;
    }

    // TODO: fix this initially diosplaying 0
    public void AddDamage(int damage)
    {
        if(anim != null){
            anim.SetTrigger("Damage");
        }
        totalDamage += damage;
    }

    public void Remove()
    {
        Destroy (gameObject);
    }
}
