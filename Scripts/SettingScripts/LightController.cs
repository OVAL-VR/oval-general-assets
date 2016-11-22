using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightController : MonoBehaviour {

	public Light target;
	public string targetTag;
	public float intensifyMag = 2.0f;
	public Text intensityReadout;
	float initIntensity;

	// Use this for initialization
	void Start () {
		if(targetTag != null) {
			target = GameObject.FindWithTag(targetTag).GetComponent<Light>();
		}
		initIntensity = target.intensity;
	}

	public void IntensifyByValue(float intensifyValue) {
		//This line sets the new intensity based on intensifyValue
		target.intensity = (initIntensity * Mathf.Pow((intensifyValue + 0.5f) , intensifyMag));

		if(intensityReadout != null && intensityReadout.isActiveAndEnabled) {
			intensityReadout.text = "Intensity: " + Mathf.Pow((intensifyValue + 0.5f) , intensifyMag).ToString("F1");
		}
	}
}
