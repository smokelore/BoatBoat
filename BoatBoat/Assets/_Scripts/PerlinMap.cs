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
	public Texture2D normalTex, baseTex;

	public GameObject[] blocks;
	
	public ParticleSystem.Particle[] particles;
	public float particleScale;

	public Material verticesMaterial;
	public MeshFilter terrainMesh;
	public Vector3[] verts;
	public List<int> tris;
	public Vector2[] uvs;
	public Color[] colors;
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
		normalTex = new Texture2D(texSize, texSize);
		baseTex = new Texture2D(texSize, texSize);
		pixels = new Color[normalTex.height*normalTex.width];

		blocks = new GameObject[normalTex.height*normalTex.width];

		particles = new ParticleSystem.Particle[normalTex.height*normalTex.width];

		verts = new Vector3[normalTex.height*normalTex.width];
		tris = new List<int>();
		uvs = new Vector2[normalTex.height*normalTex.width];
		colors = new Color[normalTex.height*normalTex.width];
		heights = new float[normalTex.height*normalTex.width];

		blockSize = this.transform.localScale.x / texSize;
		for (int j = 0; j < normalTex.height; j++) {
			float zCoord = this.transform.position.z - this.transform.localScale.z/2 + j*blockSize + blockSize/2;
			for (int i = 0; i < normalTex.width; i++) {
				float xCoord = this.transform.position.x - this.transform.localScale.x/2 + i*blockSize + blockSize/2;
				GameObject block;

				switch (RenderMode) {
					case Mode.Bumps:
						this.renderer.enabled = true;
						renderer.material = bumpsMaterial;
						renderer.material.SetTexture("_BumpMap", normalTex);
						break;
					case Mode.Blocks:
						block = Instantiate(blockPrefab, new Vector3(xCoord, this.transform.position.y, zCoord), Quaternion.identity) as GameObject;
						block.transform.localScale *= blockSize;
						block.name = i + " " + j;
						blocks[j * normalTex.width + i] = block;
						break;
					case Mode.Pillars:
						block = Instantiate(blockPrefab, new Vector3(xCoord, this.transform.position.y, zCoord), Quaternion.identity) as GameObject;
						block.transform.localScale *= blockSize;
						block.name = i + " " + j;
						blocks[j * normalTex.width + i] = block;
						break;
					case Mode.Particles:
						particles[j * normalTex.width + i].position = new Vector3(xCoord, this.transform.position.y, zCoord);
						particles[j * normalTex.width + i].color = lowColor;
						particles[j * normalTex.width + i].size = particleScale;
						break;
					case Mode.Vertices:
						this.renderer.enabled = true;
						renderer.material = verticesMaterial;
						this.gameObject.renderer.material.SetTextureScale("_MainTex",new Vector2(1/this.transform.localScale.x,1/this.transform.localScale.z))  ;
						renderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));

						verts[j * normalTex.width + i] = new Vector3(xCoord/this.transform.localScale.x, this.transform.position.y, zCoord/this.transform.localScale.z);
						uvs[j * normalTex.width + i] = new Vector2(xCoord, zCoord);
						colors[j * normalTex.width + i] = lowColor;

						if (i+1 >= normalTex.width || j+1 >= normalTex.width) {
							continue;
						}
						tris.Add(i + j*normalTex.width);
						tris.Add(i + (j+1)*normalTex.width);
						tris.Add((i+1) + j*normalTex.width);

						tris.Add(i + (j+1)*normalTex.width);
						tris.Add((i+1) + (j+1)*normalTex.width);
						tris.Add((i+1) + j*normalTex.width);
						break;
					default:

						break;
				}
				
			}
		}

		if (RenderMode == Mode.Vertices) {
			//this.transform.RotateAround(this.transform.position, Vector3.right, 180);
			Mesh ret = new Mesh();
	        ret.vertices = verts;
	        ret.triangles = tris.ToArray();
	        ret.uv = uvs;

	        ret.RecalculateBounds();
	        ret.RecalculateNormals();
	        terrainMesh.mesh = ret;
		}

		xOffset = this.transform.position.x;
		yOffset = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		CalculateNoise();
	}

	public void CalculateNoise() {
		for (int y = 0; y < normalTex.height; y++) {
			for (int x = 0; x < normalTex.width; x++) {
				float value = Mathf.PerlinNoise(timeScale * Time.time + x * pixelScale + xOffset, timeScale * Time.time + y * pixelScale + yOffset);
				pixels[y * normalTex.width + x] = new Color(value, value, value);
				Color newColor =  lowColor * (1-Mathf.Pow(value,colorExponent)) + highColor * (Mathf.Pow(value,colorExponent));
				colors[y * normalTex.width + x] = newColor;
				//baseTex.SetPixel(x, y*normalTex.width, newColor);

				float newHeight =  this.transform.position.y + value * heightScale;
				heights[y * normalTex.width + x] = newHeight;

				Vector3 blockScale, blockPosition;
				Vector3 particlePosition;
				Vector3 vertPosition;

				switch (RenderMode) {
					case Mode.Blocks:
						blockScale = blocks[y * normalTex.width + x].transform.localScale;
						blockPosition = blocks[y * normalTex.width + x].transform.position;
						blocks[y * normalTex.width + x].transform.position = new Vector3(blockPosition.x, newHeight, blockPosition.z);
						blocks[y * normalTex.width + x].renderer.material.color = newColor;
						break;
					case Mode.Pillars:
						blockScale = blocks[y * normalTex.width + x].transform.localScale;
						blockPosition = blocks[y * normalTex.width + x].transform.position;
						blocks[y * normalTex.width + x].transform.localScale = new Vector3(blockScale.x, newHeight, blockScale.z);
						blocks[y * normalTex.width + x].transform.position = new Vector3(blockPosition.x, blockScale.y/2, blockPosition.z);
						blocks[y * normalTex.width + x].renderer.material.color = newColor;
						break;
					case Mode.Particles:
						particlePosition = particles[y * normalTex.width + x].position;
						particles[y * normalTex.width + x].position = new Vector3(particlePosition.x, newHeight, particlePosition.z);
						particles[y * normalTex.width + x].color = newColor;
						particles[y * normalTex.width + x].size = particleScale;
						break;
					case Mode.Vertices:
						vertPosition = verts[y * normalTex.width + x];
						verts[y * normalTex.width + x] = new Vector3(vertPosition.x, newHeight, vertPosition.z);
						break;
					default:

						break;
				}
				//Debug.Log(Time.time * timeScale);
			}
		}

		if (RenderMode == Mode.Bumps) {
			normalTex.SetPixels(pixels);
			normalTex.Apply();
			renderer.material.SetTexture("_BumpMap", normalTex);
		} else if (RenderMode == Mode.Particles) {
			this.particleSystem.SetParticles(particles, particles.Length);
		} else if (RenderMode == Mode.Vertices) {
			normalTex.SetPixels(pixels);
			normalTex.Apply();
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
	}

	public float GetHeight(float x, float y) {
		//Debug.Log(x + " " + y);
		int newY = (int) (y + this.transform.localScale.z/2);
		int newX = (int) (x + this.transform.localScale.x/2);
		newY = (int) Mathf.Round(newY / blockSize);
		newX = (int) Mathf.Round(newX / blockSize);
		//Debug.Log(newX + "/" + newY);

		return heights[newY * normalTex.width + newX];
	}
}
