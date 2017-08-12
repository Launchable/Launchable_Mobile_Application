using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    [SerializeField]
    Image splashImage;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color finishColor;
    Color lerpedColor;

    [SerializeField]
    float initializeTime = 2;
    [SerializeField]
    float fadeTime = 2;

    private float t = 0; // lerp control variable

    bool fadeOut = false;

	void Start () {
        splashImage = this.GetComponent<Image>();
        StartCoroutine(WaitToFade());
	}
	
	// Update is called once per frame
	void Update () {

        if (fadeOut)
        {
            lerpedColor = Color.Lerp(startColor, finishColor, t);
            splashImage.color = lerpedColor;
            if (t < 1)
            {
                // while t below the end limit...
                // increment it at the desired rate every update:
                t += Time.deltaTime / fadeTime;
            }
            else
            {
                fadeOut = false;
            }
        }
    }

    IEnumerator WaitToFade()
    {
        yield return new WaitForSeconds(initializeTime);
        fadeOut = true;
    }
}

