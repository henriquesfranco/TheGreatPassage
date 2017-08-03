using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {

	public float changeScreenTimedef; //Waiting time to change to the next screen for reuse

	private float changeScreenTime; //Waiting time to change to the next screen for reuse
	private int imageToLoad;

	// Use this for initialization
	void Start () {
		changeScreenTime = changeScreenTimedef;

		//Images are numbered in order to be loaded easily
		imageToLoad = 1; //First imagem to load
	}

	// Update is called once per frame
	void Update () {
		changeScreenTime -= Time.deltaTime;

		//The image to load should not be higher than 3, because there are only three images to be loaded
		if (changeScreenTime < 0 && imageToLoad < 4) {
			changeScreenTime = changeScreenTimedef;

			//To load the image, the mainTexture to the Renderer is loaded from the resources file
			GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Instructions/" + imageToLoad.ToString ()) as Texture2D;

			imageToLoad++; //Imagem to be loaded is incremented
		} else if (imageToLoad == 4) {
				//Ending the images, the level 1 is loaded
				SceneManager.LoadScene (3);
		}
	}
}
