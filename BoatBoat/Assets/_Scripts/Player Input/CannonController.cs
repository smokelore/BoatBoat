using UnityEngine;
using InControl;
using System.Collections;

public class CannonController : InputController {
	public float speedFactor;
	public GameObject cannonObject;
	public GameObject cannonballPrefab;
	public Transform cannonballSpawn;
	public GameObject cannonballTemp;
	public bool loaded, canShoot;

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

		ShootControls();
		ReloadControls();
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
