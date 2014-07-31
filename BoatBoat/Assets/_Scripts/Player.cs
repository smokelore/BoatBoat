// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {
	public int playerNum;
	public int actualPlayerNum;
	public InputController defaultController;
	public InputController controller;
	public InputDevice device;
	public GameObject zone;
	private int oldNumDevices;
	private bool disabled = true;

	// Use this for initialization
	void Start () {
		//ResetController();
	}
	
	// Update is called once per frame
	void Update () {
		if (device == null) {
			//Debug.Log(playerNum + ", " + actualPlayerNum + ": null device");
			actualPlayerNum = FindActualPlayerNum();
			if (actualPlayerNum < InputManager.Devices.Count && actualPlayerNum > -1) {
				Enable();
			} else {
				Disable();
			}
		}

		if (InputManager.Devices.Count != oldNumDevices) {
			Debug.Log("Number of Devices Available: " + InputManager.Devices.Count);
			Disable();
		}

		oldNumDevices = InputManager.Devices.Count;
	}

	public int FindActualPlayerNum() {
		int i = 0;
		int found = 0;
		while (i < InputManager.Devices.Count) {
			if ((InputManager.Devices[i] as UnityInputDevice).Profile.IsKnown) {
				if (found == playerNum) {
					return i;
				}

				found++;
			}
			i++;
		}

		return -1;
	}

	public bool SetController(InputController newController) {
		// Set this player's controller to new controller
		if (!newController.HasPlayer()) {
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
		//if (this.controller != this.gameObject.GetComponent<DefaultController>()) {
			this.controller = defaultController;
			this.controller.SetPlayer(this);
			Debug.Log("Player " + playerNum + " reset to " + defaultController + " successful");
		//}
	}

	public void Disable() {
		if (!disabled) {
			device = null;
			this.controller.UnsetPlayer();
			this.renderer.enabled = false;
			this.collider.enabled = false;
			zone = null;
			disabled = true;
		}
	}

	public void Enable() {
		if (disabled) {
			device = InputManager.Devices[actualPlayerNum];
			ResetController();
			this.controller.enabled = false;
			this.renderer.enabled = true;
			this.collider.enabled = true;
			this.controller.enabled = true;
			disabled = false;
			Debug.Log(playerNum + ", " + actualPlayerNum + ": " + (device as UnityInputDevice).Profile);
		}
	}

	public bool isDisabled() {
		return disabled;
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
