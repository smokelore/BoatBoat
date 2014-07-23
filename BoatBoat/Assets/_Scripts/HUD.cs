using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;
	public string levelTheme;
	public GameObject hudGUI;
	public bool winState;
	public Texture healthBarTexture;

	// Use this for initialization
	void Start () {
		hudGUI.guiText.text = personName + "\n" + levelTheme;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Alpha1)) {
			Application.LoadLevel("playtestScene");
		}

		if (winState){
			personName = "You Win!";
			levelTheme = "Press 1 to Restart";
		}

		hudGUI.guiText.text = personName + "\n" + levelTheme;
	}

	void OnGUI() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject player = GameObject.FindWithTag("Player");

		foreach (GameObject enemy in enemies) {
			Vector3 healthBarWorldPosition = enemy.transform.position + Vector3.up * 0f;
			float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
			if (distance < 50f) {
				if (distance > 40f) {
					distance = 40f;
				}
				int length = (int)(distance/40) * 100;
				int width = (int)(distance/40) * 20;

				Vector2 healthBarScreenPosition = Camera.main.WorldToScreenPoint(healthBarWorldPosition);
				//GUI.DrawTexture(new Rect(healthBarWorldPosition.x - (int)(length/2), healthBarWorldPosition.y + (int)(width/2), length, width), healthBarTexture, ScaleMode.StretchToFill, true, 0);
			}
		}
	}
}