using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

	/*
	 * Controls the Game levels: 1, 2 and 3
	 */

	//The variables below indicates the prefabs for the enemies
	public GameObject watcher;
	public GameObject sorrow;
	public GameObject reaper;
	//Determines the interval to be sorted for generating a new enemy
	public int minSpawnTime;
	public int maxSpawnTime;

	//Variable used internally
	private bool gameOver; //Used to check if the player lost
	private bool endLevel; //Used to check if the level has finished
	private float SpawnTime; //Stores the time to spawn the enemy
	private GameObject Player; //Stores the player
	private bool isReaperOn; //Detects if there is a reaper on the game
	public GameObject LostSoul; //Stores a reference to the lost your soul screen
	private GameObject[] visibleWalls; //Storesthe walls and floor
	private GameObject Bars; //Reference to the bars
	private PlayerController playerController; //Used to instantiate the player controller

	// Use this for initialization
	void Start () {
		//First enemy spawn time is sorted
		SpawnTime = RandomEnemySpawn (minSpawnTime, maxSpawnTime);
		gameOver = false; //game is running
		endLevel = false; //level is running

		//Reference to player using tag
		Player = GameObject.FindGameObjectWithTag ("PlayerBody");

		//Reference to bars using tag
		Bars = GameObject.FindGameObjectWithTag ("Bars");

		//No enemies at the beginnig
		isReaperOn = false;

		//Player controller is instantiated
		GameObject playerControllerObject = GameObject.FindWithTag ("PlayerBody");
		if (playerControllerObject != null) {
			playerController = playerControllerObject.GetComponent<PlayerController> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!gameOver && !endLevel) { //Player hasn't lost nor finished the level
			int enemyNumber;

			//Do not generate multiple enemies - wait for the to die
			if (SpawnTime < 0 && GameObject.FindWithTag ("Enemy") == null) {

				//Sorts a number from 0 to 2 to decide wich enemy to spawn
				enemyNumber = RandomEnemySpawn (0, 3);

				//Switch the spawn enemy according to the sorted number
				switch (enemyNumber) {
				case 0: //case 0 is The Watcher
					Instantiate (watcher);
					break;
				case 1: //case 1 is The Sorrow
					Instantiate (sorrow);
					break;
				case 2: //case 2 is The Reaper
					//The Reaper replaces the level song
					GetComponent<AudioSource> ().Pause ();
					isReaperOn = true; //Used to detect the reaper and control the song
					Instantiate (reaper);
					break;
				}

				//Generates new timer for enemy
				SpawnTime = RandomEnemySpawn (minSpawnTime, maxSpawnTime);
			} else {
				if (GameObject.FindWithTag ("Enemy") == null) { //When the enemy dies
					if (isReaperOn) { //If it is The Reaper, the song comes back
						GetComponent<AudioSource> ().Play ();
						isReaperOn = false;
					}
					//Spawn time is decreased whenever there are no enemies on field
					SpawnTime -= Time.deltaTime;
				}
			}
		} else if (endLevel) { //Level finished
			if (Input.GetButtonDown ("Fire1")) { //B pressed goes to the next level
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			}
		} else if (gameOver) { //Player lost
			if(Input.GetButtonDown ("Fire1")) { //B pressed restart level
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
			}
			if(Input.GetButtonDown ("Fire2")) { //C pressed goes to main menu
				SceneManager.LoadScene (0);
			}
		}
	}

	//Called when player dies
	public void GameOver () {
		if (!endLevel) { //to not conflict with finishing the level
			gameOver = true;

			//Blocks player movement
			playerController.BlockMovement();

			//Inactivate bars
			Bars.SetActive (false);

			//Fade Game Over screen
			StartCoroutine(FadeTo(1.0f, 1.0f));

			//Places the Game Over screen in front of the player
			LostSoul.transform.position = Player.transform.position + Player.transform.forward * 12;

			//Deletes walls and floor
			visibleWalls = GameObject.FindGameObjectsWithTag ("Respawn");
			foreach (GameObject visible in visibleWalls) {
				visible.GetComponent<MeshRenderer> ().enabled = false;
			}

			//Destroy remaining enemy
			Destroy(GameObject.FindWithTag("Enemy"));

			//Pauses the level song
			GetComponent<AudioSource> ().Pause ();

			//Starts the Game Over sound
			LostSoul.GetComponent<AudioSource> ().Play ();
		}
	}

	//Called when level is finished
	public void EndLevel() {
		endLevel = true;
	}

	//Sorts a number between the minValue and maxValue
	private int RandomEnemySpawn(int minValue, int maxValue) {
		int time;
		time = Random.Range (minValue, maxValue);
		return time;
	}

	//Fades the screen by changing its alpha component
	IEnumerator FadeTo(float aValue, float aTime) {
		float alpha = LostSoul.transform.GetComponent<Renderer>().material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            
			Color newColor = new Color(240, 120, 0, Mathf.Lerp(alpha,aValue,t));
			LostSoul.transform.GetComponent<Renderer>().material.color = newColor;
			yield return null;
		}
	}
}
