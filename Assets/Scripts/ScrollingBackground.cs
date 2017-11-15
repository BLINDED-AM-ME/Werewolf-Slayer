using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollingBackground : MonoBehaviour {

	public float area = 20.0f;
	
	[Range(0.0f, 1.0f)]
	public float speed = 1.0f;
	public float maxSpeed = 5.0f;

	private Vector3[]points;


	public static List<ScrollingBackground> controllers = new List<ScrollingBackground>(20);
	
	void Awake(){
		controllers.Add(this);
	}
	void OnDestroy(){
		controllers.Remove(this);	
	}


	// Use this for initialization
	void Start () {
	
		points = new Vector3[]{
			transform.TransformPoint(new Vector3(-area*0.5f, 0, 0)),
			transform.TransformPoint(new Vector3(area*0.5f, 0, 0))
		};

	}
	
	// Update is called once per frame
	void Update () {

		foreach(Transform tree in transform){
			MoveThis(tree);
		}
		
	}

	void MoveThis(Transform tree){
		
		tree.Translate((maxSpeed * speed) * 
		               -transform.right * Time.deltaTime, Space.World);
		
		float xdisplacement = transform.InverseTransformPoint(points[0]).x - transform.InverseTransformPoint(tree.position).x;
		if(xdisplacement >= 0.0f){
			xdisplacement = xdisplacement*transform.lossyScale.x;

			Vector3 relativePos = transform.InverseTransformPoint(tree.position); // local
			// want to keep its y,z
			relativePos.Scale(transform.lossyScale);
			relativePos.x = 0.0f;

			Vector3 targetPos = points[1] - transform.right * xdisplacement;
			targetPos += transform.TransformDirection(relativePos);

			tree.position = targetPos;
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
		Vector3[] points = new Vector3[]{
			transform.TransformPoint(new Vector3(-area*0.5f, 0, 0)),
			transform.TransformPoint(new Vector3(area*0.5f, 0, 0))
		};

		Gizmos.DrawLine(points[0], points[1]);
		
	}
}
