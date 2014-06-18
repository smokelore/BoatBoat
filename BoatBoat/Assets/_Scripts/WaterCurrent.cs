using UnityEngine;
using System.Collections;

public class WaterCurrent : MonoBehaviour {
	Vector3 current;

	// Use this for initialization
	void Start () {
		current = new Vector3 (-0.5f*Mathf.Sin(this.transform.rotation.y), 0.0f, 0.5f*Mathf.Cos(this.transform.rotation.y));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		
	}

	void OnTriggerEnter(Collider other) {
		audio.Play();
	}
	void OnTriggerStay(Collider other) {
		//other.gameObject.rigidbody.AddForce(current);
		if (other.attachedRigidbody) {
			other.attachedRigidbody.AddForce(current);
		}
	}
	void OnTriggerExit(Collider other) {
		audio.Stop();
	}
}
