using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture logo;
	public Texture pressStart;
	// Use this for initialization
	
	public int textHeight;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel("playtestScene");
		}
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width/3, 16, Screen.width/3, textHeight), logo, ScaleMode.StretchToFill, true, 0);
		GUI.DrawTexture(new Rect(Screen.width/3, Screen.height-(textHeight+16), Screen.width/3, textHeight), pressStart, ScaleMode.StretchToFill, true, 0);
	}
}
