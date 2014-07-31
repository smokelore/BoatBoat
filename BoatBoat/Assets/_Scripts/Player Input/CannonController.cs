using UnityEngine;
using InControl;
using System.Collections;
using System.Collections.Generic;

public class CannonController : InputController {
	public bool rightSide;
	public GameObject boatboatObject;
	public GameObject cannonObject;
	public GameObject cannonballPrefab;
	public GameObject smokeShotPrefab;
	public Transform cannonballSpawn;
	private GameObject cannonballTemp;
	private GameObject smokeTemp;
	public bool loaded, loading, canShoot;
	public float reloadDuration = 3f;
	private float reloadCount;

	public float stickDeadzone;
	public float aimSpeed;
	public float aimX, aimY;
	private bool needsResetting;
	private LineRenderer lineRenderer;

	public GameObject indicatorPrefab;
	private GameObject indicatorObject;
	public float indicatorHeight = 0.2f;
	public float indicatorPeriod = 1;
	public float indicatorAmplitude = 0.05f;

	private bool oldXButton;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		cannonballSpawn = cannonObject.transform.Find("chamber/spawn");
		lineRenderer = cannonballSpawn.gameObject.GetComponent<LineRenderer>();
	}
	
	// Controls is called once per frame
	public override void Controls() {
		if (cannonballTemp == null) {
			canShoot = true;
		} else {
			canShoot = false;
		}

		AimControls();
		ShootControls();
		ReloadControls();
		//HUDText();
		ZoneIndicator();

		needsResetting = true;
		oldXButton = XButton;
	}

	private void AimControls() {
		// left/right movement
		if (Mathf.Abs(LeftStick.x) > stickDeadzone) {
			float deltaX = aimSpeed * LeftStick.x * Time.deltaTime;
			if (Mathf.Abs(aimX + deltaX) < 30f) {
				aimX += deltaX;
				cannonObject.transform.RotateAround(cannonObject.transform.position, boatboatObject.transform.up, deltaX);	
			}
		}

		// up/down movement
		if (Mathf.Abs(LeftStick.y) > stickDeadzone) {
			float deltaY = aimSpeed * LeftStick.y * Time.deltaTime;
			if (rightSide) {
				// if cannon is on the right side of the ship, up/down movement is reversed
				deltaY = -deltaY;
			}
			if (Mathf.Abs(aimY + deltaY) < 15f) {
				aimY += deltaY;
				cannonObject.transform.RotateAround(cannonObject.transform.position, boatboatObject.transform.forward, deltaY);	
			}				
		}

		// render trajectory prediction lines
		if (canShoot && loaded) {
			UpdateTrajectory();
		}
	}

	private void ShootControls() {
		if (canShoot && loaded && RightBumper) {//&& RightTrigger > 0.9f) {
			Debug.Log("Player " + player.playerNum + " shot " + cannonObject.name);
			cannonballTemp = Instantiate(cannonballPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
			cannonballTemp.GetComponent<CannonBall>().cannonOnRightSide = rightSide;
			smokeTemp = Instantiate(smokeShotPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
			ClearTrajectory();
			loaded = false;
		}
	}

	private void ReloadControls() {
		if (!loaded && !loading && XButton) {
			Debug.Log("Player " + player.playerNum + " reloading " + cannonObject.name);
			loading = true;
		} else if (!loaded && loading) {
			reloadCount += Time.deltaTime;
			if (oldXButton != XButton) {
				reloadCount += 10 * Time.deltaTime;
			}
			if (reloadCount > reloadDuration) {
				Debug.Log("Player " + player.playerNum + " reloaded " + cannonObject.name);
				loaded = true;
				loading = false;
				reloadCount = 0f;
			}
		}
	}

	public override void Dismount() {
		// Call this method to return to default InputController
		if (BButton) {
			player.ResetController();
			UnsetPlayer();
			Destroy(indicatorObject);
		}
	}

	public override void Idle() {
		ClearTrajectory();

		if (needsResetting) {
			aimX = 0f;
			aimY = 0f;
			if (rightSide) {
				cannonObject.transform.localEulerAngles = new Vector3(270f, 270f, 0f);
			} else {
				cannonObject.transform.localEulerAngles = new Vector3(270f, 90f, 0f);
			}
			needsResetting = false;
		}
	}

	private void UpdateTrajectory() {
		const float timePerTrajectorySegment = 0.1f;

		List<Vector3> positions = new List<Vector3>();
		positions.Add(cannonballSpawn.position);
		Vector3 lastPos;

		float speed = cannonballPrefab.GetComponent<CannonBall>().speed;
		Vector3 direction = cannonballSpawn.forward;
		Vector3 velocity = direction * speed;
		bool done = false;
		int j = 0;
		while (!done) {
			lastPos = positions[positions.Count-1];
			Vector3 newPos = lastPos + velocity * timePerTrajectorySegment + 0.5f * Physics.gravity * timePerTrajectorySegment * timePerTrajectorySegment;
			if (newPos.y < -1f) {
				done = true;
			} else {
				j++;
				positions.Add(newPos);
				velocity += Physics.gravity * timePerTrajectorySegment;
			}
		}

		if (lineRenderer != null) {
			lineRenderer.SetVertexCount(positions.Count);
			for (int i = 0; i < positions.Count; i++) {
				lineRenderer.SetPosition(i, positions[i]);
			}
		}
	}

	private void ClearTrajectory() {
		if (lineRenderer != null) {
			lineRenderer.SetVertexCount(0);
		}
	}

	private void HUDText() {
		Camera.main.GetComponent<HUD>().personName = "Left Stick to aim, X to reload";
		Camera.main.GetComponent<HUD>().levelTheme = "Right Trigger to shoot, B to Dismount";
	}

	private void ZoneIndicator() {
		if (!loaded) {
			if (indicatorObject == null) {
				indicatorObject = Instantiate(indicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			}
			float height = player.zone.transform.position.y + indicatorHeight + indicatorAmplitude * Mathf.Sin(Time.time / indicatorPeriod * 2 * Mathf.PI);
			indicatorObject.transform.position = new Vector3(player.zone.transform.position.x, height, player.zone.transform.position.z);
			indicatorObject.transform.LookAt(Camera.main.transform);
		} else {
			if (indicatorObject != null) {
				Destroy(indicatorObject);
			}
		}
	}
}
