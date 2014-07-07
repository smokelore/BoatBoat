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
	public float leftOarPrevious;
	public float leftOarCurrent;

	public bool rightOarStarted;
	public bool rightOarEnded;
	public float rightOarInitial;
	public float rightOarFinal;
	public float rightOarPrevious;
	public float rightOarCurrent;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
	}
	
	// Move is called once per frame
	public override void Move () {
		leftOarPrevious = leftOarCurrent;
		leftOarCurrent = LeftStick.y;

		if (leftOarStarted) {
			if (leftOarEnded) {
				// row finished
				if (leftOarFinal > leftOarInitial) {
					Row(false, Mathf.Pow((leftOarFinal - leftOarInitial)/2, 2));
				}
				leftOarStarted = false;
			} else {
				// row in progress
				if (leftOarCurrent >= leftOarPrevious) {
					// still rowing
					leftOarFinal = leftOarCurrent;
				} else {
					// stopped rowing
					leftOarEnded = true;
				}
			}
		} else {
			if (leftOarEnded) {
				// row needs resetting
				if (leftOarCurrent > leftOarPrevious) {
					// new row started before last row reset
					leftOarStarted = true;
					leftOarEnded = false;
					leftOarInitial = leftOarCurrent;
				}
			} else {
				// row already reset
				if (leftOarCurrent > 0) {
					// new row started
					leftOarStarted = true;
					leftOarInitial = leftOarCurrent;
				}
			}
		}

		rightOarPrevious = rightOarCurrent;
		rightOarCurrent = RightStick.y;

		if (rightOarStarted) {
			if (rightOarEnded) {
				// row finished
				if (rightOarFinal > rightOarInitial) {
					Row(true, Mathf.Pow((rightOarFinal - rightOarInitial)/2, 2));
				}
				rightOarStarted = false;
			} else {
				// row in progress
				if (rightOarCurrent >= rightOarPrevious) {
					// still rowing
					rightOarFinal = rightOarCurrent;
				} else {
					// stopped rowing
					rightOarEnded = true;
				}
			}
		} else {
			if (rightOarEnded) {
				// row needs resetting
				if (rightOarCurrent > rightOarPrevious) {
					// new row started before last row reset
					rightOarStarted = true;
					rightOarEnded = false;
					rightOarInitial = rightOarCurrent;
				}
			} else {
				// row already reset
				if (rightOarCurrent > 0) {
					// new row started
					rightOarStarted = true;
					rightOarInitial = rightOarCurrent;
				}
			}
		}
		

	}

	public void Row(bool right, float amount) {
		if (right) {
			Debug.Log("Player " + player.playerNum + " rowed right oar: " + amount);
		} else {
			Debug.Log("Player " + player.playerNum + " rowed left oar: " + amount);
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
