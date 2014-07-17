﻿using UnityEngine;
using InControl;
using System.Collections;

public class InputController : MonoBehaviour {
	public Player player;

	public Vector2 LeftStick;
	public Vector2 RightStick;
	public bool AButton;
	public bool BButton;
	public bool XButton;
	public bool YButton;
	public float LeftTrigger;
	public float RightTrigger;
	public bool LeftBumper;
	public bool RightBumper;

	void Start() {
		InputManager.Setup();
		//SetPlayer(1);
	}

	void Update() {
		if (HasPlayer()) {
			InputManager.Update();
			// Update input values
			LeftStick = new Vector2(player.device.LeftStickX, player.device.LeftStickY);
			RightStick = new Vector2(player.device.RightStickX, player.device.RightStickY);
			AButton = player.device.GetControl(InputControlType.Action1).IsPressed;
			BButton = player.device.GetControl(InputControlType.Action2).IsPressed;
			XButton = player.device.GetControl(InputControlType.Action3).IsPressed;
			YButton = player.device.GetControl(InputControlType.Action4).IsPressed;
			LeftTrigger = player.device.GetControl(InputControlType.LeftTrigger).Value;
			RightTrigger = player.device.GetControl(InputControlType.RightTrigger).Value;
			LeftBumper = player.device.GetControl(InputControlType.LeftBumper).IsPressed;
			RightBumper = player.device.GetControl(InputControlType.RightBumper).IsPressed;

			Controls();

			Mount();
			Dismount();
		} else {
			Idle();
		}
	}

	public virtual void Idle() {
		// Override this method with stuff that should be run when the controller is not in use
	}

	public virtual void Controls() {
		// Override this method with all player input code
	}

	public virtual void Mount() {
		// Override this method with mount code (for taking control of other inputs, eg. cannons, oars)

	}

	public virtual void Dismount() {
		// Override this method with dismount code (for returning to default control)
		
	}

	public void SetPlayer(Player newPlayer) {
		Debug.Log("Number of Devices Available: " + InputManager.Devices.Count);
		if (newPlayer != null && newPlayer.playerNum <= 4 && newPlayer.playerNum >= 0) {
			this.player = newPlayer;
		} else {
			LeftStick = Vector2.zero;
			RightStick = Vector2.zero;
			AButton = false;
			BButton = false;
			XButton = false;
			YButton = false;
			LeftTrigger = 0f;
			RightTrigger = 0f;
			LeftBumper = false;
			RightBumper = false;

			this.player = null;
		}
	}

	public void UnsetPlayer() {
		SetPlayer(null);
	}

	public bool HasPlayer() {
		return (player != null && (player.playerNum <= 4 && player.playerNum >= 0) && player.device != null);
	}
}
