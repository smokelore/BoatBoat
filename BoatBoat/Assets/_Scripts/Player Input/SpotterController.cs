using UnityEngine;
using InControl;
using System.Collections;

public class SpotterController : InputController {
	public GameObject boatboatObject;
	public Camera cam;
	public float stickDeadzone;
	public float sensitivity;
	public Vector3 defaultOffset;
	public float lerpSpeed;
	private Vector3 aimAtPoint;
	public float radius;
	private float aimX = -1f, aimY = 0;
	public Vector3 targetOffset;
	public Vector3 zeroOffset;
	public Vector3 aimOffset;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		cam = Camera.main;

		targetOffset = defaultOffset;
	}
	
	// Controls is called once per frame
	public override void Controls() {
		float deltaX = 0f, deltaY = 0f;

		// left/right controls
		if (Mathf.Abs(LeftStick.x) > stickDeadzone) {
			deltaX = LeftStick.x * sensitivity * Time.deltaTime;
			aimX += deltaX;
			//targetOffset = zeroOffset + Mathf.Sin(aimX * Mathf.PI/2) * Vector3.forward * radius + Mathf.Cos(aimX * Mathf.PI/2) * Vector3.right * radius;
		}

		// up/down controls
		if (Mathf.Abs(LeftStick.x) > stickDeadzone) {
			deltaY = LeftStick.y * sensitivity*1.5f * Time.deltaTime;
			if (aimY + deltaY > -zeroOffset.y && aimY + deltaY < 10f) {
				aimY += deltaY;
			}
		}

		// zoom controls
		if (Mathf.Abs(RightStick.y) > stickDeadzone) {
			float deltaRadius = -RightStick.y * sensitivity * Time.deltaTime;
			if (radius + deltaRadius > 5f && radius + deltaRadius < 10f) {
				radius += deltaRadius;
			}
		}

		//if (deltaX + deltaY > 0f) {
			targetOffset = zeroOffset + Mathf.Sin(aimX * Mathf.PI/2) * Vector3.forward * radius + Mathf.Cos(aimX * Mathf.PI/2) * Vector3.right * radius + aimY * Vector3.up;
		//}

		LerpCamera();
		//HUDText();
	}

	public override void Idle() {
		radius = 5f;
		LerpCamera();
	}

	private void LerpCamera() {
		Vector3 newForward = boatboatObject.transform.forward;
		newForward = new Vector3(newForward.x, 0, newForward.z).normalized;

		Vector3 newRight = boatboatObject.transform.right;
		newRight = new Vector3(newRight.x, 0, newRight.z).normalized;

		aimAtPoint = boatboatObject.transform.position + newRight * aimOffset.x + Vector3.up * aimOffset.y + newForward * aimOffset.z;
		cam.transform.LookAt(aimAtPoint);

		Vector3 currentPosition = cam.transform.position;
		Vector3 targetPosition = boatboatObject.transform.position + newRight * targetOffset.x + Vector3.up * targetOffset.y + newForward * targetOffset.z;

		cam.transform.position = Vector3.Lerp(currentPosition, targetPosition, lerpSpeed * Time.deltaTime);
	}

	public override void Dismount() {
		// Call this method to return to default InputController
		if (BButton) {
			player.ResetController();
			UnsetPlayer();
		}
	}

	private void HUDText() {
		Camera.main.GetComponent<HUD>().personName = "Left Stick to move camera";
		Camera.main.GetComponent<HUD>().levelTheme = "B to dismount";
	}
}
