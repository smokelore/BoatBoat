using UnityEngine;
using InControl;
using System.Collections;

public class DefaultController : InputController {
	public GameObject boatboatObject;
	public Collider WalkableArea;
	public float speedFactor;
	public GameObject zone;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		SetPlayer(this.gameObject.GetComponent<Player>());
	}

	// Controls is called once per frame
	public override void Controls() {
		MovementControls();
		HUDText();
	}

	private void MovementControls() {
		this.transform.rotation = WalkableArea.transform.rotation;
		if (LeftStick.magnitude > 0) {
			Vector3 newForward = Camera.main.transform.forward;
			newForward = new Vector3(newForward.x, 0, newForward.z).normalized;

			Vector3 newRight = Camera.main.transform.right;
			newRight = new Vector3(newRight.x, 0, newRight.z).normalized;


			Vector3 movement = newRight * LeftStick.x * speedFactor + newForward * LeftStick.y * speedFactor;
			Vector3 newLocation = this.transform.position + movement*Time.deltaTime; 
			
			newLocation.y = boatboatObject.transform.position.y + 0.3f; 	// FIX THIS MORE BETTER
			
			//Debug.Log("// " + newLocation);
			if (WalkableArea.bounds.Contains(newLocation)) {
				this.transform.position = newLocation;
			}
		}
	}

	public override void Mount() {
		if (player.zone != null && AButton) {
			if (this.player.SetController(player.zone.GetComponent<InputController>())) {
				this.player = null;
			}
		}
	}

	private void HUDText() {
		Camera.main.GetComponent<HUD>().personName = "Left Stick to move, A to mount";
		Camera.main.GetComponent<HUD>().levelTheme = "Red = cannon, Blue = oars, Green = spotter";
	}
}