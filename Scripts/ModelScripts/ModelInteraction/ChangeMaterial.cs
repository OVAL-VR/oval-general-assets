using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// To be attached to the model container
public class ChangeMaterial : MonoBehaviour {

	public Material alternateMaterial;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ToggleObjectWithMaterial(Toggle toggle) {
		if(toggle.isOn) {
			ActivateObjectWithMaterial(alternateMaterial);
		} else {
			ActivateObjectWithDefaultMaterial();
		}
	}


	public void ActivateObjectWithDefaultMaterial() {
		//Deactivate all children of the model container
		foreach(Transform child in transform) {
			child.gameObject.SetActive(false);
		}
		//Activate the child with the specified material
		transform.GetChild(0).gameObject.SetActive(true); //Change into an rpc call for network functionality
	}

	public void ActivateObjectWithMaterial(Material material) {
		//Get name of the new material
		string materialName = material.name.Replace("(Instance)","");
		//If use of material is not found
		if(transform.Find(materialName) == null) {
			DuplicateObjectWithNewMaterial(material, transform.GetChild(0).gameObject);
		}
		//Deactivate all children of the model container
		foreach(Transform child in transform) {
			child.gameObject.SetActive(false);
		}
		//Activate the child with the specified material
		transform.Find(materialName).gameObject.SetActive(true); //Change into an rpc call for network functionality
	}


	void DuplicateObjectWithNewMaterial(Material newMaterial, GameObject myObject) {
		GameObject newMaterialObject = Instantiate(myObject, transform) as GameObject;
		string newMaterialName = newMaterial.name.Replace("(Instance)","");
		newMaterialObject.name = newMaterialName;
		AssignNewMaterial(newMaterial, newMaterialObject);
		//Add call to network instantiate newMaterialObject for pun functionality
	}


	void AssignNewMaterial(Material newMaterial, GameObject myObject) {
		//Assign the newMaterial to the meshRenderer of all children of this gameobject
		foreach(Renderer rend in myObject.GetComponentsInChildren<Renderer>()) {
			//Create an array of new materials with the same size as the materials array on this meshrenderer
			Material[] newMaterials = new Material[rend.materials.Length];
			//Fill the new materials array with the new material
			for(var i = 0; i < rend.materials.Length; i++) {
				newMaterials[i] = newMaterial;
			}
			//Replace all materials on this meshrenderer with the new material
			rend.materials = newMaterials;
		}
	}


	// void ResetMaterials() {
	// 	for(int i = 0; i < gameObject.GetComponentsInChildren<MeshRenderer>().Length; i++) {
	// 		gameObject.GetComponentsInChildren<MeshRenderer>()[i] = initialRenderers[i];
	// 	}
	// }
}
