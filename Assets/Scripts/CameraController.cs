using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player; //Represents the playerBody, used for positioning the camera

	private Vector3 offsetHeight; //Sets an offest on the camera Height in order to give the player's proper height

	void Start()
	{
		offsetHeight = new Vector3(0.0f, 3, 0.0f); //Starts by setting the offset
	}

	// Update is called once per frame
	void LateUpdate () {
		//On each frame, updates the camera position with the player position
		transform.position = player.transform.position + offsetHeight;
	}
}
