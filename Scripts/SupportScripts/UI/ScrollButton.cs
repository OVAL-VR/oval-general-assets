using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollButton : MonoBehaviour {

	public ScrollRect scrollRect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveScrollBar(float increment) {
		scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition + increment, 0, 1);
	}
}
