using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {

	public delegate void Callback();

	public Transform[] spawn_points;
	[Range(0.1f, 1.0f)]
	public float spawn_delay = 1.0f;

	public int enemy_number = 5;
	
	public int enemy_attackChance_high = 50;
	public int enemy_attackChance_walking = 100;
	public int enemy_attackChance_low = 25;


	public BasicEnemy_Controller enemy_high_prefab;
	public BasicEnemy_Controller enemy_mid_prefab;
	public BasicEnemy_Controller enemy_low_prefab;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartWave(float startDelay, Callback callback){

		StartCoroutine(SpawnCycle(startDelay, enemy_number, callback));
	}

	public IEnumerator SpawnCycle(float startDelay, int numOfEnemies, Callback callback){

		Debug.Log("wave started");

		yield return new WaitForSeconds(startDelay);

		for(int i=0; i<numOfEnemies; i++){
			yield return StartCoroutine(Scheldule_NextSpawn());
		}


		while(Enemy_Controller.controllers.Count > 0)
			yield return null;

		callback();
		
	}

	private int spawn_iterator = 0;
	IEnumerator Scheldule_NextSpawn(){
		
		BasicEnemy_Controller.AttackType newEnemy;
		
		int high    = Random.Range(0, enemy_attackChance_high);
		int walking = Random.Range(0, enemy_attackChance_walking);
		int low     = Random.Range(0, enemy_attackChance_low);
		
		if(high > walking && high > low){
			newEnemy = BasicEnemy_Controller.AttackType.high;
			
		}else if(walking > low){
			newEnemy = BasicEnemy_Controller.AttackType.mid;
			
		}else{
			newEnemy = BasicEnemy_Controller.AttackType.low;
		}

		yield return new WaitForSeconds(spawn_delay);

		spawn_iterator = (spawn_iterator+1) % spawn_points.Length;
		
		if(enabled){
			
			switch(newEnemy){
				
			case BasicEnemy_Controller.AttackType.high:
				Instantiate(enemy_high_prefab, spawn_points[spawn_iterator].position, Quaternion.identity);
				break;
				
			case BasicEnemy_Controller.AttackType.mid:
				Instantiate(enemy_mid_prefab, spawn_points[spawn_iterator].position, Quaternion.identity);
				break;
				
			case BasicEnemy_Controller.AttackType.low:
				Instantiate(enemy_low_prefab, spawn_points[spawn_iterator].position, Quaternion.identity);
				break;
				
			}
			
		}
		
	}
}
