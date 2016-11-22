using UnityEngine;
using System.Collections;

/* This script moves a user along a predefined series of sequential points 
 */
public class TourMode : MonoBehaviour {

	//Whether or not this user is currently touring
	bool isTouring = false;
	//The FlyAround component of this user, disabled while touring
	FlyAround flyAround;

	//Speed of movement for the user
	public float speed = 0.5f;
	public float rotationSpeed = 0.5f;

	//Transform of this user's OVALPlayer
	Transform ovalPlayer;
	//Transform of the model container
	Transform modelContainer;

	//Transform of the TourContainer, gameobject holding all of the tour points
	Transform tourContainer;
	//The index of the next tour point to move to
	int nextPointIndex = 0;
	//The transform of the next tour point
	Transform nextPoint;
	//The maximum distance at which the tour point is considered "reached"
	public float triggerDistance;


	// Use this for initialization
	void Start () 
	{
		ovalPlayer = transform.root;
		flyAround = ovalPlayer.gameObject.GetComponent<FlyAround>();
		modelContainer = GameObject.FindWithTag("ModelContainer").transform;
		tourContainer = GameObject.FindWithTag("TourContainer").transform;
		nextPoint = tourContainer.GetChild(nextPointIndex);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isTouring)
		{
			if(Vector3.Distance(ovalPlayer.position, nextPoint.position) > triggerDistance) 
			{
				//Move ovalPlayer towards next point
				ovalPlayer.position = Vector3.Lerp(ovalPlayer.position, nextPoint.position, speed);
        		//Rotate ovalPlayer toward modelContainer
        		Quaternion lookTarget = Quaternion.LookRotation(modelContainer.position - ovalPlayer.position, Vector3.up);
        		ovalPlayer.rotation = Quaternion.Slerp(ovalPlayer.rotation, lookTarget, rotationSpeed*Time.deltaTime);

			}
			else
			{
				if(nextPointIndex < tourContainer.childCount -1)
				{
					nextPointIndex++;
					nextPoint = tourContainer.GetChild(nextPointIndex);
				}
				else
				{
					nextPointIndex = 0;
					nextPoint = tourContainer.GetChild(nextPointIndex);
				}
			}

		}
	}

	/* Starts or Stops a tour, depending on value of "start"
	 * bool start: Calls StartTour if true, calls StopTour if false
	 */
	public void StartOrStopTour(bool start) 
	{
		if(start)
		{
			StartTour();
		}
		else
		{
			StopTour();
		}
	}

	/* Starts a tour by setting isTouring = true
	 */
	public void StartTour() 
	{
		flyAround.enabled = false;
		isTouring = true;
	}

	/* Stops a tour by setting isTouring = false
	 */
	public void StopTour() 
	{
		isTouring = false;
		flyAround.enabled = true;
	}
}
