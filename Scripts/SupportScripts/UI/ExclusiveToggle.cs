using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* To be attached to a GameObject with a toggle component
 * Provides methods to disable sibling toggles when this toggle is activated
 */
public class ExclusiveToggle : MonoBehaviour {

	//Enum to select which type of exclusivity this toggle is using:
	public enum Exclusivity {AllSiblings, Specific};
	public Exclusivity exclusivity;

	//Variables for AllSiblings:
	//The toggle components of this object, its parent, and its siblings
	Toggle[] familyToggles;
	//==================================================================

	//Variables for Specific:
	//The toggle components to disable when this toggle is enabled
	public Toggle[] specificToggles;
	//==================================================================


	// Use this for initialization
	void Start () {
		familyToggles = FindFamilyToggles();
		if(familyToggles == null) {
			Debug.Log("No Toggle components found on this object, its siblings, or its parent!");
		}
	}

	// Find sibling toggles
	Toggle[] FindFamilyToggles () {
		Toggle[] familyToggles = transform.parent.GetComponentsInChildren<Toggle>();
		return familyToggles;
	}

	// Toggle siblings off
	public void ToggleSiblingsOff () {
		if(exclusivity == Exclusivity.AllSiblings) {
			ToggleAllSiblingsOff();
		}
		else if(exclusivity == Exclusivity.Specific) {
			ToggleSpecificSiblingsOff();
		}
	}

	// Toggle all siblings off
	void ToggleAllSiblingsOff () {
		foreach(Toggle toggle in familyToggles) {
			//If this toggle has just been toggled on
			if(GetComponent<Toggle>().isOn) {
				//If toggle does not belong to this object or its parent
				if(toggle.gameObject != gameObject && toggle.gameObject != transform.parent.gameObject) {
					toggle.isOn = false;
				}
			}
		}
	}

	// Toggle only specific siblings off
	void ToggleSpecificSiblingsOff () {
		foreach(Toggle toggle in specificToggles) {
			//If this toggle has just been toggled on
			if(GetComponent<Toggle>().isOn) {
				//Turn off the specific toggle
				toggle.isOn = false;
			}
		}
	}
}
