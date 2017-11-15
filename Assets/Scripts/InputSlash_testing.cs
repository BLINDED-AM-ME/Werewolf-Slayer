using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InputSlash_testing : MonoBehaviour {

	public int frameRate = 60;
	private float lineZPostion;
	
	public struct Slashing{
		
		public int     fingerID;
		public Vector2 startPoint;
		public Vector2 endPoint;
		
	}
	private Slashing[] userSlashes = new Slashing[6]; // one for each finger
	public LineRenderer[] userSlashLines = new LineRenderer[6];

	private int userSlashIterator = 0;

	// Use this for initialization
	void Start () {
		lineZPostion =  Camera.main.transform.position.z + Camera.main.nearClipPlane;

		Time.timeScale = 1.0f;

		for(int s=0; s<userSlashes.Length; s++)
			userSlashes[s].fingerID = -1;
		
		Application.targetFrameRate = frameRate;

		Input.multiTouchEnabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		
	//	if(Application.isMobilePlatform && !Application.isEditor)
	//		MobileTouchInput();
	//	else
			MouseInput();
		
		
	}
	
	void MobileTouchInput(){
		
		int touchCount = Input.touchCount;
		
		for(int i=0; i<touchCount; i++){
			Touch finger = Input.GetTouch(i);

			switch(finger.phase){
				
			case TouchPhase.Began: // grab a slash
				
				for(int s=0; s<userSlashes.Length; s++){
					if(userSlashes[s].fingerID == -1){
						userSlashes[s].fingerID = finger.fingerId;
						
						userSlashes[s].startPoint = (Vector2) Camera.main.ScreenToWorldPoint(finger.position);
						userSlashes[s].endPoint   = userSlashes[s].startPoint;
						
						userSlashLines[s].startWidth = 0.0f;
						userSlashLines[s].endWidth = 0.5f;
						
						userSlashLines[s].SetPosition(0, new Vector3(userSlashes[s].startPoint.x,userSlashes[s].startPoint.y, lineZPostion));
						userSlashLines[s].SetPosition(1, new Vector3(userSlashes[s].endPoint.x,  userSlashes[s].endPoint.y, lineZPostion));
						
						break;
					}
				}
				
				break;
				
			case TouchPhase.Moved: // update your slash
				
				for(int s=0; s<userSlashes.Length; s++){
					if(userSlashes[s].fingerID == finger.fingerId){
						
						userSlashes[s].endPoint = (Vector2) Camera.main.ScreenToWorldPoint(finger.position);
						
						userSlashLines[s].SetPosition(1, new Vector3(userSlashes[s].endPoint.x,  userSlashes[s].endPoint.y, lineZPostion));
						
						break;
					}
				}
				
				break;
				
			case TouchPhase.Ended: // complete your slash
				
				for(int s=0; s<userSlashes.Length; s++){
					if(userSlashes[s].fingerID == finger.fingerId){
						
						userSlashes[s].endPoint = (Vector2) Camera.main.ScreenToWorldPoint(finger.position);
						
						userSlashLines[s].SetPosition(1, new Vector3(userSlashes[s].endPoint.x,  userSlashes[s].endPoint.y, lineZPostion));
						userSlashLines[s].GetComponent<Animation>().Play();

						StartCoroutine(ResetSlashing(s));
						
						break;
					}
				}
				
				break;
				
				
			case TouchPhase.Canceled: // cancel your slash
				
				for(int s=0; s<userSlashes.Length; s++){
					if(userSlashes[s].fingerID == finger.fingerId){
						
						userSlashes[s].fingerID = -1;
						userSlashes[s].endPoint = Vector2.zero;
						userSlashes[s].startPoint = Vector2.zero;
						userSlashLines[s].startWidth = 0.0f;
						userSlashLines[s].endWidth = 0.0f;
						
						break;
					}
				}
				
				break;
				
			}
			
		}
		
	}
	
	IEnumerator ResetSlashing(int index){
		
		yield return new WaitForSeconds(0.5f);
		
		userSlashes[index].fingerID = -1;
		userSlashes[index].startPoint = Vector2.zero;
		userSlashes[index].endPoint = Vector2.zero;
	}
	
	void MouseInput(){
		
		if(Input.GetMouseButtonDown(0)){
			
			userSlashIterator = (userSlashIterator+1) % userSlashLines.Length;

			userSlashLines[userSlashIterator].startWidth = 0.0f;
			userSlashLines[userSlashIterator].endWidth = 0.5f;
			
			userSlashes[userSlashIterator].startPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			userSlashes[userSlashIterator].endPoint   = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			userSlashLines[userSlashIterator].SetPosition(0, new Vector3(userSlashes[userSlashIterator].startPoint.x,userSlashes[userSlashIterator].startPoint.y, lineZPostion));
			userSlashLines[userSlashIterator].SetPosition(1, new Vector3(userSlashes[userSlashIterator].endPoint.x,  userSlashes[userSlashIterator].endPoint.y, lineZPostion));
		}
		
		if(Input.GetMouseButton(0)){
			userSlashes[userSlashIterator].endPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			userSlashLines[userSlashIterator].SetPosition(1, new Vector3(userSlashes[userSlashIterator].endPoint.x,  userSlashes[userSlashIterator].endPoint.y, lineZPostion));
		}
		
		if(Input.GetMouseButtonUp(0)){
			userSlashes[userSlashIterator].endPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			userSlashLines[userSlashIterator].SetPosition(1, new Vector3(userSlashes[userSlashIterator].endPoint.x,  userSlashes[userSlashIterator].endPoint.y, lineZPostion));
			userSlashLines[userSlashIterator].GetComponent<Animation>().Play();
			
		}	
	}


	public void LoadLevel(int index){
		SceneManager.LoadScene(index);
	}
}
