using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public float health = 100f;

	public GameObject waveMesh;
	private PerlinMap perlinMap;
	// public float lerpFactor;
	// public float pushUpSpeed;
	// public float pushHeight;
	private float targetHeight;
	// private bool hitFlag, prevHitFlag;
	// private bool pushBoat = false;
	// private bool pushBoatUp;
	// private bool pushBoatDown;
	// private SpoutMove spoutMove;
	// private bool spoutUpDirection;
	// public AudioClip spoutSound;
	public bool sinking;

	private Vector3 pointN, pointE, pointS, pointW;
	private float width, length, zAngle, xAngle;

	public ParticleSystem wakeSystemL;
	public ParticleSystem wakeSystemR;

	// Use this for initialization
	void Start () {
		perlinMap = waveMesh.GetComponent<PerlinMap>();

		length = this.gameObject.GetComponent<CapsuleCollider>().height * this.transform.lossyScale.z;
		width = this.gameObject.GetComponent<CapsuleCollider>().radius * this.transform.lossyScale.x * 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (perlinMap == null) {
			perlinMap = waveMesh.GetComponent<PerlinMap>();
		}

		Vector3 nextPosition = this.transform.position + this.rigidbody.velocity * Time.deltaTime;
		float x = nextPosition.x;
		float z = nextPosition.z;

		targetHeight = perlinMap.GetHeight(x, z) + 0.08f;
		float NHeight = perlinMap.GetHeight(x, z+length/2) + 0.08f;
		float EHeight = perlinMap.GetHeight(x+width/2, z) + 0.08f;
		float SHeight = perlinMap.GetHeight(x, z-length/2) + 0.08f;
		float WHeight = perlinMap.GetHeight(x-width/2, z) + 0.08f;

		pointN = nextPosition + this.transform.forward*length/2;
		pointN = new Vector3(pointN.x, NHeight, pointN.z);
		pointS = nextPosition - this.transform.forward*length/2;
		pointS = new Vector3(pointS.x, SHeight, pointS.z);
		pointE = nextPosition + this.transform.right*width/2;
		pointE = new Vector3(pointE.x, EHeight, pointE.z);
		pointW = nextPosition - this.transform.right*width/2;
		pointW = new Vector3(pointW.x, WHeight, pointW.z);

		Vector3 NSVector = (pointN - pointS).normalized;
		Vector3 EWVector = (pointE - pointW).normalized;

		zAngle = AngleSigned(this.transform.forward, NSVector, this.transform.right);
		xAngle = AngleSigned(this.transform.right, EWVector, this.transform.forward);

		//float xAngle = AngleSigned(this.transform.right, xVector.normalized, this.transform.forward);
		//float zAngle = AngleSigned(this.transform.forward, zVector.normalized, this.transform.right);
		
		Debug.DrawLine(pointW, pointE, Color.red);
		Debug.DrawLine(pointS, pointN, Color.blue);

		//Vector3 targetRotation = new Vector3(zAngle, this.transform.rotation.y, xAngle);
		
		//Quaternion qX = Quaternion.AngleAxis(xAngle, this.transform.forward);
		//Quaternion qZ = Quaternion.AngleAxis(zAngle, this.transform.right);
		//this.transform.localRotation = qX * qZ;

		//this.transform.Rotate(this.transform.forward, xAngle - this.transform.eulerAngles.x);
		//this.transform.Rotate(this.transform.right, zAngle - this.transform.eulerAngles.z);
		//Debug.Log(zAngle);

		Vector3 localVelocity = this.transform.InverseTransformDirection(this.rigidbody.velocity);
		if (this.gameObject.tag == "Player") {
			forceController fc = this.gameObject.GetComponent<forceController>();
			if (sinking) {
				fc.enabled = false;
			} else {
				if (localVelocity.z >= 0f) {
					if (wakeSystemL != null && wakeSystemR != null) {
						wakeSystemL.emissionRate = 500f * Mathf.Abs(fc.curSpeed/fc.maxSpeed);
						wakeSystemR.emissionRate = 500f * Mathf.Abs(fc.curSpeed/fc.maxSpeed);
					}
				}
			}
		} else if (this.gameObject.tag == "Enemy") {
			enemyController fc = this.gameObject.GetComponent<enemyController>();
			if (sinking) {
				fc.enabled = false;
			} else {
				if (localVelocity.z >= 0f) {
					if (wakeSystemL != null && wakeSystemR != null) {
						wakeSystemL.emissionRate = 500f * Mathf.Abs(fc.curSpeed/fc.maxSpeed);
						wakeSystemR.emissionRate = 500f * Mathf.Abs(fc.curSpeed/fc.maxSpeed);
					}
				}
			}
		}

		if (this.transform.position.y <= -2f) {
			Destroy(this.gameObject);
		}

		
	}

	void FixedUpdate() {
		// if (this.transform.position.y > targetHeight + pushHeight) {
		// 	hitFlag = false;
		// }

		// if (hitFlag && !prevHitFlag) {
		// 	audio.PlayOneShot(spoutSound, 0.7f);
		// }

		// if (hitFlag && this.transform.position.y < targetHeight + pushHeight) {
		// 	rigidbody.AddForce(Vector3.up * pushUpSpeed);
		// } else if (!hitFlag && this.transform.position.y > targetHeight + 1f) {
		// 	rigidbody.AddForce(-Vector3.up * pushUpSpeed/2);
		// }else 

		if (sinking){
			sink();
		} else {
			Vector3 newRight = new Vector3(this.transform.right.x, 0f, this.transform.right.z).normalized;
			Vector3 newForward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z).normalized;
			this.transform.position = new Vector3(this.transform.position.x, targetHeight, this.transform.position.z);
			zAngle = Mathf.Clamp(zAngle, -20f, 20f);
			float zForce = Mathf.Sign(zAngle) * Mathf.Pow(zAngle/5f, 2f);
			this.rigidbody.AddTorque(newRight * zForce);

			xAngle = Mathf.Clamp(xAngle, -20f, 20f);
			float xForce = Mathf.Sign(xAngle) * Mathf.Pow(xAngle/5f, 2f);
			this.rigidbody.AddTorque(newForward * xAngle/5);
		}

		// prevHitFlag = hitFlag;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Spout")
		{
			Debug.Log("inside spout");
			// Get velocity of y right as we enter, only once, to set it the same as we leave
			// hitFlag = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Spout")
		{
			//hitFlag = false;
		}
	}

	public void ApplyDamage(float damage) {
		Debug.Log(gameObject.name + " was shot for " + damage + " damage.");
		health = Mathf.Clamp(health - damage, 0, 100);
		if (health <= 0) {
			sinking = true;
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}

	void sink(){
		this.rigidbody.AddTorque (this.transform.right * -1f);
		this.rigidbody.AddForce(Vector3.down * 0.75f);
		Destroy(this.gameObject, 6f);
	}

	void OnDestroy() {
		//Debug.Log("die");
		if (this.gameObject.tag == "Player" && sinking) {
			//Debug.Log("load");
			Application.LoadLevel("main");
		}
	}
}