using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {
	public int playerNum;
	public InputController controller;
	public InputDevice device;
	public GameObject zone;

	// Use this for initialization
	void Start () {
		ResetController();
	}
	
	// Update is called once per frame
	void Update () {
		if (device == null) {
			Debug.Log("Number of Devices Available: " + InputManager.Devices.Count);
			device = InputManager.Devices[playerNum];
		}
	}

	public bool SetController(InputController newController) {
		// Set this player's controller to new controller
		if (!newController.HasPlayer()) {
			this.controller.UnsetPlayer();
			this.controller = newController;
			this.controller.SetPlayer(this);
			Debug.Log("Player " + playerNum + " mount to " + newController + " successful");
			return true;	// return true if setting controller is successful
		}
		
		Debug.Log("Player " + playerNum + " mount to " + newController + " unsuccessful");
		return false;	// return false if controller already occupied
	}

	public void ResetController() {
		// Reset this player's controller to default controller
		if (this.controller != this.gameObject.GetComponent<DefaultController>()) {
			InputController defaultController = this.gameObject.GetComponent<DefaultController>();
			defaultController.SetPlayer(this);
			this.controller = defaultController;
			Debug.Log("Player " + playerNum + " dismount successful");
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Zone") {
			Debug.Log("Player " + playerNum + " entered zone " + collider.gameObject.name);
			zone = collider.gameObject;
		}
	}

	void OnTriggerStay(Collider collider) {
		if (collider.gameObject.tag == "Zone" && collider.gameObject != zone) {
			Debug.Log("Player " + playerNum + " entered zone " + collider.gameObject.name);
			zone = collider.gameObject;
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Zone") {
			Debug.Log("Player " + playerNum + " left zone " + collider.gameObject.name);
			zone = null;
		}
	}
}
