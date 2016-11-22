using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* This script should be attached to a slider
 * It works by relating the changes in a slider to changes in the scale of a gameobject
 */
public class Rescale : MonoBehaviour {

	public Transform target;
	public string targetTag;
	public float rescaleMag = 2.0f;
	public Text scaleReadout;
	Vector3 initScale;
	
	// Use this for initialization
	void Start () {
		if(targetTag != null) {
			target = GameObject.FindWithTag(targetTag).transform;
		}
		initScale = target.lossyScale;
	}

	public void RescaleByValue(float rescaleValue) {
		//This line sets the new scale based on the rescale value and magnitude
		target.localScale = new Vector3 (
			(initScale.x * Mathf.Pow((rescaleValue + 0.5f) , rescaleMag) ), 
			(initScale.y * Mathf.Pow((rescaleValue + 0.5f) , rescaleMag) ), 
			(initScale.z * Mathf.Pow((rescaleValue + 0.5f ) , rescaleMag) ) 
			);

		if(scaleReadout != null && scaleReadout.isActiveAndEnabled) 
		{
			scaleReadout.text = "Scale: " + Mathf.Pow((rescaleValue + 0.5f) , rescaleMag).ToString("F1");
		}
	}
}
