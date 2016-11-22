using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitToMenu : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Exit() {
		PhotonNetwork.Disconnect();
	}

	void OnDisconnectedFromPhoton() {
		//Return to Main Menu
		SceneManager.LoadScene(0); 
	}
}
