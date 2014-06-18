using UnityEngine;
using System.Collections;

public class SpoutMove : MonoBehaviour
{
	public float speed;
	public float waitSpawn;
	public Vector3 spoutHeight;
	public Vector3 minSpoutHeight;
	private bool goingUp = true;
	private int deathByCount = 0;
	public int deathByCountNumber;
	// Use this for initialization
	void Start ()
	{	
	//	waitSpawn = waitSpawn;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (waitSpawn < 0)
		{
			if (goingUp)
			{
				rigidbody.position += Vector3.up * speed * Time.deltaTime;
				if (rigidbody.position.y > spoutHeight.y)
				{
					goingUp = false;	
				}
			}
			else
			{
				rigidbody.position += Vector3.down * speed * Time.deltaTime;
				if (rigidbody.position.y < minSpoutHeight.y)
				{
					deathByCount += 1;
					goingUp = true;	
				}
			}

			if (deathByCount > deathByCountNumber)
			{
				Destroy (gameObject);
			}
		}
		else
		{
			waitSpawn -= Time.deltaTime;
		}

	
	}
}
