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
	public float aimX = -90f, aimY = 0;
	public Vector3 targetOffset;
	public Vector3 zeroOffset;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		cam = Camera.main;

		targetOffset = defaultOffset;
	}
	
	// Controls is called once per frame
	public override void Controls() {
		// left/right controls
		if (Mathf.Abs(LeftStick.x) > stickDeadzone) {
			aimX += LeftStick.x * sensitivity * Time.deltaTime;
			targetOffset = zeroOffset + Mathf.Sin(aimX * Mathf.PI/2) * Vector3.forward * radius + Mathf.Cos(aimX * Mathf.PI/2) * Vector3.right * radius;
		}

		LerpCamera();
	}

	public override void Idle() {
		LerpCamera();
	}

	private void LerpCamera() {
		aimAtPoint = boatboatObject.transform.position + Vector3.up * 2.5f;
		cam.transform.LookAt(aimAtPoint);

		Vector3 newForward = boatboatObject.transform.forward;
		newForward = new Vector3(newForward.x, 0, newForward.z).normalized;

		Vector3 newRight = boatboatObject.transform.right;
		newRight = new Vector3(newRight.x, 0, newRight.z).normalized;

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
}
