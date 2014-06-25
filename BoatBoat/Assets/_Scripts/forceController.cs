using UnityEngine;
using System.Collections;

public class forceController : MonoBehaviour {
	private Vector3 curPos;
	
	public float forceFactor;
	public float rotFactor, curRotFactor;
	public float maxSpeed, curSpeed;
	public float maxTurn, curMaxTurn;
	public float idleDrag;
	private bool movingForward, movingBackward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 localVelocity = this.transform.InverseTransformDirection(this.rigidbody.velocity);
		curSpeed = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude;

		// turning speed limited when going to slow or going too fast (max is at 3/5 of top speed)
		if (curSpeed > maxSpeed) {
			curMaxTurn = maxTurn * Mathf.Sin(maxSpeed/maxSpeed * (Mathf.PI/2) * 5/3);
		} else {
			curMaxTurn = maxTurn * Mathf.Sin(curSpeed/maxSpeed * (Mathf.PI/2) * 5/3);
		}
		curRotFactor = curMaxTurn/maxTurn * rotFactor;
		
		movingForward = (localVelocity.z >= 0);
		movingBackward = !movingForward;

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
			if (movingForward && rigidbody.angularVelocity.y < curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * curRotFactor);
			} else if (movingBackward && rigidbody.angularVelocity.y > -curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * -curRotFactor);
			}
		} else if (Input.GetAxis("Horizontal") < 0) {
			// LEFT TURN
			if (movingForward && rigidbody.angularVelocity.y > -curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * -curRotFactor);
			} else if (movingBackward && rigidbody.angularVelocity.y < curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * curRotFactor);
			}
		}

		Vector2 planeVelocity = new Vector2(this.rigidbody.velocity.x, this.rigidbody.velocity.z);
		if (movingForward) {
			// if moving forward, redirect all velocity forward after turning
			this.rigidbody.velocity = this.transform.forward * planeVelocity.magnitude;
		} else if (movingBackward) {
			// if moving backward, redirect all velocity backward after turning
			this.rigidbody.velocity = this.transform.forward * -planeVelocity.magnitude;
		}
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("Boat Collision: " + collision.gameObject.name);
	}
}