using System.Collections;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour {

	/*
	 * Controls the actions when ending the level
	 */

	public GameObject screenToFade; //Screen with the ending message

	private GameController gameController; //To instance game controller
	private PlayerController playerController; //To instance player controller
	private GameObject[] visibleWalls; //Groups the walls to iteratively delete them

	//Start only instantiate objects
	void Start() {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}

		GameObject playerControllerObject = GameObject.FindWithTag ("PlayerBody");
		if (playerControllerObject != null) {
			playerController = playerControllerObject.GetComponent<PlayerController> ();
		}
	}

	//If the player walks through the quad representing the end of the level, than the level is finished
	void OnTriggerEnter(Collider other) {
		if (other.tag == "PlayerBody") {//The Player tag checks for the player and no other object
			//Start Coroutine to fade screen
			StartCoroutine(FadeTo(1.0f, 1.0f));

			//Kill enemies so they don't kill you while waiting for next level to be selected
			Destroy(GameObject.FindWithTag("Enemy"));

			//Set flag on GameController to end level
			gameController.EndLevel();

			//Freezes player movement
			playerController.BlockMovement();

			//Delete walls and floor by identifying their Tag and disabling renderer
			visibleWalls = GameObject.FindGameObjectsWithTag ("Respawn");
			foreach (GameObject visible in visibleWalls) {
				visible.GetComponent<MeshRenderer> ().enabled = false;
			}
		}
	}

	//The fadeto coroutine increases the alpha value of the color component removing
	//transparency until the sreen is completely shown
	IEnumerator FadeTo(float aValue, float aTime) {
		float alpha = screenToFade.transform.GetComponent<Renderer>().material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
			Color newColor = new Color(240, 120, 0, Mathf.Lerp(alpha,aValue,t));
			screenToFade.transform.GetComponent<Renderer>().material.color = newColor;
			yield return null;
		}
	}
}
