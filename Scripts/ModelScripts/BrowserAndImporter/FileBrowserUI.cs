using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

//Generates and displays a list of buttons containing the contents of the current directory
public class FileBrowserUI : MonoBehaviour {

	public FileBrowser fileBrowser;
	public GameObject browserContentsPanel;
	public Text currentDirectoryLabel;
	public Text selectedReadout;
	public GameObject buttonPrefab;
	public Vector3 topLeftButtonPos;
	public Vector3 buttonSpacing;

	
	// Use for initialization
	void Start () {
		//Find the FileBrowser in the scene, a child of ManagerScripts
		fileBrowser = GameObject.FindWithTag("ManagerScripts").transform.GetComponentInChildren<FileBrowser>();
	}

	// Update is called once per frame
	void Update () {
		if(fileBrowser.currentDirectory.Name != null && currentDirectoryLabel.text != fileBrowser.currentDirectory.Name) {
			currentDirectoryLabel.text = fileBrowser.currentDirectory.Name;
			AdjustBrowserContentsPanel();
			DestroyButtons();
			GenerateButtons();
		}
	}

	// Adjust the size of the browser contents panel based on the size of the current directory
	private void AdjustBrowserContentsPanel () {
		int directorySize = fileBrowser.folderContentNames.Count;
		float newHeight = Mathf.Abs(topLeftButtonPos.y) * 2 + buttonSpacing.y * directorySize / 2;
		RectTransform contentHolder = browserContentsPanel.transform.parent.GetComponent<RectTransform>();
		contentHolder.sizeDelta = new Vector2(contentHolder.sizeDelta.x, newHeight);
	}

	// Generate buttons based on the content of the current directory
	private void GenerateButtons () {

		buttonPrefab.SetActive(true);

		for(int i = 0; i < fileBrowser.folderContentNames.Count; i++) {

			//Instantiate button as child of browserContentsPanel and set buttonText to folder content name
			GameObject button = Instantiate(buttonPrefab, browserContentsPanel.transform, false) as GameObject;
			button.GetComponentInChildren<Text>().text = fileBrowser.folderContentNames[i];

			// Set button transform values
			if(i % 2 == 0) {
				float xPos = topLeftButtonPos.x;
				float yPos = topLeftButtonPos.y - buttonSpacing.y * i/2;
				button.GetComponent<RectTransform>().anchoredPosition = new Vector3(xPos, yPos, 0);
			} else {
				float xPos = topLeftButtonPos.x + buttonSpacing.x;
				float yPos = topLeftButtonPos.y - buttonSpacing.y * (i-1)/2;
				button.GetComponent<RectTransform>().anchoredPosition = new Vector3(xPos, yPos, 0);
			}
		}

		buttonPrefab.SetActive(false);
	}

	// Destroy all buttons in the browserContentsPanel
	private void DestroyButtons () {
		foreach(Transform child in browserContentsPanel.transform) {
			if(child != buttonPrefab) {
				Destroy(child.gameObject);
			}
		}
	}

	// Displays a currently selected button's text content in the "Selected" readout 
	public void DisplaySelected (Text selectedText) {
		selectedReadout.text = selectedText.text;
	}

	// To be activated by a button. Clears the "Selected" readout
	public void ClearSelected () {
		selectedReadout.text = "";
	}

	// To be activated by a button. Passes selectedText up to FileBrowser.
	public void HandleSelectedText (Text selectedText) {
		fileBrowser.HandleSelectedText(selectedText);
	}
}
