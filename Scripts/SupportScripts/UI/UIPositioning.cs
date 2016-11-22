using UnityEngine;
using System.Collections;

public class UIPositioning : MonoBehaviour {

	public Transform target;
	public Vector3 offset = Vector3.zero;
	public float distance;
	public float panelSpacing;
	public bool isObjectBackwards = false;

	Vector3 currentPosition;
	Vector3 previousPosition;

	// Use this for initialization
	void Start () {
		previousPosition = transform.position;
		if(target == null)
			Debug.LogError("OVAL/UIPositioning: The target of component UIPositioning is not initialized!");
		else
			AdjustDistance();
	}

	void OnEnable() {
		AdjustHorizontalSpacing();
	}
	
	// Update is called once per frame
	void Update () {
		currentPosition = transform.position;

		AdjustRotation(isObjectBackwards);

		if(currentPosition != previousPosition) {
			AdjustDistance();
			AdjustHorizontalSpacing();
		}

		previousPosition = transform.position;
	}

	void AdjustRotation(bool isObjectBackwards) {
		if(isObjectBackwards) {
			if(transform.GetSiblingIndex() == 0)
				transform.LookAt(2 * transform.position - (target.position + offset), target.up);
			else
				transform.LookAt(2 * transform.position - (target.position + offset), transform.parent.GetChild(0).up);
		}
		else {
			transform.LookAt(target.position + offset, transform.up);
		}
	}


	public void AdjustDistance() {
		Ray objectToTarget = new Ray(target.position + offset, (transform.position - (target.position + offset)).normalized);
		transform.position = objectToTarget.GetPoint(distance);
	}


	// Adjusts the horizontal spacing between menu panels
	public void AdjustHorizontalSpacing() {
		//Holds this object's closest sibling
		GameObject closestSibling = GetClosestSibling();

		//If closest sibling is not null
		if(closestSibling != null) {
			float myWidth = GetComponent<RectTransform>().sizeDelta.x * GetComponent<RectTransform>().localScale.x;
			float siblingWidth = closestSibling.GetComponent<RectTransform>().sizeDelta.x * closestSibling.GetComponent<RectTransform>().localScale.x;

			//Set this object's position and rotation equal to its sibling's position and rotation
			transform.localPosition = closestSibling.transform.localPosition;
			transform.localRotation = closestSibling.transform.localRotation;
			//Swing ui panel to the right by (0.5*siblingWidth + 0.5*thisWidth + spacing)
			float separation = (siblingWidth/2 + myWidth/2 + panelSpacing);
			float degreesToSwing = Mathf.Rad2Deg * Mathf.Acos((2*Mathf.Pow(distance, 2f) - Mathf.Pow(separation, 2f))/(2*Mathf.Pow(distance, 2f)));
			transform.RotateAround(target.position + offset, closestSibling.transform.up, degreesToSwing);
		}
	}


	// Returns the nearest activated sibling with a lower index
	private GameObject GetClosestSibling() {
		//Get this object's sibling index
		int mySiblingIndex = transform.GetSiblingIndex();

		//Holds this object's closest sibling
		GameObject closestSibling = null;

		//Check for activated siblings with lower sibling indexes
		for(int i = 0; i < mySiblingIndex; i++) {
			GameObject sibling = transform.parent.GetChild(i).gameObject;
			if(sibling.activeInHierarchy) {
				closestSibling = sibling;
			}
		}

		return closestSibling;
	}


	private Vector3 CalcUpVector(Vector3 center, Vector3 closestSibling, Vector3 thisPosition) {
		Vector3 side1 = closestSibling - center;
		Vector3 side2 = thisPosition - center;
		Vector3 upVector = Vector3.Cross(side1, side2);
		return upVector;
	}
}
