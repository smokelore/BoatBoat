using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public GameObject waveMesh;
	public PerlinMap perlinMap;
	public float lerpFactor;
	private SpoutMove spoutMove;
	public bool spoutUpDirection;

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
			spoutUpDirection = spoutMove.ChangeSpoutDirection();
			// Change height or spin 360
			// add whoo whoo whoo sounds
		}
	}

	// Update is called once per frame
	void Update () {
		float x = this.transform.position.x;
		float z = this.transform.position.z;

		//float currentHeight = this.transform.position.y;
		float targetHeight = perlinMap.GetHeight(x, z);
		//float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * lerpFactor);
		this.transform.position = new Vector3(x, targetHeight, z);
	}
}
