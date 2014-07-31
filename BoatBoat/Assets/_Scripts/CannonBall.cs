// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {
	public GameObject explosion;
	public GameObject explosionWaterL;
	public GameObject explosionWaterR;
	public GameObject explosionBoat;
	public AudioClip landHit;
	public AudioClip waterHit;
	public AudioClip enemyHit;
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
		if (this.transform.position.y < 0f) {
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
		else if (other.gameObject.GetComponent<Ship>() != null) {
			hit = 2; // makes splinters
			Destroy(gameObject);
			other.gameObject.GetComponent<Ship>().ApplyDamage(damage);
		}
	}

	void OnDestroy() {
		if (hit == 0) {
			if (cannonOnRightSide) {
				Instantiate(explosionWaterR, transform.position, transform.rotation);
				//waterHit.volume = 0.5F;
				AudioSource.PlayClipAtPoint(waterHit, transform.position);
			} else {
				Instantiate(explosionWaterL, transform.position, transform.rotation);
				//waterHit.volume = 0.5F;
				//waterHit.Play();
				AudioSource.PlayClipAtPoint(waterHit, transform.position);
			}
		}
		else if (hit == 1){
			Instantiate(explosion, transform.position, transform.rotation);
			//landHit.volume = 0.5F;
			//landHit.Play();
			AudioSource.PlayClipAtPoint(landHit, transform.position);
		}
		else if (hit == 2){
			Instantiate(explosionBoat, transform.position, transform.rotation);
			//enemyHit.volume = 0.5F;
			//enemyHit.Play();
			AudioSource.PlayClipAtPoint(enemyHit, transform.position);
		}
	}
}
