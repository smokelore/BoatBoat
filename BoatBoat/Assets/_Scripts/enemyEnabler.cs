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
