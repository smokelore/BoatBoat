using UnityEngine;
using InControl;
using System.Collections;

public class InputController : MonoBehaviour {
	public int player = -1;
	private InputDevice device;
	public string deviceName;

	public Vector2 LeftStick;
	public Vector2 RightStick;
	public bool AButton;
	public bool BButton;
	public bool XButton;
	public bool YButton;
	public float LeftTrigger;
	public float RightTrigger;

	void Start() {
		InputManager.Setup();

		//SetPlayer(1);
	}

	void Update() {
		if (HasPlayer()) {
			InputManager.Update();
			deviceName = device.Name;
			LeftStick = new Vector2(device.LeftStickX, device.LeftStickY);
			RightStick = new Vector2(device.RightStickX, device.RightStickY);
			AButton = device.GetControl(InputControlType.Action1).IsPressed;
			BButton = device.GetControl(InputControlType.Action2).IsPressed;
			XButton = device.GetControl(InputControlType.Action3).IsPressed;
			YButton = device.GetControl(InputControlType.Action4).IsPressed;
			LeftTrigger = device.GetControl(InputControlType.LeftTrigger).Value;
			RightTrigger = device.GetControl(InputControlType.RightTrigger).Value;
			Move();
		} else {
			LeftStick = Vector2.zero;
			RightStick = Vector2.zero;
			AButton = false;
			BButton = false;
			XButton = false;
			YButton = false;
			LeftTrigger = 0f;
			RightTrigger = 0f;
		}
	}

	public virtual void Move() {

	}

	public void SetPlayer(int player) {
		Debug.Log(InputManager.Devices.Count);
		if (player < 4 && player >= 0) {
			this.player = player;
			this.device = InputManager.Devices[player];
		} else {
			this.player = -1;
			this.device = null;
		}
	}

	public void ResetPlayer() {
		SetPlayer(-1);
	}

	public bool HasPlayer() {
		return ((this.player < 4 && this.player >= 0) && this.device != null);
	}
}
