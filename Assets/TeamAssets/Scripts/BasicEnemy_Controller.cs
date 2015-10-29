using UnityEngine;
using System.Collections;

public class BasicEnemy_Controller : Enemy_Controller {


	public enum AttackType{
		high,
		mid,
		low
	}
	
	public AttackType attackType = AttackType.mid;
	
	static int State_die = Animator.StringToHash("Die");
	static int State_kill = Animator.StringToHash("Kill");
	static int State_HighAttack = Animator.StringToHash("HighAttack");
	static int State_WalkAttack = Animator.StringToHash("WalkAttack");
	static int State_LowAttack  = Animator.StringToHash("LowAttack");

	public override void InheritInit ()
	{
		base.InheritInit ();

		if(attackType == AttackType.high){
			animator.Play(State_HighAttack, 0, 0.0f);
		}
		else if(attackType == AttackType.mid){
			animator.Play(State_WalkAttack, 0, 0.0f);
		}
		else{
			animator.Play(State_LowAttack, 0, 0.0f);
		}

	}

	public override bool InputUserSlash (Vector2 inputDir)
	{
		if(base.InputUserSlash (inputDir)){

			Destroy(GetComponent<BoxCollider2D>());

			return true;
		}else{
			return false;
		}
	}

	public void ReachedPlayer(){ // called by animation

		if(isHit){
			
			Player_Controller.PlayAttack(attackType);
			
			PlayDie();
			
		}else{
			Game_Controller.LoseScenario(this);
		}
		
	}

	void TellPlayerToDie(){ // called by animation

		Player_Controller.PlayDeath();
	}

	public void LoseScenario_Called(Enemy_Controller killer){

	    deathArrow.gameObject.SetActive(false);
		
		if(killer == this){
			animator.Play(State_kill, 0, 0.0f);
			Sounds_normal.PlaySound();
			Sounds_gore.PlaySound(0.5f);
			
		}else{
			GetComponent<Animator>().enabled = false;
		}

	}

	public void PlayDie(){
	
		animator.Play(State_die, 0, 0.0f);
		GetComponent<SpriteRenderer>().sortingOrder = -1;
		
		Sounds_dying.PlaySound();
		Sounds_gore.PlaySound();
		
		Destroy(gameObject, 1.0f);
	}
}
