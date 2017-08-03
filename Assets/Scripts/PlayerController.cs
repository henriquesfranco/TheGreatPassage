using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed; //Controls the player's speed
    public static float run; //Multiplicative factor for improving player speed
    public static bool isSlowed = false; //Boolean for controlling the effect of the Sorrow Enemy, which blocks running feature and slows player
    public GameObject Camera; //Object that points to the GVR Camera - used for copying rotation to the playerBody

	private bool blockMovement; //Freezes the body, not the camera rotation

    // Use this for initialization
    void Start() {
        run = 1.0f; //Starts with normal speed
		blockMovement = false; //Movements allowed at the beginning
    }

    // Update is called once per frame
    void LateUpdate() {
        // Checks the slow first so the player can't run if slowed
        if (isSlowed) {
            run = 0.5f;
            SendMessage("RecoverStamina"); //If slowed, the stamin should not drop, therefore is being recovered
        } else {
			if (Input.GetButtonDown("Fire2") || Input.GetButton("Fire2")) //Button C on keypad is pressed, player runs
            {
                run = 3.0f; //Running is 3 times faster than walking
                SendMessage("DropStamina"); //Stamina should drop gradually
            }
			else if (Input.GetButtonUp("Fire2")) //Player is exiting running mode (button being released) - normal speed applied
            {
                run = 1.0f; //normal speed
                SendMessage("RecoverStamina"); //Stamina should be recovered gradually
            }
            else //Else state is simply walking. Stamina should also be recovered in this state
            {
                run = 1f; 
                SendMessage("RecoverStamina");
            }
        }

		//The two lines below gets the movement from the player's analog controller
        float horizontal = Input.GetAxis("Horizontal") * speed * run * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * run * Time.deltaTime;

		//Rotation o the player body should follow the rotation of the GVR Camera, so the movement is affected by the camera
		//This gives an imersive sensation of movement
        transform.rotation = Camera.transform.rotation;

		//Before applying the movement to the player, should check if it is allowed to move with the variable blockMovement
		if (!blockMovement) {
			transform.localPosition += transform.right * horizontal;
			transform.localPosition += transform.forward * vertical;
		}
    }

	//The blockMovement boolean can be set by anywhere in the project, depending on game controller or enemies's functions
	public void BlockMovement() {
		blockMovement = true;
	}

}
