// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;

public class PlayersReady : MonoBehaviour {
	public Player[] allPlayers = new Player[4];
	private int numEnabled, numReady;
	public SceneFadeInOut fader;
	private bool allReady;

	// Use this for initialization
	void Start () {
		fader = Camera.main.GetComponent<SceneFadeInOut>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!allReady) {
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
				allReady = true;
				fader.alphaTarget = 1f;
				fader.fading = true;
				fader.fadeSpeed = 0.5f;
			}
		} else {
			if (!fader.fading) {
				Application.LoadLevel("playtestScene");
			}
		}
	}
}
