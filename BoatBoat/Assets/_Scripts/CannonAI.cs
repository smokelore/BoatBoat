using UnityEngine;
using System.Collections;

public class CannonAI : MonoBehaviour {
	public GameObject shipObject;
	private enemyController ec;
	private GameObject playerObject;
	public bool rightSide;
	public GameObject cannonObject;
	public GameObject cannonballPrefab;
	public GameObject smokeShotPrefab;
	public Transform cannonballSpawn;
	private GameObject cannonballTemp;
	private GameObject smokeTemp;
	public bool loaded, canShoot;

	void Start () {
		playerObject = GameObject.FindWithTag("Player");
		ec = shipObject.GetComponent<enemyController>();
	}
	
	void Update () {
		if (Vector3.Distance(this.transform.position, playerObject.transform.position) < 20f) {
			if (Mathf.Abs(AngleSigned(shipObject.transform.right, playerObject.transform.position - this.transform.position, Vector3.up)) < 30f) {
				this.transform.LookAt(playerObject.transform.position);
			}
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}
