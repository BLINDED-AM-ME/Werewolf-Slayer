using UnityEngine;
using System.Collections;

public class Game_Controller : MonoBehaviour {

	public static Game_Controller instance;
	public static bool isGameOver = false;

	public int frameRate = 60;

	WaveSpawner     waveSpawner;
	MiniBossSpawner miniBossSpawner;

	
	private float lineZPostion;

	public struct Slashing{

		public Vector2 startPoint;
		public Vector2 endPoint;

	}
	private Slashing[] userSlashes = new Slashing[6]; // one for each finger
	public float       userSlash_minDistance = 0.1f;
	public Transform[] userSlashTrails = new Transform[6];
	public LayerMask slashMask = 0; // default is nothing
	private int userSlashIterator = 0;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		lineZPostion =  Camera.main.transform.position.z + Camera.main.nearClipPlane;

		Application.targetFrameRate = frameRate;
		Input.multiTouchEnabled = false;

		waveSpawner = GetComponent<WaveSpawner>();
		miniBossSpawner = GetComponent<MiniBossSpawner>();

	}

	void OnValidate(){

	}

	
	private bool isReadyForNewWave = false;
	private bool isReadyForMiniBoss = false;

	public void GONOW(){

		isReadyForNewWave = true;
		isGameOver = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(isReadyForNewWave){
			isReadyForNewWave = false;

			waveSpawner.StartWave(0.5f, delegate() {

				PromptText.AddMessage("wave is done, here comes the big guy", 2.0f);

				isReadyForMiniBoss = true;
			});

			waveSpawner.enemy_number *= 2;
		}

		if(isReadyForMiniBoss){
			isReadyForMiniBoss = false;

			miniBossSpawner.SpawnMiniBoss(delegate() {
				PromptText.AddMessage("big guy is done", 2.0f);
				isReadyForNewWave = true;
			});
		}

		// remove due to tapCount
		//if(Application.isMobilePlatform && !Application.isEditor)
		//	MobileTouchInput();
		//else
			MouseInput();


	}


	void MouseInput(){

		if(Input.GetMouseButtonDown(0)){

			// get the next one
			userSlashIterator = (userSlashIterator+1) % userSlashTrails.Length;

			// set slash values
			userSlashes[userSlashIterator].startPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			userSlashes[userSlashIterator].endPoint = userSlashes[userSlashIterator].startPoint;  

			// move the trail
			userSlashTrails[userSlashIterator].position = new Vector3(userSlashes[userSlashIterator].endPoint.x,  userSlashes[userSlashIterator].endPoint.y, lineZPostion);

		}
		
		if(Input.GetMouseButton(0)){

			// set the new endpoint
			userSlashes[userSlashIterator].endPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// move the trail
			userSlashTrails[userSlashIterator].position = new Vector3(userSlashes[userSlashIterator].endPoint.x,  userSlashes[userSlashIterator].endPoint.y, lineZPostion);

			// check the drag distance
			if(Vector2.Distance(userSlashes[userSlashIterator].startPoint, userSlashes[userSlashIterator].endPoint) >= userSlash_minDistance){

				Attack(userSlashes[userSlashIterator]);
				userSlashes[userSlashIterator].startPoint = userSlashes[userSlashIterator].endPoint;
			}


		}
		
		if(Input.GetMouseButtonUp(0)){

			// set the new endpoint
			userSlashes[userSlashIterator].endPoint = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// move the trail
			userSlashTrails[userSlashIterator].position = new Vector3(userSlashes[userSlashIterator].endPoint.x,  userSlashes[userSlashIterator].endPoint.y, lineZPostion);

			Attack(userSlashes[userSlashIterator]);
			
		}	
	}

	private RaycastHit2D[] slashResults = new RaycastHit2D[20];
	void Attack(Slashing slash){

		int numVictims = Physics2D.LinecastNonAlloc(slash.startPoint,slash.endPoint, slashResults, slashMask);
		for(int i=0; i<numVictims; i++){
		
			Enemy_Controller victim = slashResults[i].collider.GetComponent<Enemy_Controller>();

			if(victim.InputUserSlash((slash.endPoint - slash.startPoint).normalized)){
				// hit
				UISlashShow.Slash( Vector2.Lerp(slash.endPoint, slash.startPoint, 0.5f), (slash.endPoint - slash.startPoint).normalized);
				Player_Controller.instance.Attack(victim);
			}else{
				Player_Controller.instance.Attack(victim);
			}

		}
	}


	public void Reset(){
		Application.LoadLevel(0);
	}


	public static void StopScrollingScenery(){

		ScrollingBackground[] scrolls = GameObject.FindObjectsOfType<ScrollingBackground>();
		foreach(ScrollingBackground scroll in scrolls)
			scroll.speed = 0;

	}

	public static void StartScrollingScenery(){
		
		ScrollingBackground[] scrolls = GameObject.FindObjectsOfType<ScrollingBackground>();
		foreach(ScrollingBackground scroll in scrolls)
			scroll.speed = 1;
		
	}



	public static void LoseScenario(Enemy_Controller killer){

		isGameOver = true;

		instance.enabled = false;
		instance.waveSpawner.enabled = false;

		StopScrollingScenery();

		Enemy_Controller.LoseScenario(killer);
		Player_Controller.LoseScenario(killer);
		
	}

	public void LoadLevel(int index){
		Application.LoadLevel(index);
	}

}
