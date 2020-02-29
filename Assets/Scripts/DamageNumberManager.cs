using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance { get; private set; }
    public static GameObject damagePrefab
    {
		get
		{
			return (GameObject)Resources.Load("DamageNumber", typeof(GameObject));
		}
	}
	
    public static void CreateDamage(GameObject enemy, int damage){
        if(enemy.GetComponent<EnemyController>().health <= 0){
            return;
        } 
        GameObject[] numbers = GameObject.FindGameObjectsWithTag("DamageNumber");
        for(int i = 0; i < numbers.Length; i++)
        {
            DamageNumbers currentNumber = numbers[i].GetComponent<DamageNumbers>();
            if(currentNumber.enemy == enemy)
            {
                currentNumber.AddDamage(damage);
                return;
            }
        }
        GameObject numberObj = Instantiate(DamageNumberManager.damagePrefab, GameManager.Canvas);
        DamageNumbers number = numberObj.GetComponent<DamageNumbers>();
        number.enemy = enemy.gameObject;
        number.AddDamage(damage);
    }
}
