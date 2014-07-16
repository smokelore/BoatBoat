using UnityEngine;
using System.Collections;

public class CannonBallExplosion : MonoBehaviour {
	public float duration;

	void Start () {
		Destroy(this.gameObject, duration);
	}
}