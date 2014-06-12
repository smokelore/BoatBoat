using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	public GameObject waveMesh;
	public PerlinMap perlinMap;

	// Use this for initialization
	void Start () {
		perlinMap = waveMesh.GetComponent<PerlinMap>();
	}
	
	// Update is called once per frame
	void Update () {
		float x = this.transform.position.x;
		float z = this.transform.position.z;
		/*int x1 = (int) Mathf.Floor(x);
		int x2 = (int) Mathf.Ceil(x);
		int z1 = (int) Mathf.Floor(z);
		int z2 = (int) Mathf.Ceil(z);
		
		float x1z1_height = perlinMap.GetHeight(x1, z1);
		float x1z2_height = perlinMap.GetHeight(x1, z2);
		float x2z1_height = perlinMap.GetHeight(x2, z1);
		float x2z2_height = perlinMap.GetHeight(x2, z2);
	
		float newHeight = (x1z1_height + x1z2_height + x2z1_height + x2z2_height) / 4;*/
		float newHeight = perlinMap.GetHeight(Mathf.Round(x), Mathf.Round(z));
		this.transform.position = new Vector3(x, newHeight, z);
	}
}
