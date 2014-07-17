using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class forceController : MonoBehaviour {
	private Vector3 curPos;
	
	public float forceFactor;
	public float rotFactor, curRotFactor;
	public float maxSpeed, curSpeed;
	public float maxTurn, curMaxTurn;
	public float idleDrag;
	private bool movingForward, movingBackward;
	public Cloth sailCloth;
	public Vector3 clothAcceleration;
	public float fakeVerticalAxis, fakeHorizontalAxis;
	public List<OarController> allOars = new List<OarController>();

	void Start () {
	
	}
	
	void Update() {
		GetAllRows();
	}

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

		if (fakeVerticalAxis > 0) {
			// ACCELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z < maxSpeed) {
				this.rigidbody.AddForce(this.transform.forward * forceFactor);
			}
		} else if (fakeVerticalAxis < 0) {
			// DECELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z > -maxSpeed/2) {
				this.rigidbody.AddForce(this.transform.forward * -forceFactor/2);
			}
		} else {
			this.rigidbody.drag = idleDrag;
		}

		if (fakeHorizontalAxis > 0) {
			// RIGHT TURN
			if (movingForward && rigidbody.angularVelocity.y < curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * curRotFactor);
			} else if (movingBackward && rigidbody.angularVelocity.y > -curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * -curRotFactor);
			}
		} else if (fakeHorizontalAxis < 0) {
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

		clothAcceleration = new Vector3(rigidbody.angularVelocity.y / maxTurn * 100f, 0f, 5f - localVelocity.z / maxSpeed * 5f);//curMaxTurn/maxTurn, 0);
		sailCloth.externalAcceleration = clothAcceleration;
	}

	public void GetAllRows() {
		float rightTotal = 0f, leftTotal = 0f;
		foreach (OarController oc in allOars) {
			if (oc.player != null) {
				rightTotal += oc.getRightRowAmount();
				leftTotal += oc.getLeftRowAmount();
			}
		}

		fakeVerticalAxis = (rightTotal+leftTotal) / (allOars.Count*2);
		fakeHorizontalAxis = (leftTotal / allOars.Count) - (rightTotal / allOars.Count);
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("Boat Collision: " + collision.gameObject.name);
	}
}