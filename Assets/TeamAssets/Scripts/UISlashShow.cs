using UnityEngine;
using System.Collections;

public class UISlashShow : MonoBehaviour {
	public static UISlashShow instance;

	public GameObject slashShow_prefab;

	void Awake(){
	
		instance = this;
	}

	public static void Slash(Vector3 point, Vector3 dir) {
	
		float angle = Vector3.Angle(dir, Vector3.right);

		if(dir.y < 0)
			angle = 360.0f - angle;

		GameObject newSlash = Instantiate(instance.slashShow_prefab, point, Quaternion.Euler(0,0,angle)) as GameObject;

		Destroy(newSlash, 1.0f);

	}
}
