using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CreditsController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) { //Go back to MainMenu
			//Load level 1 to start game
			SceneManager.LoadScene(0);
		} 
	}
}
