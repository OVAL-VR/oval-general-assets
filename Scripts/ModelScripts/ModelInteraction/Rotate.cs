using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* This script should be attached to a slider
 * It works by relating the changes in sliders to changes in the rotation of a gameobject
 * Each slider calls one of the rotate functions through its own OnValueChanged function in the inspector
 */
public class Rotate : MonoBehaviour {

	public Transform target;
	public string targetTag;
	public Text rotReadout;

	float xRotation;
	float yRotation;
	float zRotation;

	bool beenRotated = true;

	// Use this for initialization
	void Start () {
		if(targetTag != null) {
			target = GameObject.FindWithTag(targetTag).transform;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(!beenRotated) {
			target.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

			if(rotReadout != null && rotReadout.isActiveAndEnabled) 
			{
			rotReadout.text = "Rotation: (" + target.eulerAngles.x.ToString("F1") + ", " + target.eulerAngles.y.ToString("F1") + ", " + target.eulerAngles.z.ToString("F1") + ")";
			}

			beenRotated = true;
		}	
	}

	public void RotateByValueX(float rotateValue) {
		xRotation = (rotateValue - 0.5f)*360;
		beenRotated = false;
	}

	public void RotateByValueY(float rotateValue) {
		yRotation = (rotateValue - 0.5f)*360;
		beenRotated = false;
	}

	public void RotateByValueZ(float rotateValue) {
		zRotation = (rotateValue - 0.5f)*360;
		beenRotated = false;
	}
}
