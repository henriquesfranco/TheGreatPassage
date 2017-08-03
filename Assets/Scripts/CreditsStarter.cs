using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsStarter : MonoBehaviour {
	/*
	 * This Script's only purpose is to reuse the Credits from level 4 by setting the startCreditsImages variable
	 * The variable is not set at the Start because the sequential behaviour of execution conflicts with the start
	 * of another script setting the same variable
	 */

	private ScreenCreditsController screenCreditsController; //Used to instantiate a credits controller
	private bool started; //Used to avoid setting the variable multiple times during the credits

	// Use this for initialization
	void Start () {
		//Looks for the Credits controller to instantiate
		GameObject ScreenCreditsControllerObject = GameObject.FindWithTag ("CreditsController");
		if (ScreenCreditsControllerObject != null) {
			screenCreditsController = ScreenCreditsControllerObject.GetComponent<ScreenCreditsController> ();
		}
		//Starts as false because it should be run on first Update
		started = false;
	}

	void Update() {
		if (!started) { //Only executed once
			started = true;
			//Uses the instance of screenCreditsController to call a method to set a starter boolean
			screenCreditsController.StartCreditsImages ();
		}
	}
}
