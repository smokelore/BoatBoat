using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public GameObject waveMesh;
	public PerlinMap perlinMap;
	public float lerpFactor;

	// Use this for initialization
	void Start () {
		perlinMap = waveMesh.GetComponent<PerlinMap>();
	}
	
	// Update is called once per frame
	void Update () {
		float x = this.transform.position.x;
		float z = this.transform.position.z;

		//float currentHeight = this.transform.position.y;
		float targetHeight = perlinMap.GetHeight(x, z) + this.transform.lossyScale.y/2;
		//float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * lerpFactor);
		this.transform.position = new Vector3(x, targetHeight, z);
	}
}
