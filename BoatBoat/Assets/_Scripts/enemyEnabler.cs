// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;

public class enemyEnabler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Transform child in this.transform){
			if(Vector3.Distance (child.transform.position, Camera.main.transform.position) > 125){
				child.gameObject.SetActive (false);
			}else{
				child.gameObject.SetActive (true);
			}
		}
	}
}
