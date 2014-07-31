// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SailMesh : MonoBehaviour {
	public float pixelScale;
	public float timeScale;
	public float displacementScale;
	public float xOffset, yOffset;

	public int texSize;
	public Texture2D baseTex;

	public MeshFilter sailMesh;
	private Vector3[] verts;
	private List<int> tris;
	private Vector2[] uvs;
	private float[] displacements;

	private float blockSize;

	// Use this for initialization
	void Start () {
		baseTex = new Texture2D(texSize, texSize);
		verts = new Vector3[baseTex.height*baseTex.width];
		tris = new List<int>();
		uvs = new Vector2[baseTex.height*baseTex.width];
		displacements = new float[baseTex.height*baseTex.width];

		//this.renderer.enabled = true;
		this.gameObject.renderer.material.SetTextureScale("_MainTex",new Vector2(1/this.transform.localScale.x,1/this.transform.localScale.z))  ;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));

		blockSize = this.transform.localScale.x / texSize;
		for (int j = 0; j < baseTex.height; j++) {
			float yCoord = this.transform.position.y - this.transform.localScale.y/2 + j*blockSize + blockSize/2;
			for (int i = 0; i < baseTex.width; i++) {
				float xCoord = this.transform.position.x - this.transform.localScale.x/2 + i*blockSize + blockSize/2;
				verts[j * baseTex.width + i] = new Vector3(xCoord/this.transform.localScale.x, yCoord/this.transform.localScale.y, this.transform.position.z);
				uvs[j * baseTex.width + i] = new Vector2(xCoord, yCoord);

				if (i+1 >= baseTex.width || j+1 >= baseTex.width) {
					continue;
				}
				tris.Add(i + j*baseTex.width);
				tris.Add(i + (j+1)*baseTex.width);
				tris.Add((i+1) + j*baseTex.width);

				tris.Add(i + (j+1)*baseTex.width);
				tris.Add((i+1) + (j+1)*baseTex.width);
				tris.Add((i+1) + j*baseTex.width);

				if (i-1 < 0 || j-1 < 0) {
					continue;
				}
				tris.Add(i + j*baseTex.width);
				tris.Add(i + (j-1)*baseTex.width);
				tris.Add((i-1) + j*baseTex.width);

				tris.Add(i + (j-1)*baseTex.width);
				tris.Add((i-1) + (j-1)*baseTex.width);
				tris.Add((i-1) + j*baseTex.width);
			}
		}

		Mesh ret = new Mesh();
        ret.vertices = verts;
        ret.triangles = tris.ToArray();
        ret.uv = uvs;

        ret.RecalculateBounds();
        ret.RecalculateNormals();
        sailMesh.mesh = ret;
	}
	
	// Update is called once per frame
	void Update () {
		CalculateNoise();
	}

	public void CalculateNoise() {
		Vector3 vertPosition;
		for (int y = 0; y < baseTex.height; y++) {
			float yCoord = this.transform.position.y - this.transform.localScale.y/2 + y*blockSize + blockSize/2;
			for (int x = 0; x < baseTex.width; x++) {
				float xCoord = this.transform.position.x - this.transform.localScale.x/2 + x*blockSize + blockSize/2;

				displacements[y * baseTex.width + x] = GetDisplacement(xCoord, yCoord);

				vertPosition = verts[y * baseTex.width + x];
				verts[y * baseTex.width + x] = new Vector3(vertPosition.x, vertPosition.y, displacements[y * baseTex.width + x]);
			}
		}

		renderer.material.SetTexture("_MainTex", baseTex);

		Mesh ret = sailMesh.mesh;
        ret.vertices = verts;

        ret.RecalculateBounds();
        ret.RecalculateNormals();
        sailMesh.mesh = ret;
	}

	public float GetVertexValue(float x, float y) {
		return GetPerlinValue(x, y);
	}

	public float GetPerlinValue(float x, float y) {
		// input in terms of world position
		float perlin1 = -0.5f + Mathf.PerlinNoise(timeScale * Time.time + x * pixelScale + xOffset, timeScale * Time.time + y * pixelScale/2 + yOffset);
		//float perlin2 = -0.5f + Mathf.PerlinNoise(-timeScale * Time.time + x * pixelScale + xOffset, -timeScale * Time.time + y * pixelScale + yOffset);
		float value = perlin1; //(0.5f*perlin1 + 0.5f*perlin2);

		return value;
	}

	public float GetDisplacement(float x, float y) {
		// input in terms of world position
		float value = GetVertexValue(x, y);
		float displacement = this.transform.position.y + value * displacementScale;

		return displacement;
	}
}
