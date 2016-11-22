/* Manages the importing of all 3D models by checking for file type and calling the appropriate scripts
 * Can be attached to anything. Recommended: "Scripts" gameObject
 */
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ImportManager : Photon.MonoBehaviour {

	//A gameobject holding any manipulation components (rotate, scale, etc). Will be the parent of our imported object.
	GameObject container;
	//Constant declarations for Photon's Custom Properties to store model data
	const string MODEL_FILE_PATH = "MFP";
	const string MODEL_MAT_PATH = "MMP";
	const string MODEL_TEX_PATH = "MTP";


	// Use this for initialization
	public void Start () 
	{
		//Find Model Container
		container = GameObject.FindWithTag("ModelContainer");
		if(container == null) 
		{
			Debug.Log("No model container found! Ensure that a gameobject in the scene is tagged as \"ModelContainer\".");
		}

		//Setup Photon Event Callback
		PhotonNetwork.OnEventCall += this.OnEvent;
	}

	//Once the player has joined a Photon Room
	public void OnJoinedRoom()
	{
		//Import inital model
		OnEvent(001, null, 0);
	}

	/*This function takes a file path and a directory, and determines what functions to use based on the file type
	 */
	public void HandleFile (FileSystemInfo file, DirectoryInfo directory)
	{
		string extension = file.Extension;

		//Check the type of file
		if (extension == ".obj")
		{
			HandleObj(file, directory);
		}

	}


	/* Given an obj filepath and directory, this function scans the containing directory for materials and textures 
	 * The obj along with its material and textures is then directed down the appropriate import path
	 */
	private void HandleObj(FileSystemInfo file, DirectoryInfo directory)
	{
		//The name of the obj file, without the extension
		string fileName = file.Name.Substring(0 , file.Name.IndexOf('.'));
		//The path of the material file, if there is one
		string mtlPath = "";
		//Whether or not material file exists
		bool mtlExists = false;
		//A list of filepaths for textures
 		List<string> texturePaths = new List<string>();


		//Look for materials and textures
		foreach(FileSystemInfo otherFile in directory.GetFiles())
		{
			if(otherFile.Name == fileName + ".mtl")
			{
				mtlExists = true;
				mtlPath = otherFile.FullName;
			}
			else if(otherFile.Name.Contains(fileName) && (otherFile.Extension == ".jpg" || otherFile.Extension == ".png") )
			{
				//Add the .jpg or .png file to the texturePathList 
				texturePaths.Add(otherFile.FullName);
			}
		}

		//Import Obj using filepath and material (if available)
		if(mtlExists)
		{
			//Insert model information into a hashtable
			Hashtable modelInfo = new Hashtable();
			modelInfo.Add(MODEL_FILE_PATH, file.FullName);
			modelInfo.Add(MODEL_MAT_PATH, mtlPath);
			modelInfo.Add(MODEL_TEX_PATH, ListToString(texturePaths));
			//Store model data for this room in Photon Custom Properties
			PhotonNetwork.room.SetCustomProperties(modelInfo);
			//Send out Photon Event for the import
			byte eventCode = 001;
			bool reliable = true;
			PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
			//Import model
			ImportObjFromPath(file.FullName, mtlPath, texturePaths);
		}
		else
		{
			//Insert model information into a hashtable
			Hashtable modelInfo = new Hashtable();
			modelInfo.Add(MODEL_FILE_PATH, file.FullName);
			modelInfo.Add(MODEL_MAT_PATH, null);
			modelInfo.Add(MODEL_TEX_PATH, null);
			//Store model data for this room in Photon Custom Properties
			PhotonNetwork.room.SetCustomProperties(modelInfo);
			//Send out Photon Event for the import
			byte eventCode = 001;
			bool reliable = true;
			PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
			//Import model
			ImportObjFromPath(file.FullName);
		}
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
			string filepath = (string)PhotonNetwork.room.customProperties["MFP"];
 			string matpath = (string)PhotonNetwork.room.customProperties["MMP"];
 			List<string> texlist = StringToList((string)PhotonNetwork.room.customProperties["MTP"]);
			//ImportObjFromPath using info
			ImportObjFromPath(filepath, matpath, texlist);
		}
	}

	/* Calls the ObjImporter script to create a gameobject out of an obj string
	 * string objPath: The file path of the obj file to import
	 */
	private void ImportObjFromPath(string objPath)
	{
		//Converts file at given location into a string of text
		string objText = File.ReadAllText(objPath);

		//Import new model
		DestroyPrevModel();
		GameObject importObject = ObjImporter.Import(objText);
		importObject.transform.SetParent(container.transform);
		importObject.transform.localPosition = Vector3.zero;
		RestrictModelSize(importObject, container.GetComponent<Renderer>().bounds.size.x);
	}


	/* Calls the ObjImporter script to create a gameobject out of an obj string
	 * string objPath: The file path of the obj file to import
	 * string mtlPath: The file path of the material to import with the obj
	 */
	private void ImportObjFromPath(string objPath, string mtlPath, List<string> texturePaths)
	{		
		//Converts file at given location into a string of text
		string objText = File.ReadAllText(objPath);
		string mtlText = File.ReadAllText(mtlPath);
		//Converts texture paths to an array of Texture2D objects
		Texture2D[] textures = new Texture2D[texturePaths.Count];
		for(int i = 0; i < texturePaths.Count; i++)
		{
			textures[i] = LoadPNG(texturePaths[i]);
		}

		//Import new model
		DestroyPrevModel();
		GameObject importObject = ObjImporter.Import(objText, mtlText, textures);
		importObject.transform.SetParent(container.transform);
		importObject.transform.localPosition = Vector3.zero;
		RestrictModelSize(importObject, container.transform.localScale.x);
	}


	/* Given a filepath to a png, loads that file into unity.
	 * string filepath: path to the png file
	 */
	public static Texture2D LoadPNG(string filePath) 
	{
		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))
		{
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
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

	/* Rescales the imported object according to a specific unity-meters size restriction
	 */
	private void RestrictModelSize(GameObject importObject, float sizeRestriction) 
	{
		float initialImportScale = importObject.transform.localScale.x;
		float boundsWidth = 0;

		//If importObject has a single mesh renderer
		if (importObject.GetComponent<MeshRenderer>() != null) 
		{
			MeshRenderer meshRenderer;
			meshRenderer = importObject.GetComponent<MeshRenderer>();
			boundsWidth = meshRenderer.bounds.size.x;
		}
		//Else if importObject has multiple renderers 
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

		// Use imported model's scale/bounds ratio to resize it within the sizeRestriction
		float newScale = sizeRestriction * initialImportScale / boundsWidth;
		importObject.transform.localScale = new Vector3 (newScale, newScale, newScale*-1f);
	}

	/* Converts a List of strings to a single string for storage purposes
	 */
	private string ListToString(List<string> itemList)
	{
		//Create an array of strings that each represent one entry of the itemList
		string[] items = new string[itemList.Count];

		//Combine all items into one big string
		return string.Join("~", items);
	}

	/* De-Converts a string generated by ListToString
	 */
	private List<string> StringToList(string itemString)
	{
		List<string> itemList = new List<string>();

		//First, split the itemString into its entry segments
		string[] itemEntries = itemString.Split('~');

		//Then, add each entry to the itemList
		for(int i = 0; i < itemEntries.Length; i++)
		{
			itemList.Add(itemEntries[i]);
		}

		return itemList;
	}

}


