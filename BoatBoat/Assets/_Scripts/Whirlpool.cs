using UnityEngine;
using System.Collections;

public class Whirlpool : MonoBehaviour {
	public bool clockwise;

	// Use this for initialization
	void Start () {
		this.gameObject.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider collision) {
		if (collision.gameObject.name == "Ship") {
			forceController fc = collision.gameObject.GetComponent<forceController>();
			Vector3 boatToWhirl = this.transform.position - collision.gameObject.transform.position;//(new Vector3(collision.gameObject.transform.position.x, this.transform.position.y, collision.gameObject.transform.position.z) - this.transform.position);
			boatToWhirl = new Vector3(boatToWhirl.x, 0, boatToWhirl.z);
			float whirlRadius = this.transform.lossyScale.x;
			collision.gameObject.rigidbody.AddForce(boatToWhirl.normalized * Mathf.Sin((whirlRadius - boatToWhirl.magnitude)/whirlRadius * Mathf.PI/2) * fc.forceFactor * 3);
			if (clockwise) {
				collision.gameObject.rigidbody.AddTorque(Vector3.up * fc.rotFactor/(whirlRadius - boatToWhirl.magnitude) * 20);	
			} else {
				collision.gameObject.rigidbody.AddTorque(-Vector3.up * fc.rotFactor/(whirlRadius - boatToWhirl.magnitude) * 20);
			}
			
			Debug.DrawLine(this.transform.position, this.transform.position + boatToWhirl.normalized * 5);
		}
	}
}