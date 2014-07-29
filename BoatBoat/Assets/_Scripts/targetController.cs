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
		Debug.Log("FLARE FIRED");
		flareObject = Instantiate (flare, transform.position + Vector3.up * 2.5f, transform.rotation) as GameObject;
		flareObject.rigidbody.velocity = Vector3.up * 100;
	}
}
