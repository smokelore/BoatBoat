using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;
	public string levelTheme;
	public GameObject hudGUI;
	public bool winState;

	// Use this for initialization
	void Start () {
		hudGUI.guiText.text = personName + "\n" + levelTheme;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Alpha1)) {
			Application.LoadLevel("playtestScene");
		} else if (Input.GetKeyUp(KeyCode.Alpha2)) {
			Application.LoadLevel("playtestScene");
		} else if (Input.GetKeyUp(KeyCode.Alpha3)) {
			Application.LoadLevel("playtestScene");
		} else if (Input.GetKeyUp(KeyCode.Alpha4)) {
			Application.LoadLevel("playtestScene");
		} 

		if(winState){
			personName = "You Win! - Press 1 to Restart!";
			hudGUI.guiText.text = personName;
		}
	}
}
