using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject playerBoat;
	public Vector3 positionDifference;
	public float lerpSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 aimAtPoint = playerBoat.transform.position + Vector3.up * positionDifference.y;

		this.transform.LookAt(aimAtPoint);

		Vector3 newForward = playerBoat.transform.forward;
		newForward = new Vector3(newForward.x, 0, newForward.z);

		Vector3 currentPosition = this.transform.position;
		Vector3 targetPosition = playerBoat.transform.position + newForward*positionDifference.z + Vector3.up*positionDifference.y;

		this.transform.position = Vector3.Lerp(currentPosition, targetPosition, lerpSpeed * Time.deltaTime);
	}
}
