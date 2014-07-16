using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {
	public GameObject explosion;
	public float lifetime;
	public float speed;
	
	void Start () {
		Shoot();
		Destroy (gameObject, lifetime);
	}

	public void Shoot() {
		this.rigidbody.velocity = this.transform.forward * speed;
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.tag != "Spout Sector") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
