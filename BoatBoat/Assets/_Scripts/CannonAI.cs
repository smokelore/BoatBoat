using UnityEngine;
using System.Collections;

public class CannonAI : MonoBehaviour {
	public GameObject shipObject;
	private enemyController ec;
	private GameObject playerObject;
	public bool rightSide;
	public GameObject cannonballPrefab;
	public GameObject smokeShotPrefab;
	public Transform cannonballSpawn;
	private GameObject cannonballTemp;
	private GameObject smokeTemp;
	public float reloadDuration;
	private float reloadCountTarget;
	private float reloadCount;
	public bool loaded, canShoot;

	void Start () {
		playerObject = GameObject.FindWithTag("Player");
		ec = shipObject.GetComponent<enemyController>();
		reloadCountTarget = Random.Range(reloadDuration, reloadDuration * 3f);
	}
	
	void Update () {
		if (Vector3.Distance(this.transform.position, playerObject.transform.position) < 15f) {
			if ((rightSide && Mathf.Abs(AngleSigned(shipObject.transform.right, playerObject.transform.position - this.transform.position, Vector3.up)) < 20f) ||
				(!rightSide && Mathf.Abs(AngleSigned(-shipObject.transform.right, playerObject.transform.position - this.transform.position, Vector3.up)) < 20f)) {
				//this.transform.LookAt(playerObject.transform.position, shipObject.transform.up);
				if (loaded) {
					Shoot();
				}
			} else {
				if (rightSide) {
					this.transform.localEulerAngles = new Vector3(270f, 270f, 0f);
				} else {
					this.transform.localEulerAngles = new Vector3(270f, 90f, 0f);
				}
			}
		}

		if (!loaded) {
			reloadCount += Time.deltaTime;
			if (reloadCount > reloadCountTarget) {
				loaded = true;
				reloadCount = 0;
			}
		}
	}

	private void Shoot() {
		Debug.Log("Enemy shot " + this.gameObject.name);
		cannonballTemp = Instantiate(cannonballPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
		cannonballTemp.GetComponent<CannonBall>().cannonOnRightSide = rightSide;
		smokeTemp = Instantiate(smokeShotPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
		loaded = false;
		reloadCountTarget = Random.Range(reloadDuration, reloadDuration * 3f);
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}
