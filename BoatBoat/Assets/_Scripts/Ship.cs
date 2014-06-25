using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public GameObject waveMesh;
	public PerlinMap perlinMap;
	public float lerpFactor;
	public float pushUpSpeed;
	public float pushHeight;
	private float targetHeight;
	private bool hitFlag = false;
	//private bool pushBoat = false;
	private bool pushBoatUp = true;
	private bool pushBoatDown = false;
	//private SpoutMove spoutMove;
	//private bool spoutUpDirection;

	private Vector3 pointN, pointE, pointS, pointW;
	private float width, length, zAngle, xAngle;

	// Use this for initialization
	void Start () {
		// Chase adding find SpoutMove script
		// GameObject spoutMoveObject = GameObject.FindWithTag ("Spout");
		// if (spoutMoveObject != null)
		// {
		// 	spoutMove = spoutMoveObject.GetComponent <SpoutMove>();
		// }
		// if (spoutMove == null)
		// {
		// 	Debug.Log ("Cannot find 'SpoutMove' script");
		// }

		perlinMap = waveMesh.GetComponent<PerlinMap>();

		length = this.gameObject.GetComponent<CapsuleCollider>().height * this.transform.lossyScale.z;
		width = this.gameObject.GetComponent<CapsuleCollider>().radius * this.transform.lossyScale.x * 2;
	}

	//Chase adding coliding into spouts below
	/*void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Spout")
		{
			hitFlag = true;
			spoutUpDirection = spoutMove.ChangeSpoutDirection();
			if (spoutUpDirection)
			{
				audio.Play();
				pushBoat = true;
				pushBoatUp = true;
			}
			else
			{
				hitFlag = false;
			}
			// spin 360
		}
	}*/

	/*void PushBoatUpDown()
	{
		if ((this.transform.position.y < pushHeight.y) && pushBoatUp)
		{
			this.transform.position += Vector3.up * pushUpSpeed * Time.deltaTime;
			pushBoatDown = true;
		}
		else if ((this.transform.position.y > targetHeight) && pushBoatDown)
		{
			this.transform.position -= Vector3.up * pushUpSpeed * Time.deltaTime;
			pushBoatUp = false;
		}
		else
		{
			this.transform.position = new Vector3(this.transform.position.x, targetHeight, this.transform.position.z);
			pushBoatDown = false;
			pushBoat = false;
			hitFlag = false;
		}
	}*/
	
	// Update is called once per frame
	void Update () {
		if (perlinMap == null) {
			perlinMap = waveMesh.GetComponent<PerlinMap>();
		}
		
		float x = this.transform.position.x;
		float z = this.transform.position.z;

		//float currentHeight = this.transform.position.y;
		targetHeight = perlinMap.GetHeight(x, z) + 0.25f;
		float NHeight = perlinMap.GetHeight(x, z+length/2) + 0.25f;
		float EHeight = perlinMap.GetHeight(x+width/2, z) + 0.25f;
		float SHeight = perlinMap.GetHeight(x, z-length/2) + 0.25f;
		float WHeight = perlinMap.GetHeight(x-width/2, z) + 0.25f;

		waveMesh.transform.position = new Vector3(x, waveMesh.transform.position.y, z);
		//float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * lerpFactor);
		
		this.transform.position = new Vector3(x, targetHeight, z);

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

		

		if (hitFlag)
		{
			rigidbody.AddForce(Vector3.up * pushUpSpeed);
			pushBoatUp = true;
		}
		else
		{
			if ((this.transform.position.y < pushHeight) && pushBoatUp)
			{
				Debug.Log("pushing up");
				rigidbody.AddForce(Vector3.up * pushUpSpeed);
				pushBoatDown = true;
			}
			else if ((this.transform.position.y > targetHeight + 1.0f) && pushBoatDown)
			{
				Debug.Log("pushing down");
				rigidbody.AddForce(-Vector3.up * pushUpSpeed);
				pushBoatUp = false;
			}
			else
			{
				Debug.Log("not pushing");
				//rigidbody.constraints = originalConstraints; // To move up and down by locking position on x,z
				this.transform.position = new Vector3(x, targetHeight, z);
				this.rigidbody.AddTorque(this.transform.right * zAngle/5);
				this.rigidbody.AddTorque(this.transform.forward * xAngle/5);
				pushBoatDown = false;
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Spout")
		{
			Debug.Log("inside spout");
			// Get velocity of y right as we enter, only once, to set it the same as we leave
			hitFlag = true;

			//Lock x,z postion to push up and down with force
			//rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}
		else
		{
			hitFlag = false;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Spout")
		{
			hitFlag = false;
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}