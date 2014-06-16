using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinMap : MonoBehaviour {
	public static PerlinMap instance;

	public GameObject blockPrefab;
	
	public Mode RenderMode;

	public Color[] pixels;
	public Material bumpsMaterial;
	public Color lowColor, highColor;
	public float colorExponent;
	public float pixelScale;
	public float timeScale;
	public float heightScale;
	public float xOffset, yOffset;

	public int texSize;
	private Texture2D baseTex;

	public Material verticesMaterial;
	public MeshFilter terrainMesh;
	private Vector3[] verts;
	private List<int> tris;
	private Vector2[] uvs;
	private Color[] colors;
	private float[] heights;
	private float blockSize;

	public enum Mode {
		Bumps,
		Blocks,
		Pillars,
		Particles,
		Vertices
	}	

	// Use this for initialization
	void Start () {
		baseTex = new Texture2D(texSize, texSize);

		verts = new Vector3[baseTex.height*baseTex.width];
		tris = new List<int>();
		uvs = new Vector2[baseTex.height*baseTex.width];
		colors = new Color[baseTex.height*baseTex.width];
		heights = new float[baseTex.height*baseTex.width];

		this.renderer.enabled = true;
		renderer.material = verticesMaterial;
		this.gameObject.renderer.material.SetTextureScale("_MainTex",new Vector2(1/this.transform.localScale.x,1/this.transform.localScale.z))  ;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));


		blockSize = this.transform.localScale.x / texSize;
		for (int j = 0; j < baseTex.height; j++) {
			float zCoord = this.transform.position.z - this.transform.localScale.z/2 + j*blockSize + blockSize/2;
			for (int i = 0; i < baseTex.width; i++) {
				float xCoord = this.transform.position.x - this.transform.localScale.x/2 + i*blockSize + blockSize/2;

				verts[j * baseTex.width + i] = new Vector3(xCoord/this.transform.localScale.x, this.transform.position.y, zCoord/this.transform.localScale.z);
				uvs[j * baseTex.width + i] = new Vector2(xCoord, zCoord);
				colors[j * baseTex.width + i] = lowColor;

				if (i+1 >= baseTex.width || j+1 >= baseTex.width) {
					continue;
				}
				tris.Add(i + j*baseTex.width);
				tris.Add(i + (j+1)*baseTex.width);
				tris.Add((i+1) + j*baseTex.width);

				tris.Add(i + (j+1)*baseTex.width);
				tris.Add((i+1) + (j+1)*baseTex.width);
				tris.Add((i+1) + j*baseTex.width);
				
			}
		}

		Mesh ret = new Mesh();
        ret.vertices = verts;
        ret.triangles = tris.ToArray();
        ret.uv = uvs;

        ret.RecalculateBounds();
        ret.RecalculateNormals();
        terrainMesh.mesh = ret;


		xOffset = -this.transform.position.x;
		yOffset = -this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		CalculateNoise();
	}

	public void CalculateNoise() {
		for (int y = 0; y < baseTex.height; y++) {
			for (int x = 0; x < baseTex.width; x++) {
				float value = GetValue(this.transform.position.x - this.transform.lossyScale.x/2 + x*blockSize + blockSize/2, this.transform.position.z - this.transform.lossyScale.z/2 + y*blockSize + blockSize/2);
				Color newColor =  lowColor * (1-Mathf.Pow(value,colorExponent)) + highColor * (Mathf.Pow(value,colorExponent));
				colors[y * baseTex.width + x] = newColor;
				//baseTex.SetPixel(x, y*baseTex.width, newColor);

				float newHeight =  this.transform.position.y + value * heightScale;
				heights[y * baseTex.width + x] = newHeight;

				Vector3 vertPosition = verts[y * baseTex.width + x];
				verts[y * baseTex.width + x] = new Vector3(vertPosition.x, newHeight, vertPosition.z);
			}
		}

		baseTex.SetPixels(colors);
		baseTex.Apply();
		renderer.material.SetTexture("_MainTex", baseTex);

		Mesh ret = terrainMesh.mesh;
        ret.vertices = verts;
        ret.colors = colors;

        ret.RecalculateBounds();
        ret.RecalculateNormals();
        terrainMesh.mesh = ret;
	}

	public float GetValue(float x, float y) {
		float newY = (y + this.transform.lossyScale.z/2) / blockSize;
		float newX = (x + this.transform.lossyScale.x/2) / blockSize;
		float value = Mathf.PerlinNoise(timeScale * Time.time + newX * pixelScale + xOffset, timeScale * Time.time + newY * pixelScale + yOffset);

		return value;
	}

	public float GetHeight(float x, float y) {
		float newHeight =  this.transform.position.y + this.GetValue(x,y) * heightScale;

		return newHeight;
	}

	// public float GetHeight(float x, float y) {
	// 	//Debug.Log(x + " " + y);
	// 	int newY = (int) (y + this.transform.localScale.z/2);
	// 	int newX = (int) (x + this.transform.localScale.x/2);
	// 	newY = (int) Mathf.Round(newY / blockSize);
	// 	newX = (int) Mathf.Round(newX / blockSize);
	// 	//Debug.Log(newX + "/" + newY);

	// 	return heights[newY * baseTex.width + newX];
	// }
}
