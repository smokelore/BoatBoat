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
	public ParticleSystem rightSplasher;
	public ParticleSystem leftSplasher;
	private int rightSplashCount;
	private int leftSplashCount;

	public float maxHeight = 1.5f;
	public float rowSpeedMax = 8.0f;

	public Vector2 prevLeftStick;
	public Vector2 currentLeftStick;
	public Vector2 prevRightStick;
	public Vector2 currentRightStick;

	// Use this for initialization
	void Start () {
		forcer = BoatBoat.GetComponent<forceController>();
		forcer.allOars.Add(this);
	}
	
	// Controls is called once per frame
	public override void Controls() {

		prevRightStick = currentRightStick;
		currentRightStick = RightStick;

		prevLeftStick = currentLeftStick;
		currentLeftStick = LeftStick;

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

	public override void Idle() {
		Debug.Log("IDLE OARS");
		float speed = 1f;
		if (Mathf.Abs(currentRightStick.x - 0f) > 0.1f || Mathf.Abs(currentRightStick.y + 1f) > 0.1f) {
			currentRightStick.x = Mathf.Lerp(currentRightStick.x, 0f, Time.deltaTime * speed);
			currentRightStick.y = Mathf.Lerp(currentRightStick.y, -1f, Time.deltaTime * speed);
			rightguideball.position = rightOar.transform.position + BoatBoat.transform.right * rightOar.transform.Find("Oar").lossyScale.x + BoatBoat.transform.up * currentRightStick.y * maxHeight - BoatBoat.transform.forward * currentRightStick.x * maxHeight;
			rightOar.transform.LookAt(rightguideball);
		}

		if (Mathf.Abs(currentLeftStick.x - 0f) > 0.1f || Mathf.Abs(currentLeftStick.y + 1f) > 0.1f) {
			currentLeftStick.x = Mathf.Lerp(currentLeftStick.x, 0f, Time.deltaTime * speed);
			currentLeftStick.y = Mathf.Lerp(currentLeftStick.y, -1f, Time.deltaTime * speed);
			leftguideball.position = leftOar.transform.position - BoatBoat.transform.right * leftOar.transform.Find("Oar").lossyScale.x + BoatBoat.transform.up * currentLeftStick.y * maxHeight + BoatBoat.transform.forward * currentLeftStick.x * maxHeight;
			leftOar.transform.LookAt(leftguideball);
		}
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
			rightSplasher.transform.eulerAngles = new Vector3(270, 0, 0);
			if(rightSplashCount < 25){
				rightSplasher.Emit (10);
			}
			rightSplashCount++;
		}else{
			rightSplashCount = 0;
		}

		if(isHittingWater(LeftStick, 80)){
			leftSplasher.transform.eulerAngles = new Vector3(270, 0, 0);
			if(leftSplashCount < 25){
				leftSplasher.Emit (10);
			}
			leftSplashCount++;
		}else{
			leftSplashCount = 0;
		}
	}
}
