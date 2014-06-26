using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public GameObject waveMesh;
	public PerlinMap perlinMap;
	public float lerpFactor;
	public float pushUpSpeed;
	public float pushHeight;
	private float targetHeight;
	private bool hitFlag, prevHitFlag;
	//private bool pushBoat = false;
	private bool pushBoatUp;
	private bool pushBoatDown;
	//private SpoutMove spoutMove;
	//private bool spoutUpDirection;
	public AudioClip spoutSound;

	private Vector3 pointN, pointE, pointS, pointW;
	private float width, length, zAngle, xAngle;

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
		
		float x = this.transform.position.x;
		float z = this.transform.position.z;

		//float currentHeight = this.transform.position.y;
		targetHeight = perlinMap.GetHeight(x, z) + 0.065f;
		float NHeight = perlinMap.GetHeight(x, z+length/2) + 0.065f;
		float EHeight = perlinMap.GetHeight(x+width/2, z) + 0.065f;
		float SHeight = perlinMap.GetHeight(x, z-length/2) + 0.065f;
		float WHeight = perlinMap.GetHeight(x-width/2, z) + 0.065f;

		waveMesh.transform.position = new Vector3(x, waveMesh.transform.position.y, z);
		//float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * lerpFactor);
		
		//this.transform.position = new Vector3(x, targetHeight, z);

		pointN = this.transform.position + this.transform.forward*length/2;
		pointN = new Vector3(pointN.x, NHeight, pointN.z);
		pointS = this.transform.position - this.transform.forward*length/2;
		pointS = new Vector3(pointS.x, SHeight, pointS.z);
		pointE = this.transform.position + this.transform.right*width/2;
		pointE = new Vector3(pointE.x, EHeight, pointE.z);
		pointW = this.transform.position - this.transform.right*width/2;
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
	}

	void FixedUpdate() {
		float x = this.transform.position.x;
		float z = this.transform.position.z;

		if (this.transform.position.y > targetHeight + pushHeight) {
			hitFlag = false;
		}

		if (hitFlag && !prevHitFlag) {
			audio.PlayOneShot(spoutSound, 0.7f);
		}

		if (hitFlag && this.transform.position.y < targetHeight + pushHeight) {
			rigidbody.AddForce(Vector3.up * pushUpSpeed);
		} else if (!hitFlag && this.transform.position.y > targetHeight + 1f) {
			rigidbody.AddForce(-Vector3.up * pushUpSpeed/2);
		} else {
			this.transform.position = new Vector3(x, targetHeight, z);
			this.rigidbody.AddTorque(this.transform.right * zAngle/4);
			this.rigidbody.AddTorque(this.transform.forward * xAngle/4);
		}

		prevHitFlag = hitFlag;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Spout")
		{
			Debug.Log("inside spout");
			// Get velocity of y right as we enter, only once, to set it the same as we leave
			hitFlag = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Spout")
		{
			//hitFlag = false;
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}