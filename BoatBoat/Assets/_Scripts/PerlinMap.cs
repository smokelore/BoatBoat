using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinMap : MonoBehaviour {
	public static PerlinMap instance;
	
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

	public bool whirlpoolEnable;
	private bool[] whirl;
	public GameObject whirlColliderPrefab;
	private List<GameObject> whirlColliderInstances = new List<GameObject>();
	public float whirlRadius;
	private Vector2 whirlCenter;

	private float blockSize;

	// Use this for initialization
	void Start () {
		baseTex = new Texture2D(texSize, texSize);

		verts = new Vector3[baseTex.height*baseTex.width];
		tris = new List<int>();
		uvs = new Vector2[baseTex.height*baseTex.width];
		colors = new Color[baseTex.height*baseTex.width];
		heights = new float[baseTex.height*baseTex.width];
		whirl = new bool[baseTex.height*baseTex.width];

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


		// xOffset = -this.transform.position.x;
		// yOffset = -this.transform.position.y;
		if (whirlpoolEnable) {
			this.CreateWhirlpool(-15, 5, whirlRadius);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CalculateNoise();
	}

	public void CalculateNoise() {
		float value, newHeight;
		Color newColor;
		Vector3 vertPosition;
		for (int y = 0; y < baseTex.height; y++) {
			for (int x = 0; x < baseTex.width; x++) {
				if (!whirl[y * baseTex.width + x]) {
					value = this.GetValue(x, y);
					newColor =  lowColor * (1-Mathf.Pow(value+0.5f,colorExponent)) + highColor * (Mathf.Pow(value+0.5f,colorExponent));
					colors[y * baseTex.width + x] = newColor;

					newHeight =  this.transform.position.y + value * heightScale;
					heights[y * baseTex.width + x] = newHeight;

					vertPosition = verts[y * baseTex.width + x];
					verts[y * baseTex.width + x] = new Vector3(vertPosition.x, newHeight, vertPosition.z);
				}
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

	public float GetValue(int x, int y) {
		// input in terms of Ocean array index
		float value = -0.5f + Mathf.PerlinNoise(timeScale * Time.time + x * pixelScale + xOffset, timeScale * Time.time + y * pixelScale + yOffset);

		return value;
	}

	public float GetValue(float x, float y) {
		// input in terms of world position
		float newY = (y + this.transform.lossyScale.z/2) / blockSize;
		float newX = (x + this.transform.lossyScale.x/2) / blockSize;
		float value = -0.5f + Mathf.PerlinNoise(timeScale * Time.time + newX * pixelScale + xOffset, timeScale * Time.time + newY * pixelScale + yOffset);

		return value;
	}

	public float GetHeight(float x, float y) {
		float newHeight;
		float dist = Vector2.Distance(whirlCenter, new Vector2(x, y));
		//Debug.Log(dist);
		if (dist < whirlRadius) {
			float value = Mathf.Cos((whirlRadius - dist)/whirlRadius * Mathf.PI);
			newHeight = this.transform.position.y + (value-1)/2 * heightScale;
			//Debug.Log("whirl");
		} else {
			newHeight = this.transform.position.y + this.GetValue(x,y) * heightScale;
		}

		return newHeight;
	}

	public void CreateWhirlpool(float xWhirl, float zWhirl, float radius) {
		float value, newHeight;
		Color newColor;
		Vector3 vertPosition;

		blockSize = this.transform.localScale.x / texSize;
		whirlCenter = new Vector2(xWhirl, zWhirl);
		whirlRadius = radius;
		for (int j = 0; j < baseTex.height; j++) {
			float zCoord = this.transform.position.z - this.transform.localScale.z/2 + j*blockSize + blockSize/2;
			for (int i = 0; i < baseTex.width; i++) {
				float xCoord = this.transform.position.x - this.transform.localScale.x/2 + i*blockSize + blockSize/2;
				float dist = Vector2.Distance(whirlCenter, new Vector2(xCoord, zCoord));
				if (dist < radius) {
					whirl[j * baseTex.width + i] = true;

					value = Mathf.Cos((radius - dist)/radius * Mathf.PI);
					newColor =  lowColor * (1-Mathf.Pow(value/2+0.5f,colorExponent)) + highColor * (Mathf.Pow(value/2+0.5f,colorExponent));
					colors[j * baseTex.width + i] = newColor;

					newHeight =  this.transform.position.y + (value-1)/2 * heightScale;
					heights[j * baseTex.width + i] = newHeight;

					vertPosition = verts[j * baseTex.width + i];
					verts[j * baseTex.width + i] = new Vector3(vertPosition.x, newHeight, vertPosition.z);
				}
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

        GameObject whirlpool = Instantiate(whirlColliderPrefab, new Vector3(xWhirl, 0, zWhirl), Quaternion.identity) as GameObject;
        whirlpool.transform.localScale = new Vector3(radius*1.75f, radius*1.75f, radius*1.75f);
        whirlColliderInstances.Add(whirlpool);
	}

	public void DestroyAllWhirlpools() {
		foreach (GameObject whirl in whirlColliderInstances) {
			Destroy(whirl);
		}
		
		for (int j = 0; j < baseTex.height; j++) {
			for (int i = 0; i < baseTex.width; i++) {
				whirl[j * baseTex.width + i] = false;
			}
		}
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
