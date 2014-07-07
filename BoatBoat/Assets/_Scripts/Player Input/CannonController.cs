using UnityEngine;
using InControl;
using System.Collections;

public class CannonController : InputController {
	public float speedFactor;
	public GameObject cannonObject;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		//SetPlayer(this.player);
	}
	
	// Move is called once per frame
	public override void Move () {

	}

	public override void Dismount() {
		// Call this method to return to default InputController
		if (BButton) {
			player.ResetController();
			UnsetPlayer();
		}
	}
}
