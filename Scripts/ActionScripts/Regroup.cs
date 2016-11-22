using UnityEngine;
using System.Collections;

/* Allows a master client to call a "regroup" event,
 * forcing all other players to move to his or her location
 */
public class Regroup : Photon.MonoBehaviour {

	//Whether or not this user is currently regrouping
	bool isRegrouping = false;
	//The transform of the master client
	Transform masterTransform;
	//The transform of this user's OVALPlayer
	Transform ovalPlayer;
	//The FlyAround component of this user
	FlyAround flyAround;

	//The tour mode component of this user. Should be on the same object as this Regroup script
	TourMode tourMode;

	//The maximum distance between this user and the master client's ship
	public float maxDistance = 1.0f;
	//The speed of movement for a regrouping user
	public float speed = 0.2f;

	//Constant declaration for regroup photon event codes
	const byte REGROUP_EVENT_CODE = 003;
	const byte DISPERSE_EVENT_CODE = 004;


	// Use this for initialization
	void Start () 
	{
		ovalPlayer = transform.root;
		flyAround = ovalPlayer.gameObject.GetComponent<FlyAround>();
		tourMode = GetComponent<TourMode>();
		//Setup Photon Event Callback
		PhotonNetwork.OnEventCall += this.OnEvent;
	}


	// Update is called once per frame
	void Update () 
	{
		if(isRegrouping) 
		{
			float distance = Vector3.Distance(ovalPlayer.position, masterTransform.position);

			if(distance > maxDistance) 
			{
				float step = speed * Time.deltaTime;
        		ovalPlayer.position = Vector3.MoveTowards(ovalPlayer.position, masterTransform.position, step);
			}
		}
	}


	/* Raise either a regroup or disperse event, depending on the value of "regroup"
	 * bool regroup: If true, raise RegroupEvent. If false, raise DisperseEvent
	 */
	public void RegroupOrDisperse(bool regroup) 
	{
		if(regroup)
		{
			RaiseRegroupEvent();
		}
		else
		{
			RaiseDisperseEvent();
		}
	}


	/* Raise a regroup event in this photon room
	 */
	public void RaiseRegroupEvent() 
	{
		byte eventCode = REGROUP_EVENT_CODE;
		bool reliable = true;
		PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
	}


	/* Raise a disperse event in this photon room
	 */
	public void RaiseDisperseEvent() 
	{
		byte eventCode = DISPERSE_EVENT_CODE;
		bool reliable = true;
		PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
	}


	/* A function to be called when a Photon Event is raised
	 */
	private void OnEvent(byte eventcode, object content, int senderid)
	{
		//Code for Regroup Event
		if(eventcode == REGROUP_EVENT_CODE)
		{
			Debug.Log("Regroup event received!");
			masterTransform = GetMasterTransform();
			flyAround.enabled = false;
			tourMode.StopTour();
			isRegrouping = true;
		}
		//Code for Disperse Event
		else if(eventcode == DISPERSE_EVENT_CODE)
		{
			Debug.Log("Disperse event received!");
			flyAround.enabled = true;
			isRegrouping = false;
		}
	}


	/* Returns the transform of the master client's OVALPlayer gameobject
	 */
	Transform GetMasterTransform()
	{
		Transform masterTransform = null;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		foreach(GameObject player in players) 
		{
			if(player.GetComponent<PhotonView>().owner.isMasterClient) 
			{
				masterTransform = player.transform;
			}
		}

		if(masterTransform == null)
		{
			Debug.Log("OVAL/Regroup/GetMasterTransform: No transform found for the Master Client player!");
		}

		return masterTransform;
	}


	/* Called when a new master client is assigned
	 */
	void OnMasterClientSwitched() 
	{
		RaiseDisperseEvent();
	} 
}
