using UnityEngine;
using System.Collections;

public class OVALPlayer : Photon.MonoBehaviour {

	public GameObject professorMenu;
	public GameObject studentMenu;
	public GameObject[] objectsToActivate;
	public MonoBehaviour[] componentsToActivate;


	// Use this for initialization
	void Start () {
		if(photonView.isMine) {
			foreach(GameObject go in objectsToActivate) {
				go.SetActive(true);
			}
			foreach(MonoBehaviour mb in componentsToActivate) {
				mb.enabled = true;
			}

			if(PhotonNetwork.isMasterClient) {
				professorMenu.SetActive(true);
			}
			else {
				studentMenu.SetActive(true);
			}
		}
	}

	// Callled when the Photon master client changes for this room
	void OnMasterClientSwitched() {
		if(photonView.isMine && PhotonNetwork.isMasterClient) {
			studentMenu.SetActive(false);
			professorMenu.SetActive(true);
		}
	}
}
