using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		audio.Play();
	}
}
