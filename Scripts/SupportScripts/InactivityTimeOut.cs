using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

//Exits the application if the attached object has not moved in a certain amount of time
public class InactivityTimeOut : Photon.MonoBehaviour {

	//The maximum time of inactivity in seconds
	public float secondsToTimeout = 120f;
	//The point until timeout when a warning is placed on screen
	public float secondsOfWarning = 60f;
	//The warning message to be displayed
	public GameObject warning;
	//Variable keeping track of the timer
	float timeElapsed;
	//Has the countdown timeElapsed been started?
	bool timerStarted = false;
	//The starting position of the player (for the sake of the current countdown)
	Vector3 referencePosition;


	// Use this for initialization
	void Start () {
		referencePosition = transform.position;
		warning.SetActive(false);
	}
	

	// Update is called once per frame
	void Update () {

		//If object isn't moving and timeElapsed hasn't started
		if (transform.position == referencePosition && !timerStarted) {
			timerStarted = true;
		}
		//If object isn't moving and timeElapsed is going
		else if (transform.position == referencePosition && timerStarted) {
			//Increment timer
			timeElapsed += Time.deltaTime;
			//If timeElapsed is getting close to timeout
			if(secondsToTimeout - timeElapsed <= secondsOfWarning) {
				//Print warning message
				warning.SetActive(true);
				warning.GetComponentInChildren<Text>().text = "Still there?\nOVAL will close after\n" + Mathf.Round(secondsToTimeout - timeElapsed) + "\nseconds of no movement.";
			}
			if(secondsToTimeout - timeElapsed <= 0) {
				//Disconnect from Photon
				PhotonNetwork.Disconnect();
			}
		}
		//If object is moving and timeElapsed is going
		else if (transform.position != referencePosition && timerStarted) {
			timerStarted = false;
			timeElapsed = 0f;
			referencePosition = transform.position;
			warning.SetActive(false);
		}
		//If object is moving and timeElapsed isn't going
		else if (transform.position != referencePosition && !timerStarted) {
			referencePosition = transform.position;
		}

	}

	void OnDisconnectedFromPhoton() {
		//Return to Main Menu
		SceneManager.LoadScene(0); 
	}
}
