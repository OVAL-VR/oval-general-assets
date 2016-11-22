using UnityEngine;
using System.Collections;

public class MenuModeSwitch : MonoBehaviour {

	public float touchDistance;
	public float normalDistance;

	// To be called by a button. Changes the distance of the menu
	public void SwitchMenuMode(bool touchMenu) {

		foreach(Transform panel in gameObject.transform.parent) {
			UIPositioning uipos = panel.GetComponent<UIPositioning>();
			if(uipos != null) {
				if(touchMenu)
					uipos.distance = touchDistance;
				else
					uipos.distance = normalDistance;

				uipos.AdjustDistance();
				uipos.AdjustHorizontalSpacing();
			}
		}
	}
}
