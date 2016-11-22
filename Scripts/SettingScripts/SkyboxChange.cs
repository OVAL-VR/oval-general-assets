using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/*This script contains methods to change the skybox of a scene
 *Users can choose from a predefined list of skyboxes
 *This script is to be activated by a UI Button
 */
public class SkyboxChange : Photon.MonoBehaviour {

	//An array of skyboxes
	public Material[] skyboxes;
	//The index of the current skybox in skyboxes
	int currentIndex = -1;
	//Constant declaration for Photon's Custom Properties to store skybox data
	const string SKYBOX_INDEX = "SBI";
	//Constant declaration for skybox change photon event code
	const byte SKYBOX_EVENT_CODE = 002;


	// Use this for initialization
	void Start () {
		//Setup Photon Event Callback
		PhotonNetwork.OnEventCall += this.OnEvent;
	}

	//Once the player has joined a Photon Room
	public void OnJoinedRoom()
	{
		//Import inital skybox
		OnEvent(SKYBOX_EVENT_CODE, null, 0);
	}

	/* Set a new skybox
	 * int index: the index of the skybox within skyboxes
	 */
	public void SetSkybox(int index) {
		if(index >= 0 && index < skyboxes.Length && index != currentIndex) {
			RenderSettings.skybox = skyboxes[index];
			UpdateCustomProperties(index);
			RaiseSkyboxChangeEvent();
			currentIndex = index;
		}
		else if(index >= skyboxes.Length){
			Debug.Log("OVAL/SkyboxChange/SetSkybox: Invalid index! Index given: " + index + ", Length of skyboxes array: " + skyboxes.Length);
		}
		else if(index < 0) {
			Debug.Log("OVAL/SkyboxChange/SetSkybox: Invalid index! Index must be greater than or equal to zero.");
		}
	}

	/* Updates the Photon Room's Custom Properties with the index of the current skybox
	 * int index: index of the current skybox
	 */
	void UpdateCustomProperties(int index) {
		//Insert skybox information into a hashtable
		Hashtable skyboxInfo = new Hashtable();
		skyboxInfo.Add(SKYBOX_INDEX, index);
		//Store skybox data for this room in Photon Custom Properties
		PhotonNetwork.room.SetCustomProperties(skyboxInfo);
	}

	/* Raises a Photon Event to let everyone know that the skybox was changed
	 * And the room's custom properties have been updated
	 */
	void RaiseSkyboxChangeEvent() {
		byte eventCode = SKYBOX_EVENT_CODE;
		bool reliable = true;
		PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
	}

	/* Called when an event is raised on the Photon Network
	 * Specifically watches for a SkyboxChangeEvent
	 */
	void OnEvent(byte eventcode, object content, int senderid) {

		//Code for SkyboxChangeEvent
		if (eventcode == SKYBOX_EVENT_CODE) {
			Debug.Log("Skybox Change event received!");
			//Get skybox info from Photon Custom Properties
			int index = (int)PhotonNetwork.room.customProperties["SBI"];
			//Set skybox
			if(index >= 0 && index < skyboxes.Length) {
				RenderSettings.skybox = skyboxes[index];
			}
			
		}
	}
}
