using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameObject enemy;
    void Start()
    {
      enemy = Resources.Load("TestEnemy") as GameObject;
      StartCoroutine (Spawn());    
    }

    IEnumerator Spawn()
    {
        while(true) 
         { 
             Instantiate(enemy);
             yield return new WaitForSeconds(1f);
         }
    }
}
