using UnityEngine;
using System.Collections;

//This script takes input from the Thrustmaster joystick to translate and rotate the OVALPlayer it is attached to.
public class JoystickMovement : MonoBehaviour {

	//Speed at which the OVALplayer moves
	public float speed = 10.0F;
	//Speed at which the OVALplayer rotates
	public float rotationSpeed = 10.0F;
	//The center eye anchor of the camera rig (for finding movement direction)
	public Transform centerEyeAnchor = null;
	//The forward vector of the OVALplayer, restriced to the xz plane
	private Vector3 flatForward;
	//The right vector of the OVALplayer, restricted to the xz plane
	private Vector3 flatRight;


	void Start()
	{

	}

	//Updates once per frame
	void Update()
	{
		flatForward = new Vector3(centerEyeAnchor.forward.x, 0, centerEyeAnchor.forward.z);
		flatRight = new Vector3 (centerEyeAnchor.right.x, 0, centerEyeAnchor.right.z);

		//If Fire2 is held down, gameobject spins, rises, or lowers based on joystick input.
		if (Input.GetButton ("Fire2")) {

			float yRotation = Input.GetAxis ("Horizontal") * rotationSpeed;
			transform.Rotate(0, yRotation, 0, Space.Self);
			float yTranslation = Input.GetAxis ("Vertical") * speed;
			yTranslation *= Time.deltaTime;
			transform.Translate (0, yTranslation, 0);

		//If trigger is not held down, gameobject moves forwards, backwards, left, and right based on joystick input.
		} else {

			float xTranslation = Input.GetAxis ("Horizontal") * speed;
			float zTranslation = Input.GetAxis ("Vertical") * speed;
			xTranslation *= Time.deltaTime;
			zTranslation *= Time.deltaTime;
			transform.Translate (flatForward * zTranslation, Space.World);
			transform.Translate (flatRight * xTranslation, Space.World);

		}

	}
}

//Default input names for Thrustmaster as specified by Unity:
//Trigger- Fire1
//Black and Yellow- Fire2
//Side Black- Fire3
//Front Black - Jump
//Forward & Back - Vertical
//Left & Right - Horizontal