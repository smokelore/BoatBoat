using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;
	public string levelTheme;
	public GameObject hudGUI;
	public bool winState;
	public Texture healthBarTexture;
	//public Player[] players = new Player[2];

	// Use this for initialization
	void Start () {
		hudGUI.guiText.text = personName + "\n" + levelTheme;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Alpha1)) {
			Application.LoadLevel("main");
		}

		if (winState){
			personName = "You Win!";
			levelTheme = "Press 1 to Exit";
		}

		// hudGUI.guiText.text = personName + "\n" + levelTheme;
		// string temp = "";
		// foreach(Player player in players) {
		// 	if (player.controller != null) {
		// 		temp += player.name + " " + player.controller.RightBumper + " ";
		// 	}
		// }

		// hudGUI.guiText.text = temp;
	}

	void OnGUI() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject player = GameObject.FindWithTag("Player");

		int length, width;
		foreach (GameObject enemy in enemies) {
			// Draw enemy health bars
			if (enemy.transform.Find("Main/Mast").renderer.isVisible) {
				GUI.DrawTexture(getHealthRect(enemy.GetComponent<Ship>()), healthBarTexture, ScaleMode.StretchToFill, true, 0);
			}
		}

		// Draw player health bars
		GUI.DrawTexture(getHealthRect(player.GetComponent<Ship>()), healthBarTexture, ScaleMode.StretchToFill, true, 0);
	}

	private Rect getHealthRect(Ship ship) {
		float distance = Vector3.Distance(Camera.main.transform.position, ship.transform.position);
		if (distance < 50f) {
			distance = Mathf.Clamp(distance, 10, 50);
			Vector3 healthBarWorldPosition = ship.transform.position + Vector3.up * (2.5f + (distance-10)/25);
			int length = (int)(Mathf.Pow(2-distance/40,2) * ship.health) ;
			int width = (int)(Mathf.Pow(2-distance/40,2) * 7.5f);

			Vector2 healthBarScreenPosition = Camera.main.WorldToScreenPoint(healthBarWorldPosition);
			return new Rect(healthBarScreenPosition.x - (int)(length/2), Screen.height - healthBarScreenPosition.y + (int)(width/2), length, width);
		}
		return new Rect(-100,-100,0,0);
	}
}