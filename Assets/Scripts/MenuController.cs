using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour {

	/*
	 * Menu Controller
	 */

	// Update is called once per frame
	void Update () {
		//There are only two options on the Menu
		//C for Credits
		//B for next level
		if (Input.GetButtonDown ("Fire2")) {
			//Load level 1 to start game
			SceneManager.LoadScene(1);
		} else if (Input.GetButtonDown ("Fire1")) {
			//Load credits
			SceneManager.LoadScene(2);
		}
	}
}
