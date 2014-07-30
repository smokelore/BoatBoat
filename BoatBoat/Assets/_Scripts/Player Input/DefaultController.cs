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
		//if (LeftStick.magnitude > 0) {
			Vector3 camForward = Camera.main.transform.forward;
			camForward = new Vector3(camForward.x, 0, camForward.z).normalized;
			Vector3 newForward = Vector3.Cross(Vector3.up, Vector3.Cross(camForward, Vector3.up));

			Vector3 camRight = Camera.main.transform.right;
			camRight = new Vector3(camRight.x, 0, camRight.z).normalized;
			Vector3 newRight = Vector3.Cross(Vector3.up, Vector3.Cross(camRight, Vector3.up));

			Vector3 movement = newRight * LeftStick.x * speedFactor + newForward * LeftStick.y * speedFactor;
			Vector3 newLocalPosition = this.transform.localPosition + movement * Time.deltaTime; 
			
			//Vector3 newWorldPosition = this.transform.position + this.transform.lossyScale.x * movement * Time.deltaTime;
			// Debug.DrawLine(player.transform.position, player.transform.position + Vector3.up * 10f, Color.blue);
			// Debug.DrawLine(player.transform.position, player.transform.position + camForward * 10f, Color.red);
			// Debug.DrawLine(player.transform.position, player.transform.position + newRight * 10f, Color.green);
			// Debug.DrawLine(player.transform.position, player.transform.position + newForward * 10f, Color.yellow);
			if (newLocalPosition.x < 10f && newLocalPosition.x > -10f && newLocalPosition.z < 30f && newLocalPosition.z > -35f) {
				this.transform.localPosition = newLocalPosition;
			}
		//}
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