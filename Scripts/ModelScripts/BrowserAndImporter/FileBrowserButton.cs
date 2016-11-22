using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Defines a button used as part of the FileBrowserUI
public class FileBrowserButton : MonoBehaviour {

	FileBrowserUI fileBrowserUI;

	//The normal color for buttons representing enclosing folders
	public Color enclosingFolderNormal;
	//The color when pressed for buttons representing enclosing folders
	public Color enclosingFolderPressed;

	// Use this for initialization
	void Start () {
		fileBrowserUI = (FileBrowserUI) FindObjectOfType(typeof(FileBrowserUI));
		if(fileBrowserUI == null){
			Debug.Log("You are missing the FileBrowserUI object! Unity was unable to find a GameObject with the FileBrowserUI component.");
		}

		StartCoroutine("SetEnclosingFolderColor");
	}

	// Displays a file or folder name on this button
	public void DisplayButtonText() {
		if(fileBrowserUI != null) {
			Text buttonText = GetComponentInChildren<Text>();
			fileBrowserUI.DisplaySelected(buttonText);
		}
	}

	// If this button represents an enclosing folder, colors the button
	IEnumerator SetEnclosingFolderColor() {
		yield return new WaitForSeconds(0.1f);

		if(IsEnclosingFolder()) {
			SetButtonColor(enclosingFolderNormal);
		}
	}

	// Whether or not this button represents an enclosing folder
	bool IsEnclosingFolder() {
		if(transform.GetSiblingIndex() == 0) {
			return true;
		}
		else {
			return false;
		}
	}

	// Sets the background color of this button
	void SetButtonColor(Color normalColor) {
		Image background = transform.GetChild(0).GetComponent<Image>();
		background.color = normalColor;
	}


}
