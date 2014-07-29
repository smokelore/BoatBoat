using UnityEngine;
using System.Collections;

public class targetController : MonoBehaviour {

	public AudioClip target;
	public GameObject flare;
	private GameObject flareObject;

	// Use this for initialization
	void Start () {
		audio.Stop ();
		InvokeRepeating ("fireFlare", 1, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if(flareObject != null){
			if(flareObject.transform.position.y < 0){
				Destroy(flareObject);
			}
		}
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.name == "Ship"){
			audio.Play ();
			Camera.main.GetComponent<HUD>().winState = true;
		}
	}

	void fireFlare(){
		flareObject = Instantiate (flare, transform.position + new Vector3(0, 2.5f, 0), transform.rotation) as GameObject;
		flareObject.rigidbody.AddForce (new Vector3(0, 3000, 600));
	}
}
