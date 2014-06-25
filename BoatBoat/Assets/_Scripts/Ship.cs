using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public GameObject waveMesh;
	public PerlinMap perlinMap;
	public float lerpFactor;
	public float pushUpSpeed;
	public Vector3 pushHeight;
	private float targetHeight;
	private bool hitFlag = false;
	private bool pushBoat = false;
	private bool pushBoatUp = true;
	private bool pushBoatDown = false;
	private SpoutMove spoutMove;
	private bool spoutUpDirection;

	// Use this for initialization
	void Start () {
		// Chase adding find SpoutMove script
		GameObject spoutMoveObject = GameObject.FindWithTag ("Spout");
		if (spoutMoveObject != null)
		{
			spoutMove = spoutMoveObject.GetComponent <SpoutMove>();
		}
		if (spoutMove == null)
		{
			Debug.Log ("Cannot find 'SpoutMove' script");
		}

		perlinMap = waveMesh.GetComponent<PerlinMap>();
	}

	//Chase adding coliding into spouts below
	void OnCollisionEnter (Collision col)
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
	}

	void PushBoatUpDown()
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
	}
	
	// Update is called once per frame
	void Update () {
		if (perlinMap == null) {
			perlinMap = waveMesh.GetComponent<PerlinMap>();
		}
		
		float x = this.transform.position.x;
		float z = this.transform.position.z;

		//float currentHeight = this.transform.position.y;
		targetHeight = perlinMap.GetHeight(x, z) + 0.25f;
		waveMesh.transform.position = new Vector3(x, waveMesh.transform.position.y, z);

		//float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * lerpFactor);
		if (hitFlag)
		{
			if (pushBoat)
			{
				PushBoatUpDown();
			}
		}
		else
		{
			this.transform.position = new Vector3(x, targetHeight, z);
		}
	}
}