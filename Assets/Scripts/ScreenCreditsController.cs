using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenCreditsController : MonoBehaviour {

	/*
	 * Controls the image flux on the Credits
	 * And also its positioning on level 4
	 */

	public Transform player; //Identifies the player position. Used to position the credits screen
	public float changeScreenTimedef; //Default time to change to the next screen

	private bool startCredits; //Boolean variable for executing routine of positioning credits
	private bool startCreditsImages; //Boolean variable for starting screen changing
	private float changeScreenTime; //Time to change to the next screen
	private int imageToLoad; //image to be loaded from file

	// Use this for initialization
	void Start () {
		startCredits = false;
		startCreditsImages = false;
		changeScreenTime = changeScreenTimedef;

		//Starts from image 2 because 1 is already on the GameObject
		imageToLoad = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (startCredits) { //Places the screen in front of the player's current position
			transform.position = player.position + new Vector3(0.0f, 0.0f, 35.0f);
		}

		if (startCreditsImages) { //Controls the changing of images
			changeScreenTime -= Time.deltaTime;
			if (changeScreenTime < 0 && imageToLoad < 16) { //Stops when reaching the last image (15)
				changeScreenTime = changeScreenTimedef;

				//Replaces the mainTexture on the renderer
				GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Credits/" + imageToLoad.ToString ()) as Texture2D;

				//Goes to the next image
				imageToLoad++;
			} else if (imageToLoad == 16) { //Waits for B keypress to end Credits and goes back to the main menu
				if (Input.GetButtonDown ("Fire1")) {
					SceneManager.LoadScene (0);
				}
			}
		}
	}

	public void StartCredits(){
		startCredits = true;
	}

	public void StartCreditsImages() {
		startCreditsImages = true;
		startCredits = false;
	}
}
