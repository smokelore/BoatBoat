using UnityEngine;
using InControl;
using System.Collections;

public class CannonController : InputController {
	public bool rightSide;
	public GameObject boatboatObject;
	public GameObject cannonObject;
	public GameObject cannonballPrefab;
	public Transform cannonballSpawn;
	public GameObject cannonballTemp;
	public bool loaded, canShoot;
	public float stickDeadzone;
	public float aimSpeed;
	public float aimX, aimY;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		cannonballSpawn = cannonObject.transform.Find("chamber/spawn");
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
	}

	private void ShootControls() {
		if (canShoot && loaded && RightTrigger > 0.9f) {
			Debug.Log("Player " + player.playerNum + " shot " + cannonObject.name);
			cannonballTemp = Instantiate(cannonballPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
			loaded = false;
		}
	}

	private void ReloadControls() {
		if (!loaded && XButton) {
			Debug.Log("Player " + player.playerNum + " reloaded " + cannonObject.name);
			loaded = true;
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
