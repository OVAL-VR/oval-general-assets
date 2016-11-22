using UnityEngine;
using System.Collections;

//Synchronizes a TourContainer gameobject with the transform of a ModelContainer
public class TourContainer : MonoBehaviour {

	//Whether or not to synchronize rotation with the model container
	public bool syncRotation = true;
	//Whether or not to synchronize scale with the model container
	public bool syncScale = true;
	//The transform of the model container
	Transform modelContainer;

	// Use this for initialization
	void Start () 
	{
		modelContainer = GameObject.FindWithTag("ModelContainer").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(syncRotation)
			transform.rotation = modelContainer.rotation;
		if(syncScale)
			transform.localScale = modelContainer.localScale;
	}
}
