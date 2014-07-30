using UnityEngine;
using System.Collections;

public class PlayersReady : MonoBehaviour {
	public Player[] allPlayers = new Player[4];
	public int numEnabled, numReady;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		numEnabled = 0;
		foreach (Player p in allPlayers) {
			if (!p.isDisabled()) {
				numEnabled++;
			}
		}

		numReady = 0;
		foreach (Player p in allPlayers) {
			if (p.GetComponent<IntroController>().ready) {
				numReady++;
			}
		}

		if (numReady == numEnabled && numReady > 0) {
			Application.LoadLevel("playtestScene");
		}
	}
}
