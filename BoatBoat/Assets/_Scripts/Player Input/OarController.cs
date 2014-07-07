using UnityEngine;
using InControl;
using System.Collections;

public class OarController : InputController {
	public GameObject BoatBoat;
	public float speedFactor;
	public float rowFactor;
	public GameObject leftOar;
	public GameObject rightOar;

	public bool leftOarStarted;
	public bool leftOarEnded;
	public float leftOarInitial;
	public float leftOarFinal;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
	}
	
	// Move is called once per frame
	public override void Move () {
		if (leftOarEnded) {
			if (leftOarStarted) {
				rowFactor = leftOarFinal - leftOarInitial;
				Debug.Log(rowFactor);
				leftOarStarted = false;
			} else if (LeftTrigger == 0f) {
				leftOarEnded = false;
			}
		} else if (leftOarStarted) {
			if (LeftTrigger > leftOarFinal) {
				leftOarFinal = LeftTrigger;
			} else {
				leftOarEnded = true;
			}
		} else {
			if (LeftTrigger > 0) {
				leftOarStarted = true;
				leftOarEnded = false;
				leftOarInitial = LeftTrigger;
				leftOarFinal = -1f;
			} else {
				leftOarStarted = false;
			}
		}
		

		if (RightTrigger > 0) {

		}
	}

	public override void Dismount() {
		// Call this method to return to default InputController
		if (BButton) {
			player.ResetController();
			UnsetPlayer();
		}
	}
}
