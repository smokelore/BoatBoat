using UnityEngine;
using InControl;
using System.Collections;

public class DefaultController : InputController {
	public GameObject boatboatObject;
	public Collider WalkableArea;
	public float speedFactor;
	public GameObject zone;
	public GameObject indicatorPrefab;
	private GameObject indicatorObject;
	public float indicatorHeight;
	public float indicatorPeriod;
	public float indicatorAmplitude;
	public bool disableControls;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		SetPlayer(this.gameObject.GetComponent<Player>());
	}

	// Controls is called once per frame
	public override void Controls() {
		MovementControls();
		ZoneIndicator();

		//HUDText();
	}

	private void MovementControls() {
		this.transform.rotation = WalkableArea.transform.rotation;
		if (LeftStick.magnitude > 0) {
			Vector3 newForward = Camera.main.transform.forward;
			newForward = new Vector3(newForward.x, 0, newForward.z).normalized;

			Vector3 newRight = Camera.main.transform.right;
			newRight = new Vector3(newRight.x, 0, newRight.z).normalized;


			Vector3 movement = newRight * LeftStick.x * speedFactor + newForward * LeftStick.y * speedFactor;
			Vector3 newLocalPosition = this.transform.localPosition + movement * Time.deltaTime; 
			
			Vector3 newWorldPosition = this.transform.position + this.transform.lossyScale.x * movement * Time.deltaTime;
			//Debug.DrawLine(this.transform.position, newWorldPosition, Color.red);
			if (newLocalPosition.x < 10f && newLocalPosition.x > -10f && newLocalPosition.z < 30f && newLocalPosition.z > -35f) {
				this.transform.localPosition = newLocalPosition;
			}
		}
	}

	public override void Mount() {
		if (player.zone != null && AButton) {
			if (this.player.SetController(player.zone.GetComponent<InputController>())) {
				this.player = null;
				if (indicatorObject != null) {
					Destroy(indicatorObject);
				}
			}
		}
	}

	private void ZoneIndicator() {
		if (player.zone != null && player.zone.GetComponent<InputController>().player == null) {
			if (indicatorObject == null) {
				indicatorObject = Instantiate(indicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			}
			float height = player.zone.transform.position.y + indicatorHeight + indicatorAmplitude * Mathf.Sin(Time.time / indicatorPeriod * 2 * Mathf.PI);
			indicatorObject.transform.position = new Vector3(player.zone.transform.position.x, height, player.zone.transform.position.z);
			indicatorObject.transform.LookAt(Camera.main.transform);
		} else {
			if (indicatorObject != null) {
				Destroy(indicatorObject);
			}
		}
	}

	private void HUDText() {
		Camera.main.GetComponent<HUD>().personName = "Left Stick to move, A to mount";
		Camera.main.GetComponent<HUD>().levelTheme = "Red = cannon, Blue = oars, Green = spotter";
	}
}