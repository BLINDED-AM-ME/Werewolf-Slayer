using UnityEngine;
using System.Collections;

public class MiniBoss_Controller : Enemy_Controller {

	public delegate void Callback();
	public Callback onDefeatCall;

	public int healthCount = 3;
	public float quickTime_limit = 1.0f;
	public float quickTime_delays = 0.5f;
	public float slashFury_time = 1.0f;

	public bool isFurySlashing = false;
	public int  furySlashCount = 0;

	public override void InheritInit ()
	{
		base.InheritInit ();

		deathArrow.GetComponent<SpriteRenderer>().enabled = false;

		Game_Controller.StopScrollingScenery();
		Player_Controller.StopRunning();

		StartCoroutine(AttackCycle(4.0f));

	}

	public override bool InputUserSlash (Vector2 inputDir)
	{
		if(base.InputUserSlash (inputDir)){

			deathArrow.GetComponent<SpriteRenderer>().color = Color.green;
			if(!isFurySlashing){
				GetComponent<BoxCollider2D>().enabled = false;
			}else{
				Sounds_gore.PlaySound();
				furySlashCount++;
			}
			
			return true;
		}else{
			return false;
		}
	}

	public IEnumerator AttackCycle(float delay){

		// wait till you get there
		yield return new WaitForSeconds(delay);


		// attack loop
		for(int i=0; i<healthCount; i++){
			yield return StartCoroutine(QuicktimeEventStart(quickTime_limit));
			yield return new WaitForSeconds(quickTime_delays);
		}

		// slash fury
		PromptText.AddMessage("Slash him up", slashFury_time);
		isFurySlashing = true;
		isHit = true;
		GetComponent<BoxCollider2D>().enabled = true;
		yield return new WaitForSeconds(slashFury_time);
		GetComponent<BoxCollider2D>().enabled = false;
		isFurySlashing = false;

		animator.SetTrigger("die");
		Sounds_dying.PlaySound();

		yield return new WaitForSeconds(1.0f);

		Game_Controller.StartScrollingScenery();
		Player_Controller.StartRunning();

		onDefeatCall();

		Destroy(gameObject);
	}

	private bool isHoldingForAttack = false;
	public IEnumerator QuicktimeEventStart(float timeLimit){

		GetComponent<BoxCollider2D>().enabled = true;
		SetArrow();

		yield return new WaitForSeconds(timeLimit);

		GetComponent<BoxCollider2D>().enabled = false;
		deathArrow.GetComponent<SpriteRenderer>().enabled = false;

		animator.SetTrigger("attack");

		Sounds_normal.PlaySound();

		isHoldingForAttack = true;
		while(isHoldingForAttack)
			yield return null;

	}

	public void QuicktimeEventEnd(){ // called by animation

		Sounds_gore.PlaySound();

		if(isHit){ // you are still good
			isHoldingForAttack = false;
			isHit = false;

			Player_Controller.PlayAttack();

		}else{ // you are done
			StopAllCoroutines();
			Game_Controller.LoseScenario(this);
			Player_Controller.PlayDeath();
		}

	}

}
