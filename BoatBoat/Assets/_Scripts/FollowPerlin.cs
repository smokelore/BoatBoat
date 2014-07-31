// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
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
