using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;
	public string levelTheme;
	public GameObject hudGUI;

	// Use this for initialization
	void Start () {
		hudGUI.guiText.text = personName + "\n" + levelTheme;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
