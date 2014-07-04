using UnityEngine;
using InControl;
using System.Collections;

public class DefaultController : InputController {
	public GameObject BoatBoat;
	public Collider WalkableArea;
	public float speedFactor;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		SetPlayer(this.player);
	}
	
	// Update is called once per frame
	public override void Move () {
		this.transform.rotation = WalkableArea.transform.rotation;
		Vector3 movement = WalkableArea.transform.right * LeftStick.x * speedFactor + WalkableArea.transform.forward * LeftStick.y * speedFactor;
		Vector3 newLocation = this.transform.position + movement*Time.deltaTime; 
		if (WalkableArea.bounds.Contains(newLocation)) {
			this.transform.position = newLocation;
		}
	}
}
