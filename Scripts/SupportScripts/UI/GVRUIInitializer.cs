using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

//This class sets the camera of the GVRHeadMountedRig as the event camera for all UI canvases in the scene
//Allowing a GVR Headset user to interact with the UI
//Also activates the GVREventSystem
public class GVRUIInitializer : MonoBehaviour {

	public Camera gvrCamera;
	public GameObject Menu;

	// Use this for initialization
	void Start () {
		Canvas[] canvases = Menu.GetComponentsInChildren<Canvas>(true);
		foreach(Canvas canvas in canvases) {
			Debug.Log("Setting event camera!");
			canvas.worldCamera = gvrCamera;
		}

		EventSystem[] eventSystems = Menu.GetComponentsInChildren<EventSystem>(true);
		EventSystem gvrEventSystem = null;
		//Deactivate all event systems
		foreach(EventSystem es in eventSystems) {
			Debug.Log("Deactivating event systems!");
			es.gameObject.SetActive(false);
			if(es.GetComponent<GazeInputModule>() != null)
				gvrEventSystem = es;
		}
		//Activate the gvreventsytem
		if(gvrEventSystem != null) {
			Debug.Log("Activating event system!");
			gvrEventSystem.gameObject.SetActive(true);
		}
	}	
	
	// Update is called once per frame
	void Update () {
	
	}
}
