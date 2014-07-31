using UnityEngine;
using System.Collections;

public class targetController : MonoBehaviour {

	public AudioClip target;
	public GameObject flare;
	private GameObject flareObject;
	public GameObject lid;
	private int openCount;

	public AudioClip flareSound;
	public AudioClip firework1;
	public AudioClip firework2;
	public AudioClip firework3;
	public AudioClip firework4;
	public AudioClip firework5;
	public float vol = 1.0f;
	private int soundNum;

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
		if(Camera.main.GetComponent<HUD>().winState && openCount < 20){
			openTreasure ();
		}
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.name == "Player Ship"){
			audio.Play ();
			Camera.main.GetComponent<HUD>().winState = true;
			CancelInvoke ();
			Celebrate();
		}
	}

	void fireFlare(){
		flareObject = Instantiate (flare, transform.position + new Vector3(0, 0.1f, 0), transform.rotation) as GameObject;
		flareObject.rigidbody.AddForce (new Vector3(0, 3000, 600));
		AudioSource.PlayClipAtPoint (flareSound, transform.position, vol);
		Destroy(flareObject, 6f);
	}

	void fireRando(){
		flareObject = Instantiate (flare, transform.position + new Vector3(0, 0.1f, 0), transform.rotation) as GameObject;
		flareObject.rigidbody.AddForce (new Vector3(Random.Range (-600, 601), Random.Range (1000,2001), Random.Range(-600, 601)));
		AudioSource.PlayClipAtPoint (flareSound, transform.position, vol);
		Destroy(flareObject, 6f);
	}

	void openTreasure(){
		lid.transform.Rotate (Vector3.right * Time.deltaTime , -3);
		openCount++;
	}

	void Celebrate(){
		InvokeRepeating("fireRando", 1, 0.2f);
		InvokeRepeating ("ExplosionsInTheSky", 2.5f, 0.2f);
	}

	void ExplosionsInTheSky(){
		soundNum = Random.Range (1, 6);
		if(soundNum == 1){
			AudioSource.PlayClipAtPoint (firework1, transform.position, vol);
		}else if(soundNum == 2){
			AudioSource.PlayClipAtPoint (firework2, transform.position, vol);
		}else if(soundNum == 3){
			AudioSource.PlayClipAtPoint (firework3, transform.position, vol);
		}else if(soundNum == 4){
			AudioSource.PlayClipAtPoint (firework4, transform.position, vol);
		}else if(soundNum == 5){
			AudioSource.PlayClipAtPoint (firework5, transform.position, vol);
		}
	}
}
