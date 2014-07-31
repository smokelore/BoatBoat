// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;

public class SpawnSpout : MonoBehaviour
{
	public GameObject spouts;
//	public float waveWait;
//	public float spawnWait;
	//private bool hasSpout = false;
	//public float waitRespawn;
	//private float countDown;
	
	// Use this for initialization
	void Start () 
	{
		//countDown = waitRespawn;
		//StartCoroutine (SpawnWaves ());
		this.gameObject.renderer.enabled = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			InvokeRepeating ("MakeSpout", 1, 5);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			CancelInvoke("MakeSpout");
		}
	}

	// Invoke repeating function of spawing spouts
	void MakeSpout()
	{
		Vector3 spawnPosition = new Vector3 (Random.Range (transform.position.x - collider.bounds.extents.x, transform.position.x + collider.bounds.extents.x),
		                                     -3, Random.Range (transform.position.z - collider.bounds.extents.z, transform.position.z + collider.bounds.extents.z));
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate (spouts, spawnPosition, spawnRotation);
	}

	// Update is called once per frame
	void Update () 
	{
//		if (playerEnter)
//		{
//			hasSpout = true;
//			countDown = waitRespawn;
//		}
//		else
//		{
//			if (hasSpout)
//			{
//				countDown -= Time.deltaTime;
//			}
//		}
//		if (countDown < 0)
//		{
//			hasSpout = false;
//		}
	}



	
	//	IEnumerator SpawnWaves () 
	//	{
	//		while (true)
	//		{
	//			while (playerEnter)
	//			{
	//				Vector3 spawnPosition = new Vector3 (Random.Range (transform.position.x - collider.bounds.extents.x, transform.position.x + collider.bounds.extents.x),
	//				                                     -2, Random.Range (transform.position.z - collider.bounds.extents.z, transform.position.z + collider.bounds.extents.z));
	//
	//				Quaternion spawnRotation = Quaternion.identity;
	//				Instantiate (spouts, spawnPosition, spawnRotation);
	//				//hasSpout = true;
	//				//countDown = waitRespawn;
	//				yield return new WaitForSeconds (waveWait);
	//
	//			}
	//			yield return new WaitForSeconds (spawnWait);
	//		}
	//	}

	
}