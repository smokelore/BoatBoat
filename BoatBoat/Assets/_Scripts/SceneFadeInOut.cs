// TEAM:
// Mighty Morphin Pingas Rangers
// Sebastian Monroy - sebash@gatech.edu - smonroy3
// Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
// Chase Johnston - cjohnston8@gatech.edu - cjohnston8
// Jory Folker - jfolker10@outlook.com - jfolker3
using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour {
    public GameObject fadeWall;
    public float fadeSpeed;          // Speed that the screen fades to and from black.
    public float alphaTarget;
    private float alpha = 1f;
    public bool destroyWallOnTarget;
    public bool fading;      // Whether or not the scene is still fading in.
    private Color color;

    void Update() {
        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            Application.LoadLevel("playtestScene");
        }

        if (fading) {
            alpha += fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            color = Color.white;
            color.a = alpha;
            
            fadeWall.renderer.material.color = color;
            
            if ((fadeSpeed > 0 && alpha >= alphaTarget) || (fadeSpeed < 0 && alpha <= alphaTarget)) {
                fading = false;
            }
       }
    }
}