using UnityEngine;
using System.Collections;

public class ParallaxBackgroundObjects : MonoBehaviour {

	Transform[] levels;

	public Vector2 area = new Vector2(20,10);

	[Range(0.0f, 1.0f)]
	public float speed = 1.0f;
    public float maxSpeed = 10.0f;

	private Vector3[] fourCorners;

	// Use this for initialization
	void Start () {
	
		fourCorners = new Vector3[]{
			transform.TransformPoint(new Vector3(-area.x*0.5f, 0.0f, 0)),
			transform.TransformPoint(new Vector3(-area.x*0.5f, 0.0f, area.y)),
			transform.TransformPoint(new Vector3(area.x*0.5f, 0.0f, area.y)),
			transform.TransformPoint(new Vector3(area.x*0.5f, 0.0f, 0)),
		};

		levels = new Transform[transform.childCount];
		for(int i=0; i<transform.childCount; i++){
			levels[i] = transform.GetChild(i);
		}

	}
	
	// Update is called once per frame
	void Update () {


		if(levels.Length != transform.childCount){
			levels = new Transform[transform.childCount];
			for(int i=0; i<transform.childCount; i++){
				levels[i] = transform.GetChild(i);
			}
		}

		MoveTheScenery();

	}


	void MoveTheScenery(){

		float distance = 0.0f;
		for(int i=0; i<levels.Length; i++){

			distance = (float) i/(float) (transform.childCount-1);
			distance *= area.y;

			foreach(Transform tree in levels[i]){
				MoveThis(tree, distance);
			}
		}
	}

	void MoveThis(Transform tree, float distance){

		tree.Translate((maxSpeed * distance) * 
		               Vector3.left * Time.deltaTime, transform);

		float xdisplacement = (-area.x * 0.5f) - transform.InverseTransformPoint(tree.position).x;
		if(xdisplacement >= 0.0f){
			tree.position = tree.parent.position + (-transform.right * xdisplacement);
		}
	}

	void OnValidate(){

	}

	private void OnDrawGizmos()
	{
		DrawGizmos(false);
	}
	
	
	private void OnDrawGizmosSelected()
	{
		DrawGizmos(true);

	}
	
	
	private void DrawGizmos(bool selected)
	{

		Gizmos.color = selected ? Color.green : new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
		fourCorners = new Vector3[]{
			transform.TransformPoint(new Vector3(-area.x*0.5f, 0.0f, 0)),
			transform.TransformPoint(new Vector3(-area.x*0.5f, 0.0f, area.y)),
			transform.TransformPoint(new Vector3(area.x*0.5f, 0.0f, area.y)),
			transform.TransformPoint(new Vector3(area.x*0.5f, 0.0f, 0)),
		};

		Gizmos.DrawLine(fourCorners[0], fourCorners[1]);
		Gizmos.DrawLine(fourCorners[1], fourCorners[2]);
		Gizmos.DrawLine(fourCorners[2], fourCorners[3]);
		Gizmos.DrawLine(fourCorners[3], fourCorners[0]);

		if(transform.childCount == 0){
			GameObject obj = new GameObject("level 0");
			obj.transform.SetParent(transform);
		}


		for(int i=0; i<transform.childCount; i++){
			
			Transform child = transform.GetChild(i);
			child.name = "level "+i;
			child.position = Vector3.Lerp(fourCorners[3], fourCorners[2], (float) i/(float) (transform.childCount-1));
			Gizmos.DrawLine(child.position, child.position + -transform.right*area.x);
			
		}

	}

}
