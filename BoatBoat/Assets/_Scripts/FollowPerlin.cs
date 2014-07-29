using UnityEngine;
using System.Collections;

public class FollowPerlin : MonoBehaviour {
	public GameObject waveMesh;
	
	// Update is called once per frame
	void Update () {
		if (waveMesh != null) {
			waveMesh.transform.position = new Vector3(this.transform.position.x, waveMesh.transform.position.y, this.transform.position.z);
			waveMesh.renderer.material.SetTextureOffset("_MainTex", new Vector2(this.transform.position.x/10, this.transform.position.z/10));
		}
	}
}
