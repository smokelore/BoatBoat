// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using InControl;
using System.Collections;

public class IntroController : InputController {
	public GameObject indicatorPrefab;
	private GameObject indicatorObject;
	public float indicatorHeight;
	public float indicatorPeriod;
	public float indicatorAmplitude;
	public bool ready;
	private float readyThreshold = 0.2f;
	private float readyCount = 0f;

	// Use this for initialization
	void Start () {
		InputManager.Setup();
		SetPlayer(this.gameObject.GetComponent<Player>());
	}

	// Controls is called once per frame
	public override void Controls() {
		ToggleReady();
	}

	private void ToggleReady() {
		readyCount += Time.deltaTime;
		if (AButton && readyCount > readyThreshold) {
			if (indicatorObject == null) {
				indicatorObject = Instantiate(indicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				ready = true;
			} else {
				Destroy(indicatorObject);
				ready = false;
			}
			readyCount = 0f;
		} else {
			if (indicatorObject != null) {
				float height = player.transform.position.y + indicatorHeight + indicatorAmplitude * Mathf.Sin(readyCount / indicatorPeriod * 2 * Mathf.PI);;
				indicatorObject.transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z);
				indicatorObject.transform.LookAt(Camera.main.transform);
			}
		}
	}
}