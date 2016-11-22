using UnityEngine;
using System.Collections;

public class OVALShip : MonoBehaviour {

	// Spatial recalibration
    [Tooltip("Key to recenter the OVALShip space.")]
    [SerializeField]
    private KeyCode recenter = KeyCode.R;

    [Tooltip("Target of recentering")]
    public Transform target;

    [Tooltip("Offset from target")]
    public Vector3 offset = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(recenter)) {
			Recenter();
			RecenterUI();
		}
	}

	// Recenters the OVALShip on target, with offset
	void Recenter() {
		transform.position = target.position + offset;
	}

	// Recenters the UI on the OVALShip
	void RecenterUI() {
		Transform parent = transform.parent;
		UIPositioning[] posComponents = parent.GetComponentsInChildren<UIPositioning>();
		foreach(UIPositioning pos in posComponents) {
			pos.AdjustDistance();
		}
	}
}
