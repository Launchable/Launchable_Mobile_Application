using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class screenShotSharing_WORK : MonoBehaviour {

	// variables
	private string newPath;
	private string shot;

	public Button bt1;
	public Button bt2;
	public Button bt3;
	public Button bt4;
	public Button bt5;


	public GameObject launchable;
	public GameObject line;
	public GameObject findTarget;
	public GameObject flash;

	public GameObject ImageHolder;

	public GameObject _PanelCoaching1;
	public GameObject _PanelCoaching2;
	public GameObject _PanelCoaching3;


	private GUIStyle style;
	private GUIStyle style2;

	private Texture2D screenCap;
	private Texture2D border;

	analyticsController analyticsControl;
			
	/* --------------------------------------------------------------------------------------------------------- */

	// defind all the things need to be done first
	void Start() {

		InitStyles();
		ImageHolder.SetActive (false);
		screenCap = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false); // 1
		analyticsControl = GameObject.FindObjectOfType<analyticsController> ();
		// only display the coaching once 
		if (PlayerPrefs2.GetBool("messagesAlreadyShown"))
		{
			noUI ();
		}
		else
		{
			PlayerPrefs2.SetBool("messagesAlreadyShown", true);
			SetupUI ();
		}

	}
		

	/* --------------------------------------------------------------------------------------------------------- */

	// define all coaching object activity
	public void HideCoaching ()
	{
		noUI ();
	}

	private void SetupUI ()
	{
		_PanelCoaching1.SetActive (true);
		_PanelCoaching2.SetActive (true);
		_PanelCoaching3.SetActive (true);
	}

	private void noUI(){
		_PanelCoaching1.SetActive (false);
		_PanelCoaching2.SetActive (false);
		_PanelCoaching3.SetActive (false);
	}

	/* --------------------------------------------------------------------------------------------------------- */

	// define all buttons activity
	private void disableInterface()
	{
		launchable.SetActive (false);
		line.SetActive (false);
		findTarget.SetActive (false);
		flash.gameObject.SetActive (false);
	}

	private void activeInterface ()
	{
		launchable.SetActive (true);
		line.SetActive (true);
		findTarget.SetActive (true);
		flash.SetActive (true);
	}

	private void disableALLButtons()
	{
		bt1.gameObject.SetActive (false);
		bt2.gameObject.SetActive (false);
		bt3.gameObject.SetActive (false);
		bt4.gameObject.SetActive (false);
		bt5.gameObject.SetActive (false);
	}
		
	private void activeSomeButtons()
	{
		bt2.gameObject.SetActive (true);
		bt3.gameObject.SetActive (true);
		bt4.gameObject.SetActive (true);
		bt5.gameObject.SetActive (true);
	}

	/* --------------------------------------------------------------------------------------------------------- */

	// catch the screen shot and load it to the screen
	public void captureScreen()
	{
		StartCoroutine ("Capture");
	}

	IEnumerator Capture(){

		disableInterface ();
		disableALLButtons ();

		yield return new WaitForEndOfFrame();

		screenCap.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		screenCap.Apply ();

		ImageHolder.SetActive (true);
		ImageHolder.GetComponent<RawImage> ().texture = screenCap;
		analyticsControl.screenshotTaken();
		
		yield return new WaitForEndOfFrame();

		activeSomeButtons ();
	}
				
	/* --------------------------------------------------------------------------------------------------------- */

	// diable all the buttons and interface
	public void takeOutButtonFirst()
	{
		disableInterface ();
		disableALLButtons ();
	}

	// save screenshot to the gallery
	public void savePicToGallery(){
		UM_Camera.Instance.OnImageSaved += OnImageSaved;
		UM_Camera.Instance.SaveScreenshotToGallery();

	}

	void OnImageSaved (UM_ImageSaveResult result) {

		if(result.IsSucceeded) {
			//no image path for IOS
			MNPopup popup = new MNPopup ("Image Saved", result.imagePath);
			popup.AddAction ("Ok", () => {});
			popup.Show ();

			activeSomeButtons ();

		} else {
			MNPopup popup = new MNPopup ("Failed", "Image Save Failed");
			popup.AddAction ("Ok", () => {});
			popup.Show ();

			activeSomeButtons ();
		}
	}

	/* --------------------------------------------------------------------------------------------------------- */

	// go back to the interface
	public void backToInterface()
	{
		activeInterface ();
		bt1.gameObject.SetActive (true);

		ImageHolder.SetActive (false);
		bt2.gameObject.SetActive (false);
		bt3.gameObject.SetActive (false);
		bt4.gameObject.SetActive (false);
		bt5.gameObject.SetActive (false);

	}

	// facebook sharing
	public void postTextureFB(){
		UM_ShareUtility.FacebookShare("#Launchable #AR", screenCap);
		analyticsControl.screenshotShare("facebook");
	}

	// twitter sharing
	public void postTextureTwitter() {
		UM_ShareUtility.TwitterShare("#Launchable #AR", screenCap);
		analyticsControl.screenshotShare("twitter");
	}
		
	private void InitStyles () {
		style =  new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 16;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.alignment = TextAnchor.UpperLeft;
		style.wordWrap = true;


		style2 =  new GUIStyle();
		style2.normal.textColor = Color.white;
		style2.fontSize = 12;
		style2.fontStyle = FontStyle.Italic;
		style2.alignment = TextAnchor.UpperLeft;
		style2.wordWrap = true;
	}
}
