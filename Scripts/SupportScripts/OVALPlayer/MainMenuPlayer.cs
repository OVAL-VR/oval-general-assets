using UnityEngine;
using System.Collections;

/* Contains two short declarations to ensure that 
 * the MainMenuPlayer is not destroyed until the user has joined the Photon Network
 */
public class MainMenuPlayer : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Don't destroy this gameObject when a new scene is loaded
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// When this user joins a Photon Network Room
	void OnJoinedRoom() {
		// Destroy this gameObject across the Photon Network
		PhotonNetwork.Destroy(gameObject);
	}
}
