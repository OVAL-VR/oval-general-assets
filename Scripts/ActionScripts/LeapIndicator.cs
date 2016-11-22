using UnityEngine;
using System.Collections;

public class LeapIndicator : MonoBehaviour {

	/*
	[Tooltip("The current Leap Data Provider for the scene.")]
    public LeapProvider LeapDataProvider;
    [Tooltip("An optional alternate detector for pinching on the left hand.")]
    public Leap.Unity.PinchDetector LeftHandDetector;
    [Tooltip("An optional alternate detector for pinching on the right hand.")]
    public Leap.Unity.PinchDetector RightHandDetector;
	
	private Quaternion CurrentRotation;
	private Frame curFrame;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		curFrame = LeapDataProvider.CurrentFrame;

		if (Camera.main != null) {
          Quaternion HeadYaw = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
          CurrentRotation = Quaternion.Slerp(CurrentRotation, HeadYaw, 0.1f);
      	}
	}

	void Process() {
		//Calculate Shoulder Positions (for Projection)
        Vector3 ProjectionOrigin = Vector3.zero;
        if (Camera.main != null) {
          switch (curFrame.Hands[whichHand].IsRight) {
            case true:
              ProjectionOrigin = Camera.main.transform.position + CurrentRotation * new Vector3(0.15f, -0.2f, 0f);
              break;
            case false:
              ProjectionOrigin = Camera.main.transform.position + CurrentRotation * new Vector3(-0.15f, -0.2f, 0f);
              break;
          }
        }

        //Draw Shoulders as Spheres, and the Raycast as a Line
        if (DrawDebug) {
          DebugSphereQueue.Enqueue(ProjectionOrigin);
          Debug.DrawRay(ProjectionOrigin, CurrentRotation * Vector3.forward * 5f);
        }

        //Raycast from the shoulder through the knuckle
        if (true) {
          TipRaycast = GetLookPointerEventData(whichPointer, whichHand, whichFinger, ProjectionOrigin, CurrentRotation * Vector3.forward, false);
          if ((InteractionMode == InteractionCapability.Projective)) {
            PrevState[whichPointer] = pointerState[whichPointer]; //Store old state for sound transitionary purposes
          }
          UpdatePointer(whichPointer, PointEvents[whichPointer]);
          ProcessState(whichPointer, whichHand, whichFinger, TipRaycast);
        }
	}
	*/


}
