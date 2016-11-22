using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour {

	// The duration of the screenshot timer
	public int timerDuration = 3;
	// The duration of the camera flash
	public float flashDuration;
	// The gameObject displaying the value of the screenshot timer
	public GameObject numberObject;
	// The text component displaying the value of the screenshot timer
	private Text numberText; 
	// Crosshairs for lining up the screenshot
	public GameObject crosshairs;
	// A white panel covering the entire camera view
	public GameObject flash;
	// The number of screenshots taken in this OVAL session
	int screenshotCount = 0;


	// Use this for initialization
	void Start () {
		numberText = numberObject.GetComponent<Text>();
		numberObject.SetActive (false);
		crosshairs.SetActive (false);
		flash.SetActive(false);
	}


	// Starts the screenshot process
	public void StartScreenshot () {
		StartCoroutine("ScreenshotTimer", timerDuration);
	}


	// Starts or stops the screenshot process, based on the value of "start"
	public void StartOrStopScreenshot (bool start) {
		if(start) {
			StartCoroutine("ScreenshotTimer", timerDuration);
		}
		else {
			StopCoroutine("ScreenshotTimer");
			numberObject.SetActive(false);
			crosshairs.SetActive(false);
		}
	}


	// Decrements the screenshot timer.
	// Calls TakeScreenshot when the timer hits 0
	IEnumerator ScreenshotTimer (int timer) {

		numberObject.SetActive(true);
		crosshairs.SetActive(true);

		numberText.text = timer.ToString();

		while(timer > 0) {
			yield return new WaitForSeconds(1f);
			timer -= 1;
			numberText.text = timer.ToString();
		}

		numberObject.SetActive(false);
		crosshairs.SetActive(false);

		TakeScreenshot();

		StartCoroutine("Flash");
	}


	// Take the screenshot and save it to the user's desktop
	void TakeScreenshot () {
		string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
		string screenshotFilename;
		string screenshotPath;

		do {
			screenshotCount++;
			screenshotFilename = "OVAL_Screenshot" + screenshotCount + ".jpg";
			screenshotPath = desktopPath + "\\" + screenshotFilename;
		} while (System.IO.File.Exists(screenshotPath));

		Debug.Log("Screenshot path: " + screenshotPath);
		Application.CaptureScreenshot(screenshotPath);
		Debug.Log ("Screenshot taken");
	}


	// Activate the flash object momentarily
	IEnumerator Flash () {
		yield return new WaitForSeconds (flashDuration);
		flash.SetActive (true);
		yield return new WaitForSeconds (flashDuration);
		flash.SetActive (false);
	}


	// Called when the user quits the application
	void OnApplicationQuit () {
		StopAllCoroutines ();
	}
}

