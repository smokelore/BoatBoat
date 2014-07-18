using UnityEngine;
using InControl;
using System.Collections;

public class OarController : InputController {
	public GameObject BoatBoat;
	private forceController forcer;
	//public float speedFactor;
	//public float rowFactor;
	public float rowAngleRange;
	public GameObject leftOar;
	public GameObject rightOar;
	public Transform rightguideball;
	public Transform leftguideball;
	public ParticleSystem splasher;

	/*public bool leftOarStarted;
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
	public float rightOarCurrent;*/

	public float maxHeight = 1.5f;
	public float rowSpeedMax = 8.0f;
	//private float rightTempHeight;
	//private float leftTempHeight;

	public Vector2 prevLeftStick;
	public Vector2 currentLeftStick;
	public Vector2 prevRightStick;
	public Vector2 currentRightStick;

	// Use this for initialization
	void Start () {
		forcer = BoatBoat.GetComponent<forceController>();
		forcer.allOars.Add(this);
		InputManager.Setup();
//		rightguideball.position = new Vector3(rightguideball.position.x, -1, rightguideball.position.z);
//		rightOar.transform.LookAt(rightguideball);
//
//		leftguideball.position = new Vector3(leftguideball.position.x, -1, leftguideball.position.z);
//		leftOar.transform.LookAt(leftguideball);
	}
	
	// Controls is called once per frame
	public override void Controls() {
		/*leftOarPrevious = leftOarCurrent;
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
		}*/

		/*rightOarPrevious = rightOarCurrent;
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
		}*/

		prevRightStick = currentRightStick;
		currentRightStick = RightStick;

		prevLeftStick = currentLeftStick;
		currentLeftStick = LeftStick;

		//Row(true, getRightRowAmount());
		//Row(false, getLeftRowAmount());

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

		HUDText();

		splash();
	}

	private bool isHittingWater(Vector2 stick, float degreeRange) {
		// degreeRange determines the number of degrees away from (0,-1) will be considered "hitting water"
		//Debug.Log(Vector2.Angle(new Vector2(0,-1), stick) + " " + (Vector2.Angle(new Vector2(0,-1), stick) <= degreeRange));	
		return (Vector2.Angle(new Vector2(0f,-1f), stick) <= degreeRange);
	}

	public float getRightRowAmount() {
		Vector2 startVector = new Vector2(-1f, 0f);
		float prevAngle = Vector2.Angle(startVector, prevRightStick);
		float currentAngle = Vector2.Angle(startVector, currentRightStick);
		float deltaAngle = Mathf.Clamp(currentAngle-prevAngle, -rowAngleRange, rowAngleRange);

		if (isHittingWater(currentRightStick, rowAngleRange)) {
			return deltaAngle/rowAngleRange;
		} else {
			return 0f;
		}
	}

	public float getLeftRowAmount() {
		Vector2 startVector = new Vector2(1f, 0f);
		float prevAngle = Vector2.Angle(startVector, prevLeftStick);
		float currentAngle = Vector2.Angle(startVector, currentLeftStick);
		float deltaAngle = Mathf.Clamp(currentAngle-prevAngle, -rowAngleRange, rowAngleRange);

		if (isHittingWater(currentLeftStick, rowAngleRange)) {
			return deltaAngle/rowAngleRange;
		} else {
			return 0f;
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

	private void HUDText() {
		Camera.main.GetComponent<HUD>().personName = "Left and Right Sticks to row oars";
		Camera.main.GetComponent<HUD>().levelTheme = "B to Dismount";
	}

	public void splash(){
		if(isHittingWater (RightStick, 80)){
			splasher.transform.eulerAngles = new Vector3(270, 0, 0);
			splasher.Emit (5);
		}
	}
}
