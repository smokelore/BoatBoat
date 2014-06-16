using UnityEngine;
using System.Collections;

public class shipController : MonoBehaviour {
	private Vector3 curPos;
	public float axisNum;
	
	public float factor = 0.1f;
	public float rotFactor = 0.4f;
	public float maxSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		axisNum = transform.rotation.y;

		if (Input.GetAxis("Vertical") > 0) {
			// ACCELERATE
			rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, transform.forward * maxSpeed, Time.deltaTime * 2);
		} else if (Input.GetAxis("Vertical") < 0) {
			// DECELERATE

		}

		if (Input.GetAxis("Horizontal") < 0) {
			// RIGHT TURN
			axisNum -= rigidbody.velocity.magnitude;
			if(Mathf.Abs(axisNum) > rotFactor){
				axisNum = -rotFactor;
			}
			transform.Rotate(new Vector3(0, axisNum, 0));
			rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;
		} else if (Input.GetAxis("Horizontal") > 0) {
			// LEFT TURN
			axisNum += rigidbody.velocity.magnitude;
			if(Mathf.Abs (axisNum) > rotFactor){
				axisNum = rotFactor;
			}
			transform.Rotate (new Vector3(0, axisNum, 0));
			rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;
		}
	}
}
