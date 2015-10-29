using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class Menu_Controller : MonoBehaviour {

	public Slider timeScaleSlider;
	public Slider spawnDelaySlider;

	// Use this for initialization
	void Start () {
	
		timeScaleSlider.value = TimeStuff;
		spawnDelaySlider.value = SpawnStuff;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float TimeStuff{
		
		get{
			return Time.timeScale;
		}
		
		set{
			Time.timeScale = value;
		}
	}

	public float SpawnStuff{
		
		get{
			return Game_Controller.instance.GetComponent<WaveSpawner>().spawn_delay;
		}
		
		set{
			Game_Controller.instance.GetComponent<WaveSpawner>().spawn_delay = value;
		}
	}

	/*
	public float MusicVolume{
		
		get{
			
			float result = 0.0f;
			if(masterSoundMixer.GetFloat("MusicVolume", out result))
				return result;
			else
				return 0.0f;
		}
		
		set{
			masterSoundMixer.SetFloat("MusicVolume", value);
		}
	}
	
	public float SfxVolume{
		
		get{
			
			float result = 0.0f;
			if(masterSoundMixer.GetFloat("SfxVolume", out result))
				return result;
			else
				return 0.0f;
		}
		
		set{
			masterSoundMixer.SetFloat("SfxVolume", value);
		}
	}
	*/
}
