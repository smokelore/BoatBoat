using UnityEngine;
using InControl;
using System.Collections;

public class DefaultController : InputController {
	public GameObject BoatBoat;
	public Collider WalkableArea;
	public float speedFactor;
	public GameObject zone;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		SetPlayer(this.gameObject.GetComponent<Player>());
	}
	
	// Move is called once per frame
	public override void Move () {
		this.transform.rotation = WalkableArea.transform.rotation;
		if (LeftStick.magnitude > 0) {
			Vector3 movement = WalkableArea.transform.right * LeftStick.x * speedFactor + WalkableArea.transform.forward * LeftStick.y * speedFactor;
			Vector3 newLocation = this.transform.position + movement*Time.deltaTime; 
			Debug.Log("// " + newLocation);
			if (WalkableArea.bounds.Contains(newLocation)) {
				this.transform.position = newLocation;
			}
		}
	}

	public override void Mount() {
		if (player.zone != null && AButton) {
			this.player.SetController(player.zone.GetComponent<InputController>());
			this.player = null;
		}
	}
}