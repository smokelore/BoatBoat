// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyController : MonoBehaviour {
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
	
	public float viewDistance;
	private GameObject playerObject;
	private Vector3 target;
	public NavMeshAgent navagent;
	private List<Vector3> waypoints = new List<Vector3>();

	public List<GameObject> patrolpoints;
	public int patrolIndex;

	public bool isStuck;

	public float stuckDuration;
	public float stuckTime = 0f;
	//public TerrainFeeler terrainfeeler;

	public enum State {
		PATROL,
		CHASE,
		ATTACK,
		RETREAT
	}

	public State state, lastState;

	// Use this for initialization
	void Start () {
		SetState(State.PATROL);
	}

	void Update() {
		switch(state) {
			case State.PATROL:
				if (SpotPlayer()) {
					SetState(State.CHASE);
				}
				GetWaypoints();
				SetFakeAxes();
				break;
			case State.CHASE:
				if (!SpotPlayer()) {
					SetState(State.PATROL);
				}

				if (NearTarget(playerObject.transform.position, 15f)) {
					SetState(State.ATTACK);
				}

				GetWaypoints();
				SetFakeAxes();
				break;
			case State.ATTACK:
				if (!NearTarget(playerObject.transform.position, 15f)) {
					SetState(State.CHASE);
				}

				GetWaypoints();
				SetFakeAxes();
				break;
			case State.RETREAT:

				break;
			default:

				break;
		}

		if (IsStuck()) {
			fakeVerticalAxis = -fakeVerticalAxis;
			fakeHorizontalAxis = -fakeHorizontalAxis;
		}
		
	}

	public void SetState(State newState) {
		lastState = state;
		state = newState;
	}

	public void GetWaypoints() {
		navagent.transform.position = new Vector3(this.transform.position.x, -4, this.transform.position.z);
		navagent.SetDestination(GetTarget());
		waypoints.Clear();
		
		foreach (Vector3 wp in navagent.path.corners) {
			waypoints.Add(wp);
		}
	}

	public void SetFakeAxes() {
		if (waypoints.Count > 1) {
			Vector3 toWaypoint = waypoints[1] - navagent.transform.position;
			float angle = AngleSigned(navagent.transform.forward, toWaypoint, Vector3.up);
			fakeHorizontalAxis = angle/180f;

			float temp = Mathf.Abs(angle/180f);
			float targetSpeed;

			if (temp <= 0.4f) {
				targetSpeed = (1 - temp) * maxSpeed;//((2/5 - temp) + 3/5) * maxSpeed;
				//Debug.Log("((2/5 - " + temp + ") + 3/5) * " + maxSpeed + " = " + targetSpeed);
			} else {
				targetSpeed = (13 - 10*temp)/15 * maxSpeed;//((1f - (temp - 2/5)/(3/5))*(2/5) + 1/5) * maxSpeed;
			}

			fakeVerticalAxis = (targetSpeed - curSpeed)/maxSpeed;
		}
	}

	public Vector3 GetTarget() {
		switch(state) {
			case State.PATROL:
				if (patrolpoints.Count > 0) {
					if (NearTarget(patrolpoints[patrolIndex].transform.position, 10f)) {
						patrolIndex++;
						if (patrolIndex >= patrolpoints.Count) {
							patrolIndex = 0;
						}
					}
					return patrolpoints[patrolIndex].transform.position;
				}
				break;
			case State.CHASE:
				return playerObject.transform.position;
			case State.ATTACK:
				Vector3 oldDestination = navagent.destination;
				Vector3 rightVector = playerObject.transform.right;
				rightVector = new Vector3(rightVector.x, 0f, rightVector.z).normalized;
				Vector3 forwardVector = playerObject.rigidbody.velocity;
				forwardVector = new Vector3(forwardVector.x, 0f, forwardVector.z);

				Vector3 rightPos = playerObject.transform.position + rightVector * 5f + forwardVector * 2f;
				Vector3 leftPos = playerObject.transform.position - rightVector * 5f + forwardVector * 2f;
				rightPos = new Vector3(rightPos.x, navagent.transform.position.y, rightPos.z);
				leftPos = new Vector3(leftPos.x, navagent.transform.position.y, leftPos.z);

				bool rightValid = false, leftValid = false;

				navagent.SetDestination(rightPos);
				//Debug.Log("right " + navagent.pathStatus);
				if (navagent.pathStatus != NavMeshPathStatus.PathInvalid) {
					rightValid = true;
					Debug.DrawRay(playerObject.transform.position + forwardVector * 2f, rightVector * 2f, Color.yellow);
				}

				//Debug.Log("left " + navagent.pathStatus);
				navagent.SetDestination(leftPos);
				if (navagent.pathStatus != NavMeshPathStatus.PathInvalid) {
					leftValid = true;
					Debug.DrawRay(playerObject.transform.position + forwardVector * 2f, -rightVector * 2f, Color.yellow);
				}

				Vector3 newDestination;
				if (rightValid && leftValid) {
					if (Vector3.Distance(navagent.transform.position, rightPos) < Vector3.Distance(navagent.transform.position, leftPos)) {
						newDestination = rightPos;
					} else {
						newDestination = leftPos;
					}
				} else if (rightValid) {
					newDestination = rightPos;
				} else if (leftValid) {
					newDestination = leftPos;
				} else {
					SetState(State.CHASE);
					return oldDestination;
				}

				return newDestination;
			default:
				return Vector3.zero;
		}

		return Vector3.zero;
	}

	public bool NearTarget(Vector3 newTarget, float maxDist) {
		Vector2 trgt = new Vector2(newTarget.x, newTarget.z);
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.z);
		
		if (Vector2.Distance(trgt, pos) <= maxDist) {
			return true;
		}

		return false;
	}

	public bool SpotPlayer() {
		if (playerObject == null) {
			playerObject = GameObject.FindWithTag("Player");
		}
		
		Vector2 playerPos = new Vector2(playerObject.transform.position.x, playerObject.transform.position.z);
		Vector2 enemyPos = new Vector2(this.transform.position.x, this.transform.position.z);

		if (Vector2.Distance(playerPos, enemyPos) <= viewDistance) {
			RaycastHit hit;
			if (Physics.Raycast(this.transform.position, (playerObject.transform.position - this.transform.position).normalized, out hit)) {
				return true;
			}
		}

		return false;
	}

	public bool IsStuck() {
		if (isStuck) {
			// is stuck
			stuckTime += Time.deltaTime;
			if (stuckTime <= stuckDuration) {
				// remain stuck
				return true;
			} else {
				// done being stuck
				stuckTime = 0f;
				isStuck = false;
				return false;
			}
		}
		return false;
	}

	void OnDrawGizmos() {
		// Draw Waypoint lines
		Gizmos.color = Color.green;
		for (int i = 0; i < waypoints.Count-1; i++) {
			Vector3 wp1 = new Vector3(waypoints[i].x, 0f, waypoints[i].z);
			Vector3 wp2 = new Vector3(waypoints[i+1].x, 0f, waypoints[i+1].z);
			Gizmos.DrawLine(wp1, wp2);
		}

		// Draw Waypoint spheres
		Gizmos.color = Color.red;
		for (int i = 1; i < waypoints.Count; i++) {
			Vector3 wp1 = new Vector3(waypoints[i].x, 0f, waypoints[i].z);
			Gizmos.DrawSphere(wp1, 1f);
		}
	}
	
	void FixedUpdate () {
		fakeVerticalAxis = Mathf.Clamp(fakeVerticalAxis, -1f, 1f);
		fakeHorizontalAxis = Mathf.Clamp(fakeHorizontalAxis, -1f, 1f);
		
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
				this.rigidbody.AddForce(new Vector3(this.transform.forward.x, 0, this.transform.forward.z)  * forceFactor);
			}
		} else if (fakeVerticalAxis < 0) {
			// DECELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z > -maxSpeed/2) {
				this.rigidbody.AddForce(new Vector3(this.transform.forward.x, 0, this.transform.forward.z)  * -forceFactor/2);
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

		clothAcceleration = new Vector3(rigidbody.angularVelocity.y / maxTurn * 100f, 0f, 5f - localVelocity.z / maxSpeed * 5f);//curMaxTurn/maxTurn, 0);
		sailCloth.externalAcceleration = clothAcceleration;
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
		return Mathf.Atan2(
			Vector3.Dot(n, Vector3.Cross(v1, v2)),
			Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log(this.gameObject.name + " Collision: " + collision.gameObject.name);
		if (collision.gameObject.name == "Terrain") {
			Vector3 newForward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
			Vector3 toCollision = collision.contacts[0].point - this.transform.position;
			float hitAngle = AngleSigned(newForward, toCollision, Vector3.up);
			if (Mathf.Abs(hitAngle) <= 60f) {
				isStuck = true;
			} else {
				stuckTime = 0f;
				isStuck = false;
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.name == "Terrain") {

		}
	}
}