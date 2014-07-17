using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {
	public GameObject explosion;
	public float lifetime;
	public float speed;
	
	void Start() {
		Shoot();
		Destroy (gameObject, lifetime);
	}

	void Update() {
		if (this.transform.position.y < 0) {
			Destroy(gameObject);
		}
	}

	public void Shoot() {
		this.rigidbody.velocity = this.transform.forward * speed;
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.name == "Terrain" || other.collider.name == "Enemy" || other.collider.name == "Target") {
			Destroy(gameObject);
		}
	}

	void OnDestroy() {
		Instantiate(explosion, transform.position, transform.rotation);
	}
}
