using UnityEngine;
using System.Collections;

public class landCollideSoundController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.name == "Ship"){
			audio.volume = 0.5f * collision.gameObject.GetComponent<forceController>().curSpeed / collision.gameObject.GetComponent<forceController>().maxSpeed;
			audio.Play ();
		}
	}
}
