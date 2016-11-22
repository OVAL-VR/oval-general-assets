using UnityEngine;
using System.Collections;

public class LookAtTransform : MonoBehaviour {

	public Transform target;
	public Vector3 offset = Vector3.zero;
	public bool isObjectBackwards = false;

	// Use this for initialization
	void Start () {
		if(target == null)
			Debug.LogError("The target of component LookAtTransform is not initialized!");
	}
	
	// Update is called once per frame
	void Update () {
		if(isObjectBackwards)	
			transform.LookAt(2 * transform.position - (target.position + offset));
		else
			transform.LookAt(target.position + offset);
	}
}
