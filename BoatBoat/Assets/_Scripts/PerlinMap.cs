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

	public bool whirlpoolsEnabled;
	public GameObject[] allWhirlpools;

	private float blockSize;

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


		// xOffset = -this.transform.position.x;
		// yOffset = -this.transform.position.y;
		allWhirlpools = GameObject.FindGameObjectsWithTag("Whirlpool");
		Debug.Log(allWhirlpools);
		if (!whirlpoolsEnabled) {
			DestroyAllWhirlpools();
		}
	}
	
	// Update is called once per frame
	void Update () {
		CalculateNoise();
	}

	public void CalculateNoise() {
		Vector3 vertPosition;
		for (int y = 0; y < baseTex.height; y++) {
			float zCoord = this.transform.position.z - this.transform.localScale.z/2 + y*blockSize + blockSize/2;
			for (int x = 0; x < baseTex.width; x++) {
				float xCoord = this.transform.position.x - this.transform.localScale.x/2 + x*blockSize + blockSize/2;
				
				colors[y * baseTex.width + x] = GetColor(xCoord, zCoord);
				heights[y * baseTex.width + x] = GetHeight(xCoord, zCoord);//this.transform.position.y + perlin * heightScale;

				vertPosition = verts[y * baseTex.width + x];
				verts[y * baseTex.width + x] = new Vector3(vertPosition.x, heights[y * baseTex.width + x], vertPosition.z);
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

	public float GetPerlinValue(float x, float z) {
		// input in terms of world position
		float perlin1 = -0.5f + Mathf.PerlinNoise(timeScale * Time.time + x * pixelScale + xOffset, timeScale * Time.time + z * pixelScale + yOffset);
		float perlin2 = -0.5f + Mathf.PerlinNoise(-timeScale * Time.time + x * pixelScale + xOffset, -timeScale * Time.time + z * pixelScale + yOffset);
		float value = (perlin1 + perlin2)/2;

		return value;
	}

	public Color GetColor(float x, float z) {
		// input in terms of world position
		float perlin = GetPerlinValue(x, z);
		Color newColor = lowColor * (1-Mathf.Pow(perlin+0.5f,colorExponent)) + highColor * (Mathf.Pow(perlin+0.5f,colorExponent));
	
		return newColor;
	}

	public float GetHeight(float x, float z) {
		// input in terms of world position
		float height;

		if (HasWhirlpool(x, z)) {
			height = this.transform.position.y + GetWhirlpoolValue(x, z) * heightScale;
			//Debug.Log("whirl");
		} else {
			height = this.transform.position.y + GetPerlinValue(x, z) * heightScale;
		}

		return height;
	}

	public float GetWhirlpoolValue(float x, float z) {
		// input in terms of world position
		if (whirlpoolsEnabled) {
			foreach (GameObject whirlpool in allWhirlpools) {
				Vector2 whirlpoolCenter = new Vector2(whirlpool.transform.position.x, whirlpool.transform.position.z);
				float whirlRadius = whirlpool.transform.lossyScale.x; 

				float dist = Vector2.Distance(whirlpoolCenter, new Vector2(x, z));
				if (dist <= whirlRadius) {
					float value = Mathf.Cos((whirlRadius - dist)/whirlRadius * Mathf.PI);
					return (value-1)/2;
				}
			}
		}

		return -999f;
	}

	public bool HasWhirlpool(float x, float z) {
		// input in terms of world position
		if (whirlpoolsEnabled) {
			foreach (GameObject whirlpool in allWhirlpools) {
				Vector2 whirlpoolCenter = new Vector2(whirlpool.transform.position.x, whirlpool.transform.position.z);
				float whirlRadius = whirlpool.transform.lossyScale.x; 

				float dist = Vector2.Distance(whirlpoolCenter, new Vector2(x, z));
				if (dist <= whirlRadius) {
					return true;
				}
			}
		}

		return false;
	}

	/*public void CreateWhirlpool(float xWhirl, float zWhirl, float radius) {
		float value, newHeight;
		Color newColor;
		Vector3 vertPosition;

		blockSize = this.transform.localScale.x / texSize;
		whirlCenter = new Vector2(xWhirl, zWhirl);
		whirlRadius = radius;
		for (int j = 0; j < baseTex.height; j++) {
			for (int i = 0; i < baseTex.width; i++) {
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
	}*/

	public void DestroyAllWhirlpools() {
		foreach (GameObject whirlpool in allWhirlpools) {
			Destroy(whirlpool);
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
