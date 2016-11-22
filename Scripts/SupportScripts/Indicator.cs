using UnityEngine;
using System.Collections;

public class Indicator : Photon.MonoBehaviour {
	
	//The arrow or 'pointer' of the indicator
	public GameObject arrow;
	
	//True when an indicator is placed. Declared as a property to prevent it from showing up in the inspector.
	public bool indicatorPlaced { get; set; }
	
	//These variables control movement of the arrow.
	Vector3 rotationAngle;
	int rotationSpeed;
	Vector3 translateVector;
	int bobTracker = 0;
	bool bobUp = true;
	
	//These variables determine the length of time the indicator remains visible
	double timePlaced = 0.0;
	double lifeSpan = 10.0;
	
	//The sound the indicator plays when placed
	AudioClip sonar;
	
	
	// Use this for initialization
	void Start () 
	{
		rotationSpeed = 50;
		sonar = gameObject.GetComponent<AudioSource>().clip;
		photonView.RPC ("Show", PhotonTargets.All, false);
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		//Rotates the arrow
		transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime);
		
		
		//Bobs the arrow
		if (bobTracker < 20 && bobUp) {
			arrow.transform.position += (Vector3.up/20)*Time.deltaTime;
			bobTracker++;
		} else {
			bobUp = false;
		}
		if (bobTracker > 0 && !bobUp) {
			arrow.transform.position -= (Vector3.up/20)*Time.deltaTime;
			bobTracker--;
		} else {
			bobUp = true;
		}
		
		
		//If an indicator has just been placed, it will be visible across the network.
		if (indicatorPlaced) 
		{
			timePlaced = PhotonNetwork.time;
			indicatorPlaced = false;
			photonView.RPC ("Show", PhotonTargets.All, true);
		}
		
		//If the time passed since the indicator was placed is greater than its lifespan, hide the indicator.
		if ((PhotonNetwork.time - timePlaced) > lifeSpan) 
		{
			photonView.RPC("Show", PhotonTargets.All, false);
		}
		
	}
	
	
	//Plays an audio clip
	[PunRPC]
	public void PlaySound()
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot (sonar);
	}
	
	
	/* Sets color of the indicator
	 * r: The red value of the new color
	 * g: The green value of the new color
	 * b: The blue value of the new color
	 */
	[PunRPC]
	public void SetColor(float r, float g, float b)
	{
		Color indicatorColor = new Color (r, g, b);
		GetComponent<MeshRenderer> ().material.SetColor ("_Color", indicatorColor);
		arrow.GetComponent<MeshRenderer> ().material.SetColor ("_Color", indicatorColor);
	}
	
	
	/* Controls visibility of the indicator
	 * show: 'true' shows the indicator, 'false' hides it.
	 */
	[PunRPC]
	public void Show(bool show)
	{
		transform.GetComponent<MeshRenderer> ().enabled = show;
		arrow.transform.GetComponent<MeshRenderer> ().enabled = show;
	}
	
}