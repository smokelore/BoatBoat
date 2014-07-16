using UnityEngine;
using InControl;
using System.Collections;

public class OarController : InputController {
	public GameObject BoatBoat;
	public float speedFactor;
	public float rowFactor;
	public GameObject leftOar;
	public GameObject rightOar;
	public Transform rightguideball;
	public Transform leftguideball;

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

	public float maxHeight = 1.5f;
	public float rowSpeedMax = 8.0f;
	public float rightTempHeight;
	public float leftTempHeight;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
//		rightguideball.position = new Vector3(rightguideball.position.x, -1, rightguideball.position.z);
//		rightOar.transform.LookAt(rightguideball);
//
//		leftguideball.position = new Vector3(leftguideball.position.x, -1, leftguideball.position.z);
//		leftOar.transform.LookAt(leftguideball);
	}
	
	// Move is called once per frame
	public override void Move () {
		leftOarPrevious = leftOarCurrent;
		leftOarCurrent = LeftStick.y;

		/*if (leftOarStarted) {
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
		}*/

		rightOarPrevious = rightOarCurrent;
		rightOarCurrent = RightStick.y;

		/*if (rightOarStarted) {
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
		}*/

		//****OAR ANIMATION****//

		//STICK CONTROLS
		//rightTempHeight = Mathf.MoveTowards(rightguideball.position.y, RightStick.y * maxHeight, rowSpeedMax * Time.deltaTime);
		//rightguideball.position = new Vector3(rightguideball.position.x, rightTempHeight, rightguideball.position.z);
		rightguideball.position = rightOar.transform.position + BoatBoat.transform.right * rightOar.transform.Find("Oar").lossyScale.x + BoatBoat.transform.up * RightStick.y * maxHeight - BoatBoat.transform.forward * RightStick.x * maxHeight;
		rightOar.transform.LookAt(rightguideball);

		//leftTempHeight = Mathf.MoveTowards(leftguideball.position.y, LeftStick.y * maxHeight, rowSpeedMax * Time.deltaTime);
		//leftguideball.position = new Vector3(leftguideball.position.x, leftTempHeight, leftguideball.position.z);
		leftguideball.position = leftOar.transform.position - BoatBoat.transform.right * leftOar.transform.Find("Oar").lossyScale.x + BoatBoat.transform.up * LeftStick.y * maxHeight + BoatBoat.transform.forward * LeftStick.x * maxHeight;
		leftOar.transform.LookAt(leftguideball);

		//TRIGGER CONTROLS
		/*rightTempHeight = Mathf.MoveTowards(rightguideball.position.y, RightTrigger * maxHeight, rowSpeedMax * Time.deltaTime);
		rightguideball.position = new Vector3(rightguideball.position.x, rightTempHeight, rightguideball.position.z);
		rightOar.transform.LookAt(rightguideball);

		leftTempHeight = Mathf.MoveTowards(leftguideball.position.y, LeftTrigger * maxHeight, rowSpeedMax * Time.deltaTime);
		leftguideball.position = new Vector3(leftguideball.position.x, leftTempHeight, leftguideball.position.z);
		leftOar.transform.LookAt(leftguideball);*/

		//**** OAR ANIMATION END****//
	}

	private bool isHittingWater(Vector2 stick, float degreeRange) {
		// degreeRange determines the number of degrees away from (0,-1) will be considered "hitting water"
		//Debug.Log(Vector2.Angle(new Vector2(0,-1), stick) + " " + (Vector2.Angle(new Vector2(0,-1), stick) <= degreeRange));	
		return (Vector2.Angle(new Vector2(0,-1), stick) <= degreeRange);
	}

	/*private float getRowAmount(Vector2 stick) {

	}*/

	public void Row(bool right, float amount) {
		if (right) {
			//Debug.Log("Player " + player.playerNum + " rowed right oar: " + amount);
		} else {
			//Debug.Log("Player " + player.playerNum + " rowed left oar: " + amount);
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
