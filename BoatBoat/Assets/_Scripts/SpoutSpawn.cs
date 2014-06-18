using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public GameObject spouts;
	private bool hasSpout = false;
	private bool playerEnter = false;

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Spout")
		{
			hasSpout = true;
		}
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
		if (other.tag == "Spout")
		{
			hasSpout = false;
		}
	}


	// Update is called once per frame
	void Update () 
	{
		if (!hasSpout && playerEnter)
		{
			Vector3 spawnPosition = new Vector3 (Random.Range (transform.position.x - collider.bounds.extents.x, transform.position.x + collider.bounds.extents.x),
			                                     -2, Random.Range (transform.position.z - collider.bounds.extents.z, transform.position.z + collider.bounds.extents.z));
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (spouts, spawnPosition, spawnRotation);
		}
		else
		{
			return;
		}
	}

}
