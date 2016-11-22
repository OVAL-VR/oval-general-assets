using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	Transform mainCamera;
	public bool isObjectBackwards = false;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
	if(isObjectBackwards)	
		transform.LookAt(2 * transform.position - mainCamera.position);
	else
		transform.LookAt(mainCamera);
	}
}
