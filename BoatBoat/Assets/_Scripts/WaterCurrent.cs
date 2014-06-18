﻿using UnityEngine;
using System.Collections;

public class WaterCurrent : MonoBehaviour {
	Vector3 current;

	// Use this for initialization
	void Start () {
		current = this.transform.forward * 2f;
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
		if (other.attachedRigidbody != null && other.gameObject.name != "River Current" && other.gameObject.name != "Cube") {
			other.gameObject.rigidbody.AddForce(current);
			Vector3 oldRot = other.gameObject.transform.forward;
			Vector3 targetRot = this.gameObject.transform.forward;
			Vector3 newRot = Vector3.RotateTowards(oldRot, targetRot, 1*Time.deltaTime, 0.0f);
			other.gameObject.rigidbody.AddTorque(Vector3.up * AngleSigned(oldRot, targetRot, Vector3.up)/100);
			//other.gameObject.transform.rotation = Quaternion.LookRotation(newRot);
			//Debug.Log();
		}
	}
	void OnTriggerExit(Collider other) {
		audio.Stop();
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}