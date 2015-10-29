using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Sounds_Holder : MonoBehaviour {

	public string soundsTag = "other";
	public AudioClip[] sounds;
	private AudioSource box;


	void Start(){

		box = GetComponent<AudioSource>();

	}

	public void PlaySound(){
		box.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
	}

	public void PlaySound(float delay){
		StartCoroutine(PlayWait(sounds[Random.Range(0, sounds.Length)], delay));
	}

	public void PlaySound(int index){
		box.PlayOneShot(sounds[index]);
	}

	public void PlaySound(int index, float delay){
		StartCoroutine(PlayWait(sounds[index], delay));
	}


	private IEnumerator PlayWait(AudioClip clip, float delay){
		
		yield return new WaitForSeconds(delay);
		
		box.PlayOneShot(clip);
		
	}
}
