using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Text selectedRoomText;

	//Updates selectedRoomText using a given text component
	public void UpdateSelectedRoomText(Text roomText) {
		selectedRoomText.text = "Selected Room: " + roomText.text;
	}

	//Updates the selected room in player preferences
	public void UpdateSelectedRoom(Text roomText) {
		PlayerPrefs.SetString("PUN Session Name", roomText.text);
		Debug.Log("PUN Room Name Set:" + roomText.text);
	}

	//Updates selectedRoomText using a given string
	public void UpdateSelectedRoomText(string roomString) {
		selectedRoomText.text = "Selected Room: " + roomString;
	}

	//Updates the selected room in player preferences
	public void UpdateSelectedRoom(string roomString) {
		PlayerPrefs.SetString("PUN Session Name", roomString);
		Debug.Log("PUN Room Name Set:" + roomString);
	}

	//Loads the scene with the given index
	public void LoadScene(int index) {
		StartCoroutine("LoadSceneSlowly", index);
	}

	//Waits for two seconds, then loads the scene at the given index
	IEnumerator LoadSceneSlowly(int index) {
		yield return new WaitForSeconds(2.0f);
		//Begin loading the scene
		AsyncOperation async = SceneManager.LoadSceneAsync(index);
		//Do not allow scene activation while scene is loading
		async.allowSceneActivation = false;

		while(async.progress < 0.9f) {
			//Debug.Log("Loading scene. Progress: " + async.progress);
		}
		
		//Activate scene to finish the last bit of loading
		async.allowSceneActivation = true;
		while(!async.isDone) {
			//Debug.Log("Almost done. Progress: " + async.progress);
			yield return null;
		}
	}

	//Quits the application
	public void QuitApplication() {
		Application.Quit();
	}
}
