using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {
	public GameObject explosion;
	public GameObject explosionWaterL;
	public GameObject explosionWaterR;
	public GameObject explosionBoat;
	private int hit = 0;
	public float lifetime;
	public float speed;
	public float damage;
	public bool cannonOnRightSide;
	
	void Start() {
		Shoot();
		Destroy (gameObject, lifetime);
	}

	void Update() {
		if (this.transform.position.y < 0) {
			hit = 0; //makes splash
			Destroy(gameObject);
		}
	}

	public void Shoot() {
		this.rigidbody.velocity = this.transform.forward * speed;
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.name == "Terrain" || other.collider.name == "Target") {
			hit = 1; // makes smoke
			Destroy(gameObject);
		}
		else if (other.collider.gameObject.GetComponent<Ship>() != null){
			other.collider.gameObject.GetComponent<Ship>().ApplyDamage(damage);
			hit = 2; // makes splinters
			Destroy(gameObject);
		}
	}

	void OnDestroy() {
		if (hit == 0) {
			if (cannonOnRightSide) {
				Instantiate(explosionWaterR, transform.position, transform.rotation);
			} else {
				Instantiate(explosionWaterL, transform.position, transform.rotation);
			}
		}
		else if (hit == 1){
			Instantiate(explosion, transform.position, transform.rotation);
		}
		else if (hit == 2){
			Instantiate(explosionBoat, transform.position, transform.rotation);
		}
	}
}
