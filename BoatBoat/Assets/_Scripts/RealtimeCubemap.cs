/*// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RealtimeCubemap : MonoBehaviour {
	public RenderTexture rendertexture;
	public Camera cam;
	public GameObject boatboatObject;
	public SpotterController spotterZoneController;
	//public Vector3 camPositionOffset;
	//public Vector3 camTargetOffset;

	// Use this for initialization
	void Start () {
		rendertexture.isCubemap = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (cam == null) {
			GameObject tempCam = new GameObject("Cubemap Camera");
			tempCam.AddComponent("Camera");
			cam = tempCam.camera;
			cam.farClipPlane = Camera.main.farClipPlane;
			cam.fieldOfView = Camera.main.fieldOfView;
			//tempCam.AddComponent("Skybox");
			//tempCam.GetComponent<Skybox>().material  = Camera.main.gameObject.GetComponent<Skybox>().material;
			cam.cullingMask = ~(1 << 5) ^ (1 << 4);

			//cam.transform.position = Vector3.zero;
			//cam.transform.rotation = Quaternion.identity;
			//cam.transform.position = Camera.main.transform.position;
			//cam.transform.rotation = Camera.main.transform.rotation;
			cam.enabled = false;
		}

		Vector3 newForward = boatboatObject.transform.forward;
		newForward = new Vector3(newForward.x, 0, newForward.z);

		Vector3 newRight = boatboatObject.transform.right;
		newRight = new Vector3(newRight.x, 0, newRight.z);

		//Vector3 newPosition = boatboatObject.transform.position + newForward * camPositionOffset.z + newRight * camPositionOffset.x + Vector3.up * camPositionOffset.y;
		//Vector3 aimAtPoint = boatboatObject.transform.position + newForward * camTargetOffset.z + newRight * camTargetOffset.x + Vector3.up * camTargetOffset.y;
		Vector3 newPosition = Camera.main.transform.position;
		newPosition = new Vector3(newPosition.x, -newPosition.y, newPosition.z);
		Vector3 aimAtPoint = boatboatObject.transform.position + spotterZoneController.aimOffset;

		cam.transform.position = newPosition;
		cam.transform.LookAt(aimAtPoint);

		//rendertexture = new RenderTexture (64, 64, 16);
		rendertexture.isCubemap = true;
		cam.RenderToCubemap(rendertexture);

		this.renderer.sharedMaterial.SetTexture("_Cube", rendertexture);
	}
}
*/