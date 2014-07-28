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
			float distance = Vector3.Distance(Camera.main.transform.position, enemy.transform.position);
			if (distance < 50f) {
				distance = Mathf.Clamp(distance, 10, 50);
				Vector3 healthBarWorldPosition = enemy.transform.position + Vector3.up * (1.5f + (distance-10)/20);
				int length = (int)(Mathf.Pow(2-distance/40,2) * enemy.GetComponent<Ship>().health) ;
				int width = (int)(Mathf.Pow(2-distance/40,2) * 10);

				Vector2 healthBarScreenPosition = Camera.main.WorldToScreenPoint(healthBarWorldPosition);
				Debug.Log((healthBarScreenPosition.x - (int)(length/2)) + " " + (healthBarScreenPosition.y - (int)(width/2)));
				GUI.DrawTexture(new Rect(healthBarScreenPosition.x - (int)(length/2), Screen.height - healthBarScreenPosition.y + (int)(width/2), length, width), healthBarTexture, ScaleMode.StretchToFill, true, 0);
			}
		}
	}
}