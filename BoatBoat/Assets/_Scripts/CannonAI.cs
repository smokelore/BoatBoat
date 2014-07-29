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
		reloadCountTarget = Random.Range(reloadDuration/2, reloadDuration * 1.5f);
	}
	
	void Update () {
		if (Vector3.Distance(this.transform.position, playerObject.transform.position) < 15f) {
			float angle;
			if (rightSide) {
				angle = Mathf.Abs(AngleSigned(shipObject.transform.right, playerObject.transform.position - this.transform.position, Vector3.up));
				if (loaded && Random.Range(0, Mathf.Pow(angle, 2)) < 5f) {
					Shoot();
				} else {
					this.transform.localEulerAngles = new Vector3(270f, 270f, 0f);
				}
			} else {
				angle = Mathf.Abs(AngleSigned(-shipObject.transform.right, playerObject.transform.position - this.transform.position, Vector3.up));
				if (loaded && Random.Range(0, Mathf.Pow(angle, 2)) < 5f) {
					Shoot();
				} else {
					this.transform.localEulerAngles = new Vector3(270f, 90f, 0f);
				}
			}
		}

		if (!loaded) {
			reloadCount += Time.deltaTime;
			if (reloadCount > reloadCountTarget) {
				loaded = true;
				reloadCount = 0f;
				reloadCountTarget = Random.Range(reloadDuration/2, reloadDuration * 1.5f);
			}
		}
	}

	private void Shoot() {
		Debug.Log("Enemy shot " + this.gameObject.name);
		cannonballTemp = Instantiate(cannonballPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
		cannonballTemp.GetComponent<CannonBall>().cannonOnRightSide = rightSide;
		smokeTemp = Instantiate(smokeShotPrefab, cannonballSpawn.position, cannonballSpawn.rotation) as GameObject;
		loaded = false;
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}
