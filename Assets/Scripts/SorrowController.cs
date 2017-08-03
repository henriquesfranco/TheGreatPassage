using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorrowController : MonoBehaviour {

	private Animator animator;
    private GameObject Player;



    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
        Player = GameObject.FindGameObjectWithTag("PlayerBody"); // Finds the player

		transform.position = Player.transform.position + Player.transform.forward * 3; // Always start in front of the player
    }

    // Update is called once per frame
    void Update() {

        // Vectors that keep the slow area limits
        Vector3 position_pos;
        Vector3 position_neg;

        position_pos = transform.position; //Grabs current position
        position_neg = transform.position;

        position_pos.x += 3; // Apply upper limits for x and z axis
        position_pos.z += 3;
        position_neg.x -= 3; // Apply lower limits for x and z axis
        position_neg.z -= 3;

        // If inside the limits box, slow the player
        if (Player.transform.position.x <= position_pos.x && Player.transform.position.x >= position_neg.x && Player.transform.position.z <= position_pos.z && Player.transform.position.z >= position_neg.z)
        {
            PlayerController.isSlowed = true;
        }
        else
        {
            PlayerController.isSlowed = false;
        }

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Dead")) {
			Destroy (gameObject); // Destroy the Sorrow based on its animation
            PlayerController.isSlowed = false; // Disable the slow being applied
		}
	}
}
