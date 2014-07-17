﻿using UnityEngine;
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

	// Controls is called once per frame
	public override void Controls() {
		MovementControls();
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
			//Debug.Log("// " + newLocation);
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