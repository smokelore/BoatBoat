using UnityEngine;
using System.Collections;

public class forceController : MonoBehaviour {
	private Vector3 curPos;
	
	public float forceFactor;
	public float rotFactor, curRotFactor;
	public float maxSpeed, curSpeed;
	public float maxTurn, curMaxTurn;
	public float idleDrag;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 localVelocity = this.transform.InverseTransformDirection(this.rigidbody.velocity);
		curSpeed = rigidbody.velocity.magnitude;

		// turning speed limited when going to slow or going too fast (max is at 3/5 of top speed)
		if (rigidbody.velocity.magnitude <= maxSpeed) {
			curMaxTurn = maxTurn * Mathf.Sin(rigidbody.velocity.magnitude/maxSpeed * (Mathf.PI/2) * 5/3);
		} else {
			curMaxTurn = maxTurn * Mathf.Sin(maxSpeed/maxSpeed * (Mathf.PI/2) * 5/3);
		}
		curRotFactor = curMaxTurn/maxTurn * rotFactor;
		
		if (Input.GetAxis("Vertical") > 0) {
			// ACCELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z < maxSpeed) {
				this.rigidbody.AddForce(this.transform.forward * forceFactor);
			}
		} else if (Input.GetAxis("Vertical") < 0) {
			// DECELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z > -maxSpeed/2) {
				this.rigidbody.AddForce(this.transform.forward * -forceFactor/2);
			}
		} else {
			this.rigidbody.drag = idleDrag;
		}

		if (Input.GetAxis("Horizontal") > 0) {
			// RIGHT TURN
			if (rigidbody.angularVelocity.y < curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * curRotFactor);
			}
		} else if (Input.GetAxis("Horizontal") < 0) {
			// LEFT TURN
			if (rigidbody.angularVelocity.y > -curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * -curRotFactor);
			}
		}

		if (localVelocity.z > 0) {
			// if moving forward, redirect all velocity forward after turning
			this.rigidbody.velocity = this.transform.forward * this.rigidbody.velocity.magnitude;
		} else if (localVelocity.z < 0) {
			// if moving backward, redirect all velocity backward after turning
			this.rigidbody.velocity = this.transform.forward * -this.rigidbody.velocity.magnitude;
		}
	}
}