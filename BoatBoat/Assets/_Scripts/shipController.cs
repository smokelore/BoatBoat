using UnityEngine;
using System.Collections;

public class shipController : MonoBehaviour {

	Vector3 curPos;
	float axisNum;
	
	public float factor = 0.1f;
	public float rotFactor = 0.4f;
	public float monitor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		monitor = axisNum;
		axisNum = transform.rotation.y;
		if(Input.GetKey (KeyCode.W)){
			/*if(rigidbody.velocity.magnitude <= 2.0f){
				rigidbody.velocity+=transform.forward * factor;
			}*/
			rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, transform.forward, Time.deltaTime * 2);
		}
		if(Input.GetKey(KeyCode.A)){
			axisNum -= rigidbody.velocity.magnitude;
			if(Mathf.Abs(axisNum) > rotFactor){
				axisNum = -rotFactor;
			}
			transform.Rotate(new Vector3(0, axisNum, 0));
			rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;
		}
		if(Input.GetKey (KeyCode.D)){
			axisNum += rigidbody.velocity.magnitude;
			if(Mathf.Abs (axisNum) > rotFactor){
				axisNum = rotFactor;
			}
			transform.Rotate (new Vector3(0, axisNum, 0));
			rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;
		}
	}
}
