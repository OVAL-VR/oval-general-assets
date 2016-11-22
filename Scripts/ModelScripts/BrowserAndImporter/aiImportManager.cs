/* Manages the importing of all 3D models by checking for file type and calling the appropriate scripts
 * Can be attached to anything. Recommended: "Scripts" gameObject
 */
using UnityEngine;
using UnityImporter;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class aiImportManager : Photon.MonoBehaviour {

	//A gameobject holding any manipulation components (rotate, scale, etc). Will be the parent of our imported object.
	GameObject container;
	// The aiImport script
	ModelImport modelImporter;
	//Constant declaration for Photon's Custom Properties to store model data
	const string MODEL_FILE_PATH = "MFP";


	// Use this for initialization
	public void Start () 
	{
		//Find Model Container
		container = GameObject.FindWithTag("ModelContainer");
		if(container == null) 
		{
			Debug.Log("OVAL/aiImportManager/Start(): No model container found! Ensure that a gameobject in the scene is tagged as \"ModelContainer\".");
		}

		//Find modelImporter
		modelImporter = GetComponentInChildren<ModelImport>();
		if(modelImporter == null) 
		{
			Debug.Log("OVAL/aiImportManager.cs/Start(): No model importer found! Ensure that a child of the aiImportManager has a ModelImport component.");
		}

		//Setup Photon Event Callback
		PhotonNetwork.OnEventCall += this.OnEvent;
	}


	// Once the player has joined a Photon Room
	public void OnJoinedRoom()
	{
		//Import inital model
		OnEvent(001, null, 0);
	}


	/* A function to be called when a Photon Event is raised
	 */
	private void OnEvent(byte eventcode, object content, int senderid)
	{
		//Code for Import Event
		if(eventcode == 001)
		{
			Debug.Log("Import event received!");
			//Get model info from Photon Custom Properties
			string filePath = (string)PhotonNetwork.room.customProperties["MFP"];
			//Import model
			if(filePath != null) {
				ImportModelFromPath(filePath);
			}
		}
	}


	/* This function takes a filepath and begins the import process
	 * string filePath: the file path of the model to be imported
	 */
	public void HandleImport(string filePath)
	{
		//Insert model information into a hashtable
		Hashtable modelInfo = new Hashtable();
		modelInfo.Add(MODEL_FILE_PATH, filePath);
		//Store model data for this room in Photon Custom Properties
		PhotonNetwork.room.SetCustomProperties(modelInfo);
		//Send out Photon Event for the import
		byte eventCode = 001;
		bool reliable = true;
		PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
		//Import model
		ImportModelFromPath(filePath);
	}


	/* Calls the ModelImport script to create a gameobject given a filePath
	 * string filePath: The file path of the model to import
	 */
	private void ImportModelFromPath(string filePath)
	{
		//Destroy old model
		DestroyPrevModel();
		//Reset model container transform
		ResetModelContainer();
		//Import new model
		GameObject importObject = modelImporter.LoadFileFromPath(filePath);
		if(importObject == null) {
			Debug.Log("OVAL/aiImportManager/ImportModelFromPath: ModelImport/LoadFileFromPath did not return a GameObject.");
		}
		else {
			importObject.transform.SetParent(container.transform);
			importObject.transform.localPosition = Vector3.zero;
			RestrictModelSize(importObject, container.transform.localScale.x);
		}
	}


	/* Destroys the imported model (a child of "container")
	 */
	private void DestroyPrevModel() 
	{
		if (container.transform.childCount > 0) 
		{
			foreach(Transform child in container.transform) 
			{
				Destroy(child.gameObject);
			}
		}
	}

	/* Resets the rotation and scale of the model container
	 */
	private void ResetModelContainer() 
	{
		container.transform.localScale = Vector3.one;
		container.transform.localEulerAngles = Vector3.zero;
	}


	/* Rescales the imported object according to a specific unity-meters size restriction
	 */
	private void RestrictModelSize(GameObject importObject, float sizeRestriction) 
	{
		float initialImportScale = importObject.transform.localScale.x;
		float boundsWidth = 1;

		//If importObject has a single mesh renderer (for small static models)
		if (importObject.GetComponent<MeshRenderer>() != null) 
		{
			MeshRenderer meshRenderer;
			meshRenderer = importObject.GetComponent<MeshRenderer>();
			boundsWidth = meshRenderer.bounds.size.x;
		}
		//Else if importObject has multiple renderers (for large static models)
		else if(importObject.GetComponentsInChildren<MeshRenderer>().Length != 0)
		{
			MeshRenderer[] meshRenderers;
			meshRenderers = importObject.GetComponentsInChildren<MeshRenderer>();
			float xmin = 0;
			float xmax = 0;

			foreach (MeshRenderer meshRend in meshRenderers)
			{
				if (meshRend.bounds.min.x <= xmin)
				{
					xmin = meshRend.bounds.min.x;
				}

				if (meshRend.bounds.max.x >= xmax)
				{
					xmax = meshRend.bounds.max.x;
				}

				boundsWidth = xmax - xmin;
			}
		}
		//Else if importObject has multiple skinned mesh renderers (For animated models)
		else if(importObject.GetComponentsInChildren<SkinnedMeshRenderer>().Length != 0)
		{
			SkinnedMeshRenderer[] meshRenderers;
			meshRenderers = importObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			float xmin = 0;
			float xmax = 0;

			foreach (SkinnedMeshRenderer meshRend in meshRenderers)
			{
				if (meshRend.bounds.min.x <= xmin)
				{
					xmin = meshRend.bounds.min.x;
				}

				if (meshRend.bounds.max.x >= xmax)
				{
					xmax = meshRend.bounds.max.x;
				}

				boundsWidth = xmax - xmin;
			}
		}

		// Use imported model's scale/bounds ratio to resize it within the sizeRestriction
		float newScale = sizeRestriction * initialImportScale / boundsWidth;
		Debug.Log("OVAL/aiImportManager/RestrictModelSize: boundsWidth = " + boundsWidth + ", newScale = " + newScale);
		importObject.transform.localScale = new Vector3 (newScale, newScale, newScale*-1f);
	}

}


