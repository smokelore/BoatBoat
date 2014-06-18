using UnityEngine;
using System.Collections;

public class SpwanSpout : MonoBehaviour
{
	public GameObject spouts;
	private bool playerEnter = false;
	public float waveWait;
	public float spawnWait;
	//private bool hasSpout = false;
	//private bool created = false;
	//public float waitRespawn;
	//private float countDown;
	
	// Use this for initialization
	void Start () 
	{
		//countDown = waitRespawn;
		StartCoroutine (SpawnWaves ());
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerEnter = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			playerEnter = false;
		}
	}
	
	
	IEnumerator SpawnWaves () 
	{
		while (true)
		{
			while (playerEnter)
			{
				Vector3 spawnPosition = new Vector3 (Random.Range (transform.position.x - collider.bounds.extents.x, transform.position.x + collider.bounds.extents.x),
				                                     -2, Random.Range (transform.position.z - collider.bounds.extents.z, transform.position.z + collider.bounds.extents.z));

				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (spouts, spawnPosition, spawnRotation);
				//hasSpout = true;
				//countDown = waitRespawn;
				yield return new WaitForSeconds (waveWait);

			}
			yield return new WaitForSeconds (spawnWait);
		}
	}











//	// Update is called once per frame
//	void Update () 
//	{
//		Quaternion spawnRotation = Quaternion.identity;
//		GameObject clone = (GameObject)Instantiate (spouts, Vector3.zero, spawnRotation);
//		clone.SetActive (false);
//		if (!hasSpout && playerEnter)
//		{
//			Vector3 spawnPosition = new Vector3 (Random.Range (transform.position.x - collider.bounds.extents.x, transform.position.x + collider.bounds.extents.x),
//			                                     -2, Random.Range (transform.position.z - collider.bounds.extents.z, transform.position.z + collider.bounds.extents.z));
//			if (!created)
//			{
////				Quaternion spawnRotation = Quaternion.identity;
////				GameObject obj = (GameObject)Instantiate (spouts, spawnPosition, spawnRotation);
//				created = true;
//			}
//			else
//			{
//
//				spouts.transform = spawnPosition;
//				spouts.SetActive(true);
//			}
//			hasSpout = true;
//			countDown = waitRespawn;
//		}
//		else
//		{
//			if (hasSpout)
//			{
//				countDown -= Time.deltaTime;
//			}
//			//return;
//		}
//		if (countDown < 0)
//		{
////			spouts.SetActive(false);
//			Destroy(clone);
//			hasSpout = false;
//		}
//	}
	
}
