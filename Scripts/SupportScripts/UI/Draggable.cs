using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.InputModule;
using System.Collections;

//Allows an object to be dragged by the mocement of an input device when a UI Button is held down
public class Draggable : MonoBehaviour {

	//Enum to select which type of input device this build is using:
	public enum InputDevice {Leap, Vive, Cardboard, XBox, Mouse};
	public InputDevice inputDevice;
	//=============================================================

	//Variables for Leap Input Device:
	public LeapInputModule leapInput;
	public Transform referencePoint;
	public Transform leftPalm;
	Vector3 leftStartPosition;
	bool leftHandControlling = false;
	public Transform rightPalm;
	Vector3 rightStartPosition;
	bool rightHandControlling = false;
	//===============================

	//Variables for Vive:
	//==================

	//Variables for Cardboard:
	//=======================

	//Variables for Xbox Controller:
	//============================

	//Variables for all devices:
	public float dragMagnifier = 2.0f;
	//Arbitrary variable to be set if the object can or cannot be dragged at the moment
	bool canDrag = false;
	//Starting position of this object
	Vector3 objectStartingPosition;
	//=============================

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(canDrag)
			Drag();
	}

	//Function to be called by UI Button OnPointerDown
	public void StartDragging() {
		canDrag = true;

		if(inputDevice == InputDevice.Leap)
		{
			StartLeapDrag();
		}
		/*else if() --Other Input Devices Here -- */
	}

	//Function to be called by UI Button OnPointerUp
	public void StopDragging() {
		canDrag = false;
		//Debug.Log("Stop Dragging");
		if(inputDevice == InputDevice.Leap)
		{

		}
		/*else if() --Other Input Devices Here -- */
	}

	//Function to handle dragging of all sorts
	void Drag() {
		if(inputDevice == InputDevice.Leap)
			LeapDrag();
		/*else if(inputDevice == InputDevice.Vive)
			ViveDrag();
		else if(inputDevice == InputDevice.Cardboard)
			CardboardDrag();
		else if(inputDevice == InputDevice.Xbox)
			XboxDrag();
		else if(inputDevice == InputDevice.Mouse)
			MouseDrag();
		*/			
	}

	//Function to Initialize Leap Dragging
	void StartLeapDrag() {

		objectStartingPosition = transform.localPosition;

		if(leapInput.LeftHandDetector != null && leapInput.LeftHandDetector.IsPinching)
		{
			//Debug.Log("left hand pinching");
			leftStartPosition = referencePoint.position - leftPalm.position;
			leftHandControlling = true;
		}
		else if(leapInput.RightHandDetector != null && leapInput.RightHandDetector.IsPinching)
		{
			//Debug.Log("right hand pinching");	
			rightStartPosition = referencePoint.position - rightPalm.position;
			rightHandControlling = true;
		}

	}

	//Function to handle Leap Dragging
	void LeapDrag() {
		if(rightHandControlling)
		{
			Vector3 rightPalmUpdate = referencePoint.position - rightPalm.position;

			transform.localPosition = new Vector3(
				objectStartingPosition.x + (rightStartPosition.x - rightPalmUpdate.x)*dragMagnifier,
				objectStartingPosition.y + (rightStartPosition.y - rightPalmUpdate.y)*dragMagnifier,
				objectStartingPosition.z + (rightStartPosition.z - rightPalmUpdate.z)*dragMagnifier
				);
		}
		else if(leftHandControlling)
		{
			Vector3 leftPalmUpdate = referencePoint.position - leftPalm.position;

			transform.localPosition = new Vector3(
				objectStartingPosition.x + (leftStartPosition.x - leftPalmUpdate.x)*dragMagnifier,
				objectStartingPosition.y + (leftStartPosition.y - leftPalmUpdate.y)*dragMagnifier,
				objectStartingPosition.z + (leftStartPosition.z - leftPalmUpdate.z)*dragMagnifier
				);
		}

		//Cancel if user stops pinching
		if(rightHandControlling && !leapInput.RightHandDetector.IsPinching)
		{
			StopDragging();
			rightHandControlling = false;
		}
		else if(leftHandControlling && !leapInput.LeftHandDetector.IsPinching)	
		{
			StopDragging();
			leftHandControlling = false;
		}
	}

}
