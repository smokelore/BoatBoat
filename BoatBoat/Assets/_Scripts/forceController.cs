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

		// if (localVelocity.z > maxSpeed) {
		// 	Debug.Log("slow it down");
		// 	this.rigidbody.velocity = this.rigidbody.velocity.normalized * maxSpeed;
		// }

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
				this.rigidbody.AddForce(new Vector3(this.transform.forward.x, 0f, this.transform.forward.z).normalized * forceFactor);
			}
		} else if (fakeVerticalAxis < 0) {
			// DECELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z > -maxSpeed) {
				this.rigidbody.AddForce(new Vector3(this.transform.forward.x, 0f, this.transform.forward.z).normalized * -forceFactor/2);
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

		Vector3 newForward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z).normalized;
		float planeVelocity = new Vector3(this.rigidbody.velocity.x, 0f, this.rigidbody.velocity.z).magnitude;
		if (movingForward) {
			// if moving forward, redirect all velocity forward after turning
			this.rigidbody.velocity = planeVelocity * newForward;
		} else if (movingBackward) {
			// if moving backward, redirect all velocity backward after turning
			this.rigidbody.velocity = -planeVelocity * newForward;
		}

		clothAcceleration = new Vector3(rigidbody.angularVelocity.y / maxTurn * 50f, 0f, 4f - localVelocity.z / maxSpeed * 4f);//curMaxTurn/maxTurn, 0);
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