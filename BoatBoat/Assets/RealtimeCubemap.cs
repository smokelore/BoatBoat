using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RealtimeCubemap : MonoBehaviour {
	public RenderTexture rendertexture;
	public Camera cam;
	public GameObject boatboatObject;

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
			cam.cullingMask = ~(1 << 5);

			//cam.transform.position = Vector3.zero;
			//cam.transform.rotation = Quaternion.identity;
			//cam.transform.position = Camera.main.transform.position;
			//cam.transform.rotation = Camera.main.transform.rotation;
			cam.enabled = false;
		}
		Vector3 newPosition = Camera.main.transform.position;
		newPosition = new Vector3(newPosition.x, -newPosition.y, newPosition.z);
		cam.transform.position = newPosition;
		cam.transform.LookAt(boatboatObject.transform.position);
		

		rendertexture = new RenderTexture (256, 256, 16);
		rendertexture.isCubemap = true;
		cam.RenderToCubemap(rendertexture);

		this.renderer.sharedMaterial.SetTexture("_Cube", rendertexture);
	}
}
