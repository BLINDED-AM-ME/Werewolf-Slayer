using UnityEngine;
using System.Collections;

public class MiniBossSpawner : MonoBehaviour {

	public delegate void Callback();

	public MiniBoss_Controller miniBoss_prefab;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnMiniBoss(Callback callback){

		MiniBoss_Controller boss = Instantiate(miniBoss_prefab, new Vector3(100,0,0), Quaternion.identity) as MiniBoss_Controller;

		boss.onDefeatCall = delegate (){
			callback();
		};
	}

}
