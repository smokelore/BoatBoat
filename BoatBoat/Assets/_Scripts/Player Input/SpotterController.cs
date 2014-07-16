using UnityEngine;
using InControl;
using System.Collections;

public class SpotterController : InputController {
	public GameObject BoatBoat;
	public float speedFactor;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
	}
	
	// Controls is called once per frame
	public override void Controls() {

	}

	public override void Dismount() {
		// Call this method to return to default InputController
		if (BButton) {
			player.ResetController();
			UnsetPlayer();
		}
	}
}
