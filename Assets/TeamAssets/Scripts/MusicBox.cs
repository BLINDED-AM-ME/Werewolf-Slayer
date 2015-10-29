using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicBox : MonoBehaviour {
	public static MusicBox instance;

	public AudioClip[] music;
	public int track = 0;
	private AudioSource box;

	// Use this for initialization
	void Start () {

		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
			return;
		}

		box = GetComponent<AudioSource>();

		box.clip = music[track];
		box.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!box.isPlaying){

			track = (track+1) % music.Length;

			box.clip = music[track];
			box.Play();
		}

	}


}
