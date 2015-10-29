using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PromptText : MonoBehaviour {

	public static List<string> messages = new List<string>();
	public static List<float>  lengths = new List<float>();
	public static Text comp;

	bool isShowing = false;

	// Use this for initialization
	void Start () {
	
		comp = GetComponent<Text>();
		messages.Clear();

	}

	void Update(){

		if(!isShowing && messages.Count > 0){
			StartCoroutine(ShowMessages());
		}
	}

	public IEnumerator ShowMessages(){

		isShowing = true;
		comp.enabled = true;

		float length = 0.0f;
		while(messages.Count > 0){

			comp.text = messages[0];
			length = lengths[0];

			messages.RemoveAt(0);
			lengths.RemoveAt(0);

			yield return new WaitForSeconds(length);
		}

		comp.enabled = false;
		isShowing = false;
	}

	public static void AddMessage(string message, float length){

		messages.Add(message);
		lengths.Add(length);

	}
}
