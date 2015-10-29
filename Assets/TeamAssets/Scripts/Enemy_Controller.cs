using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Controller : MonoBehaviour {
	
	[HideInInspector]
	public Animator animator;

	public Transform deathArrow;
	public Transform player_attack_position;


	private Vector2 killDir;
	public float killAngleDeathzone = 45.0f;

	[HideInInspector]
	public bool isHit = false;

	public Sounds_Holder Sounds_gore;
	public Sounds_Holder Sounds_normal;
	public Sounds_Holder Sounds_dying;
	

	public static List<Enemy_Controller> controllers = new List<Enemy_Controller>(20);

	void Awake(){
		controllers.Add(this);
	}
	void OnDestroy(){
		controllers.Remove(this);	
	}

	// Use this for initialization
	void Start () {

		SetArrow();

		animator = GetComponent<Animator>();

		InheritInit();

	}

	public void SetArrow(){

		float angle = Random.Range(-80.0f, 80.0f);
		
		killDir = BLINDED_Math.Rotate_Direction(Vector3.right, Vector3.forward, angle);
		
		deathArrow.eulerAngles = new Vector3(0,0, angle);

		deathArrow.GetComponent<SpriteRenderer>().color = Color.white;
		deathArrow.GetComponent<SpriteRenderer>().enabled = true;

	}

	public virtual void InheritInit(){ // for the kids

	}

	public virtual bool InputUserSlash(Vector2 inputDir){

		if(isHit)
			return true;

		float angle = Vector2.Angle(inputDir, killDir);

		if(angle <= killAngleDeathzone){
			isHit = true;
			deathArrow.GetComponent<SpriteRenderer>().color = Color.green;
		}else{
			deathArrow.GetComponent<SpriteRenderer>().color = Color.red;
		}

		return isHit;

	}

	/*
	public bool DidUserKillEnemy(){

		float angle = Vector2.Angle(userInputDir, killDir);

		if(angle <= killAngleDeathzone){
			animator.Play(State_die, 0, 0.0f);
			GetComponent<SpriteRenderer>().sortingOrder = -1;

			Sounds_dying.PlaySound();
			Sounds_gore.PlaySound();

			Destroy(gameObject, 1.0f);
			return true;
		}else{
			Game_Controller.LoseScenario(this);
			return false;
		}
	}
	*/

	/// <summary>
	/// called by Game_Controller
	/// </summary>
	/// <param name="killer">Killer.</param>

	public static void LoseScenario(Enemy_Controller killer){

		BasicEnemy_Controller basicGuy = null;

		foreach(Enemy_Controller wolf in controllers){
		
			if(wolf.GetType() == typeof(BasicEnemy_Controller)){

				basicGuy = (BasicEnemy_Controller) wolf;
				basicGuy.LoseScenario_Called(killer);
			}

		}

	}

}
