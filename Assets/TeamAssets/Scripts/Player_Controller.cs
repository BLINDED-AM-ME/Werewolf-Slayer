using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Controller : MonoBehaviour {
	public static Player_Controller instance;

	public enum PlayerState{

		Running,
		Attacking

	}

	public PlayerState state;

	public float dash_speed = 5.0f;

	Animator animator;

	public static Vector3 runningPosition;

	private List<Enemy_Controller> victims = new List<Enemy_Controller>(10);

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		runningPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {


		switch(state){
		case PlayerState.Running:

			if(victims.Count > 0)
				StartCoroutine(AttackSequence());

			break;
		case PlayerState.Attacking:

			break;
		}
	}

	public void Attack(Enemy_Controller victim){

		if(!victims.Contains(victim))
			victims.Add(victim);

	}

	private bool isAttackSequenceOnHold = false;
	IEnumerator AttackSequence()
	{

		state = PlayerState.Attacking;


		while(victims.Count > 0){

			// dash to victim
			yield return StartCoroutine(DashTime(victims[0].player_attack_position));
			// dash complete


			if(victims[0].isHit){ // you win

				// attack victim
				PlayAttack();

				BasicEnemy_Controller basic = (BasicEnemy_Controller) victims[0];
				basic.PlayDie();

				isAttackSequenceOnHold = true; // wait for animation to call "NextEventInAttackSequence_Please"
				while(isAttackSequenceOnHold){
					yield return null;
				}

				victims.RemoveAt(0);

			}else{ // you lose

				Game_Controller.LoseScenario(victims[0]);
				break;
			}

		}
		// no more victims or you messed up

		if(!Game_Controller.isGameOver){

			transform.localScale = new Vector3(-1, 1, 1);

			// dash back to running
			yield return StartCoroutine(DashTime(runningPosition));
			animator.SetTrigger("StopDash");

			yield return null; // next frame

			StartRunning();
			state = PlayerState.Running;
		
		}

	}

	IEnumerator DashTime(Transform target){

		animator.SetTrigger("Dash");

		while(transform.position != target.position){
			yield return new WaitForEndOfFrame();
			transform.position = Vector3.MoveTowards(transform.position, target.position, dash_speed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator DashTime(Vector3 point){

		animator.SetTrigger("Dash");

		while(transform.position != point){
			yield return new WaitForEndOfFrame();
			transform.position = Vector3.MoveTowards(transform.position, point, dash_speed * Time.deltaTime);
			yield return null;
		}
	}

	void NextEventInAttackSequence_Please(){ // called by animation
		isAttackSequenceOnHold = false;
	}

	

	private static int attackIterator = 0;
	public static void PlayAttack(){
		
		switch(attackIterator){
			
		case 0:
			instance.animator.SetTrigger("AttackHigh");
			break;
			
		case 1:
			instance.animator.SetTrigger("AttackMid");
			break;
			
		case 2:
			instance.animator.SetTrigger("AttackLow");
			break;
			
		}

		attackIterator = (attackIterator+1) % 3;
		
	}

	public static void PlayAttack(BasicEnemy_Controller.AttackType stance){

		switch(stance){

		case BasicEnemy_Controller.AttackType.high:
			instance.animator.SetTrigger("AttackHigh");
			break;

		case BasicEnemy_Controller.AttackType.mid:
			instance.animator.SetTrigger("AttackMid");
			break;

		case BasicEnemy_Controller.AttackType.low:
			instance.animator.SetTrigger("AttackLow");
			break;

		}

	}

	public static void PlayDeath(){

		instance.animator.SetTrigger("Die");

	}

	public static void StopRunning(){
		instance.transform.localScale = new Vector3(1, 1, 1);
		instance.animator.SetBool("isRunning", false);
	}
	
	public static void StartRunning(){
		instance.transform.localScale = new Vector3(1, 1, 1);
		instance.animator.SetBool("isRunning", true);
	}
	
	public static void LoseScenario(Enemy_Controller killer){
		
		instance.enabled = false;
		instance.StopAllCoroutines();

		instance.transform.position = killer.player_attack_position.position;

		instance.animator.SetTrigger("StopDash");

		StopRunning();
	}
	
}


