using UnityEngine;
using UnityEngine.VR;
using System.Collections;

/*This script uses the Photon Unity Network to establish a room on the network.
 * It also handles initialization and instantiation of the player prefab in the scene.
 */
public class NetworkManager : Photon.PunBehaviour {

	//The player prefab to instantiate
	public GameObject playerPrefab;
	//The PUN logLevel
	public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
	//The version of OVAL
	public string OVALVersion = "1";
	//The maximum number of players per room
	public byte maxPlayersPerRoom = 10;
	//If set to true, the program runs as expeced, without connecting to the Photon Unity Network
	public bool offlineMode = false;
	//An array of "SpawnSpots". Gameobjects detailing the locations where the Player should spawn.
	SpawnSpot[] spawnSpots = null;
	
	// Early initialization
	void Awake() {
		PhotonNetwork.logLevel = logLevel;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.automaticallySyncScene = true;
	}

	// Use this for initialization
	void Start () {
		//Finds all SpawnSpots in the scene
		spawnSpots = GameObject.FindObjectsOfType(typeof(SpawnSpot)) as SpawnSpot[];
		
		if (offlineMode) {
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.CreateRoom ("null");
		} 
		else {
			Connect();
		}
		
	}
	
	//Connects to Photon Unity Network
	void Connect () {
		if(PhotonNetwork.connected) {
			PhotonNetwork.JoinRandomRoom();
		}
		else {
			//Connects to all other OVAL instances using the same "PUN Session Name", as defined by the user in the main menu
			PhotonNetwork.ConnectUsingSettings(OVALVersion + PlayerPrefs.GetString("PUN Session Name","default"));
		}
	}
	
	#region Photon.PunBehaviour CallBacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("OVAL2.0/NetworkManager: OnConnectedToMaster() was called by PUN");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("OVAL2.0/NetworkManager:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = maxPlayersPerRoom}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom}, null);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("OVAL2.0/NetworkManager: OnJoinedRoom() called by PUN. Now this client is in a room.");
		//Activate VR
		//VRSettings.enabled = true;
		//Spawn Player
		SpawnMyPlayer ();
	}

	public override void OnDisconnectedFromPhoton()
	{
		Debug.LogWarning("OVAL2.0/NetworkManager: OnDisconnectedFromPhoton() was called by PUN");
	}

	#endregion
	
	//Handles the instantiation of the player prefab
	void SpawnMyPlayer() {
		
		//If there are no spawnSpots
		if (spawnSpots.Length == 0) {
			Debug.Log("There are no spawnspots in the scene");
			//Initialize myPlayer to a new instantiated player prefab
			PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), new Quaternion(), 0);
		} 
		else 
		{
			//Choose a random SpawnSpot
			SpawnSpot mySpawnSpot = spawnSpots [Random.Range(0, spawnSpots.Length)];
			//Initialize myPlayer to a new instantiated player prefab
			PhotonNetwork.Instantiate(playerPrefab.name, mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		}
		
	}
}
