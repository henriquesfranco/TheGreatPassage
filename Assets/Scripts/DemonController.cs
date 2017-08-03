using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class DemonController : MonoBehaviour {


	public GameObject Room;
	public Transform endPoint;
	public Light doorLight;
	public Light floorLight;
	public GameObject subtitles;

	private PlayerController playerController;
	private ScreenCreditsController screenCreditsController;
	private Animator animator;
	private Transform player;
	private float speechTime;
	private bool subtitlesStarted = false;
	private bool fadeOutStarted = false;
	private bool goingToHeavenStarted = false;
	private float endSpeechTime;
	private GameObject[] objectsToRemove;

	// Use this for initialization
	void Start () {
		//Start by getting the animator component
		animator = GetComponent<Animator> ();

		//Finds the player's position
		player = GameObject.FindGameObjectWithTag ("PlayerBody").transform;

		//Sets the speech time according to audio
		speechTime = 19.5f;
		//The second part of the speech to control automatic movement
		endSpeechTime = 5;

		//Instantiates the player controller to use its methods
		GameObject playerControllerObject = GameObject.FindWithTag ("PlayerBody");
		if (playerControllerObject != null) {
			playerController = playerControllerObject.GetComponent<PlayerController> ();
		}

		//Instantiates the credits controller to start the image positioning and exhibition
		GameObject ScreenCreditsControllerObject = GameObject.FindWithTag ("CreditsController");
		if (ScreenCreditsControllerObject != null) {
			screenCreditsController = ScreenCreditsControllerObject.GetComponent<ScreenCreditsController> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Get variables from the animator
		float distance = animator.GetFloat ("distance");
		bool speakFinished = animator.GetBool ("SpeakFinished");

		float step = Time.deltaTime * 5;

		//Rotates towards player to always face his direction
		Vector3 playerDir = player.transform.position + new Vector3(0.0f, 3.0f, 0.0f) - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, playerDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation (newDir);

		//In case the player is close enough to trigger the speech, blocks its movements
		if(distance < 15.0f) playerController.BlockMovement ();

		//checks for the state in order to know when to point to the door
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Waiting")) {
			//The speech time is being counted here
			speechTime -= Time.deltaTime;

			//When the speech starts, bars are no longer necessary, since the player cannot be hit
			//Nor move and use Stamina
			//Therefore the bars's images are disabled
			GameObject[] bars = GameObject.FindGameObjectsWithTag ("deleteBar");
			foreach (GameObject bar in bars) {
				bar.GetComponent<Image> ().enabled = false;
			}

			//When starting the speech, the Morte et Dabo song is faded out using a coroutine
			if (!fadeOutStarted) StartCoroutine(FadeOutSound(Room.GetComponent<AudioSource> (), 4.0f));

			//The subtitles are also started with the speech using a coroutine
			if (!subtitlesStarted) StartCoroutine (RunSubtitles());

			//The speakFinished variable comes from the animator
			//The animator should be informed when to point to the door and continues the ending speech
			if (speechTime < 0) {
				speakFinished = true;
			}
		}

		//On the ending speech, marked by the state HoldDoor in the animator
		//Starts moving the player
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("HoldDoor")) {
			if (!goingToHeavenStarted) {
				endSpeechTime -= Time.deltaTime;

				if (endSpeechTime < 0.0f) {
					//To move the player, the coroutine GoingToHeaven is triggered
					StartCoroutine (GoingToHeaven());
				}
			}
		}

		//Update variables from the animator
		animator.SetFloat ("distance", Vector3.Distance (player.position, transform.position));
		animator.SetBool ("SpeakFinished", speakFinished);
	}

	//Coroutine to fade the sound
	//Avoids abrupts changes
	IEnumerator FadeOutSound(AudioSource audiosource, float fadeTime) {
		fadeOutStarted = true;
		float startVolume = audiosource.volume;

		//The volume is gradually reduced until reaching the zero value
		while (audiosource.volume > 0) {
			audiosource.volume -= startVolume * Time.deltaTime / fadeTime;
			yield return null;
		}

		//At the end, the audio is completely stopped
		audiosource.Stop ();
	}

	//Moves the player automatically to the door
	IEnumerator GoingToHeaven() {
		goingToHeavenStarted = true;

		//endPoint is right in front of the door.
		Vector3 followingPos = endPoint.transform.position;

		float step = Time.deltaTime * 1.5f;

		//The distance is set to 2.0f because of the height of endpoint.
		//If the value is too close to 0, it may cause flickering movement
		while (Vector3.Distance(player.transform.position, followingPos) > 2.0f) {
			player.transform.position = Vector3.MoveTowards (player.transform.position, followingPos, step);
			step = Time.deltaTime;
			yield return null;
		}

		//Reached the endpoint, the lights are faded in by two coroutines
		//One for each light in the environment
		StartCoroutine(FadeHeavenLight(doorLight));
		StartCoroutine(FadeHeavenLight(floorLight));
	}

	//The light is faded in by the coroutine below
	IEnumerator FadeHeavenLight(Light heavenLight) {
		//It increases the light intensity gradually until it reaches 5.0f
		while (heavenLight.intensity < 5.0f) {
			heavenLight.intensity += Time.deltaTime;
			yield return null;
		}

		//Reaching the maximum valyem the lights are faded out
		//And the credits started
		StartCoroutine (FadeOutHeavenLight (doorLight));
		StartCoroutine (FadeOutHeavenLight (floorLight));
		screenCreditsController.StartCredits ();
	}

	//Similar to fade in, but decreases the intensity until 0
	IEnumerator FadeOutHeavenLight(Light heavenLight) {

		//Also, removes the walls and azazel, giving place only for the credits screen
		objectsToRemove = GameObject.FindGameObjectsWithTag ("Respawn");
		foreach (GameObject visible in objectsToRemove) {
			visible.GetComponent<MeshRenderer> ().enabled = false;
		}

		//Removing Azazel
		GameObject.FindGameObjectWithTag ("Demon").GetComponent<SkinnedMeshRenderer> ().enabled = false;

		//Decreases light intensity
		while (heavenLight.intensity > 0.0f) {
			heavenLight.intensity -= Time.deltaTime;
			yield return null;
		}

		//Uses instance of credits controller to start credits images
		screenCreditsController.StartCreditsImages ();
	}

	//Controls the exhibition of subtitles
	IEnumerator RunSubtitles() {
		//Used to avoid being executed more than once
		subtitlesStarted = true;

		//String containing each line and time to show each subtitle line
		string[] content = "1;-3;Hello Lazarus-6;You have crossed the great passage : despite its dangers-2.7;I am really impressed-7.5;I know about your intentions : I know you did all of this for Beatrice-7;Beyond this door : I do not hold any rights over you soul anymore-9.5;But someday : if you desire to come back : keep in mind that I will have great opportunities for you-2;HAHAHAHA-3;Remember this.".Split('-');

		//Reads the lines and splits the time to show (index 0) and text (index 1)
		foreach(string line in content) {
			if(line != null) {
				string[] textLine = line.Split(';');

				subtitles.GetComponent<Text>().text = textLine[1].Replace(":", "\n");

				//The time to show is implemented by returning a waiting time on the yield
				yield return new WaitForSeconds(float.Parse(textLine[0]));
			}
		}

		//Finishing the subtitles, they should be set to "" in order to disappear
		subtitles.GetComponent<Text> ().text = "";
	}
}