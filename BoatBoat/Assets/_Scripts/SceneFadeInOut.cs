using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour {
    public GameObject fadeInWall;
    public float fadeSpeed = 0.5f;          // Speed that the screen fades to and from black.
    private float alpha = 1f;
    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private Color color;

    void Update() {
        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            Application.LoadLevel("playtestScene");
        }

        if (sceneStarting) {
            alpha -= fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            color = Color.white;
            color.a = alpha;
            
            fadeInWall.renderer.material.color = color;
            
            if (alpha == 1f) {
                sceneStarting = false;
            }
       }
    }
}