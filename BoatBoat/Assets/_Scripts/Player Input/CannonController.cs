using UnityEngine;
using InControl;
using System.Collections;

public class CannonController : InputController {
	public float speedFactor;
	public GameObject cannonObject;
	public bool loaded;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
	}
	
	// Move is called once per frame
	public override void Move() {

	}

	public override void Shoot() {
		if (loaded && RightTrigger > 0.9f) {
			Debug.Log("Player " + player.playerNum + " shot " + cannonObject.name);
			loaded = false;
		}
	}

	public override void Reload() {
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
