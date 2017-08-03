using UnityEngine;
using System.Collections;

public class DevilScript : MonoBehaviour {

	/*
	 * Used to control The Reaper
	 */

	//Public parameters used to set difficulty
	public float referenceDistance; //distance to attack
	public float speed; //The Reaper's speed
	public float attackTime; //Time to perform the ultimate attack

	Animator animator; //Animator containing the state machine
	private Transform player; //player object. Used to get the distance
	private float attackTimeStart; //Stores the attack time for reuse
	private float showTimeStart; //Stores the show time for reuse

	void Start () {
		//Start by getting the animator component
		animator = GetComponent<Animator> ();

		//Sets the default variables for reuse
		attackTimeStart = attackTime;
		showTimeStart = animator.GetFloat ("showTime");

		//Gets the player's transform in order to follow him
		player = GameObject.FindGameObjectWithTag ("PlayerBody").transform;

		//Starts in front of the player
		transform.position = player.position + player.forward * 3 + new Vector3(0.0f, 6.0f, 0.0f);
	}

	void Update () {
		//Get variables from the animator
		float distance = animator.GetFloat ("distance");
		float showTime = animator.GetFloat ("showTime");

		//Calculates the movement step
		float step = speed * Time.deltaTime;

		if (distance > referenceDistance) { //if the distance is bigger than the reference, walks towards the players
			transform.position = Vector3.MoveTowards(transform.position, player.position + new Vector3(0.0f, 3.0f, 0.0f), step);
			attackTime = attackTimeStart;
		} else if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && !animator.GetCurrentAnimatorStateInfo(0).IsName("DISAPPEAR")){ //Else, attacks
			animator.SetFloat ("showTime", showTimeStart);
			showTime = showTimeStart;
			if (attackTime <= 0) {
                player.SendMessage("DropHP", 91f); //attacking includes dropping HP completely
			}
			attackTime = attackTime - Time.deltaTime;
		}

		//Rotates towards player to always face his direction
		Vector3 playerDir = player.position - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, playerDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation (newDir);

		//Verifies if time has ended
		if (showTime <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) {
			Destroy (gameObject); //Finishing its time means it should be destroyed
		}
			
		showTime -= Time.deltaTime;

		//Updates the variables
		animator.SetFloat ("distance", Vector3.Distance (player.position, transform.position));
		animator.SetFloat ("showTime", showTime - Time.deltaTime);
	}
}
