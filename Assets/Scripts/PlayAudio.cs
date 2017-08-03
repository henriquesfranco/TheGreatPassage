using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {
	/*
	 * Used for Animation Events using more than one audio clips
	 */

	public AudioClip audio1;
	public AudioClip audio2;

	private AudioSource localAudio;

	void Start() {
		//The component should have an Audio Source, even it its audio clip is empty
		localAudio = GetComponent<AudioSource> ();
	}

	//This script has 2 audio clips attached
	//They can be loaded bu the int parameter
	//Used inside animation Events
	void playAudio(int audioNumber){
		if (audioNumber == 1) {
			localAudio.PlayOneShot (audio1, 20.0f);
 		}
		else {
			localAudio.PlayOneShot (audio2, 8.0f);
		}
	}
}