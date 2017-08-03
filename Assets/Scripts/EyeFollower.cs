
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollower : MonoBehaviour {

	/*
	 * Script to control The Watcher
	 */

	//Variables to control difficulty
	public float speed;
	public float damageTime;

	Animator animator; //Used for the animator component
	private GameObject Player; //Used for the player
	private float damageTimeStart; //Used to store the damage time value

	// Use this for initialization
	void Start () {
		//Start by getting the animator component
		animator = GetComponent<Animator> ();

		//Stores the damagetime start value
		damageTimeStart = damageTime;

		//Looks for the player
		Player = GameObject.FindGameObjectWithTag ("PlayerBody");

		//Start its position in front of the player
		transform.position = Player.transform.position + Player.transform.forward * 3 + new Vector3(0.0f, 6.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

		//Get variables from the animator
		float distance = animator.GetFloat ("distance");
		float showTime = animator.GetFloat ("showTime");
		bool attackTime = animator.GetBool ("attackTime");

		float step = speed * Time.deltaTime; //step used to move toward the Player

		Vector3 followingPosition = Player.transform.position + Player.transform.forward * 3 + new Vector3(0.0f, 4.0f, 0.0f);

		/*
		 * Moving the Eye towards the Player
		 */

		//Checks if the distance between the player and the eye are bigger than the distance established on start
		//If so, gets closer at a rate determined by the variable step
		if (distance > 2) {
			transform.position = Vector3.MoveTowards (transform.position, followingPosition, step);
			attackTime = false;
			damageTime = damageTimeStart;
		} else { //close enough to attack
			attackTime = true;

			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) damageTime -= Time.deltaTime;

			if (damageTime <= 0) {
                Player.SendMessage("DropHP", 40f); //attacks drops just an ammount of the players's total HP
				damageTime = damageTimeStart; //Attack time is restarted
			}
		}

		//Rotates towards player to always face his direction
		Vector3 playerDir = Player.transform.position + new Vector3(0.0f, 3.0f, 0.0f) - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, -playerDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation (newDir);

		//Deletes the eye if showTime has ended, and therefore, is on Dead State
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Dead")) {
			Destroy (gameObject);
		}

		//Update the state machine variables
		animator.SetBool  ("attackTime", attackTime);
		animator.SetFloat ("distance", Vector3.Distance (followingPosition, transform.position));
		animator.SetFloat ("showTime", showTime - Time.deltaTime);
	}
}
