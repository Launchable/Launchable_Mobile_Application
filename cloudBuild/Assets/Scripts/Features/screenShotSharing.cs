using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class screenShotSharing : MonoBehaviour {

	// variables
	private string newPath;
	private string shot;

	public Button capture;
	public Button save;
	public Button cancel;
	public Button bigCancel;
	public Button facebook;
	public Button twitter;

	public GameObject launchable;
	public GameObject findTarget;
	public GameObject flash;
	public GameObject ImageHolder;
	public GameObject bigFlash;

//	public GameObject _PanelCoaching1;
//	public GameObject _PanelCoaching2;
//	public GameObject _PanelCoaching3;

	public Button currentPhone;
	public Button currentEmail;

	private GUIStyle style;
	private GUIStyle style2;

	private Texture2D screenCap;
	private Texture2D border;

	public bool noAnimation;
	public bool noPhoneEmailButtons;

//	analyticsController analyticsControl;
			
	/* --------------------------------------------------------------------------------------------------------- */

	// defind all the things need to be done first
	void Start() {

		InitStyles();
		ImageHolder.SetActive (false);
		screenCap = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false); // 1
//		analyticsControl = GameObject.FindObjectOfType<analyticsController> ();

		/*
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
		*/
	}
		

	/* --------------------------------------------------------------------------------------------------------- */
	/*
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
	*/
	/* --------------------------------------------------------------------------------------------------------- */

	// define all buttons activity
	private void disableInterface()
	{
		launchable.SetActive (false);
		bigFlash.SetActive (false);
		findTarget.SetActive (false);
		flash.gameObject.SetActive (false);
	}

	private void activeInterface ()
	{
		launchable.SetActive (true);
		bigFlash.SetActive (true);
		findTarget.SetActive (true);
		flash.SetActive (true);
	}

	private void disableALLButtons()
	{
		capture.gameObject.SetActive (false);
		save.gameObject.SetActive (false);
		cancel.gameObject.SetActive (false);
		bigCancel.gameObject.SetActive (false);
		facebook.gameObject.SetActive (false);
		twitter.gameObject.SetActive (false);
	}
		
	private void activeSomeButtons()
	{
		save.gameObject.SetActive (true);
		cancel.gameObject.SetActive (true);
		bigCancel.gameObject.SetActive (true);
		facebook.gameObject.SetActive (true);
		twitter.gameObject.SetActive (true);
	}

	/* --------------------------------------------------------------------------------------------------------- */

	// catch the screen shot and load it to the screen
	public void captureScreen()
	{
		noAnimation = true;
		noPhoneEmailButtons = true;
		StartCoroutine ("Capture");
	}

	IEnumerator Capture(){

		disableInterface ();
		disableALLButtons ();

		if(currentPhone != null){
			currentPhone.gameObject.SetActive (false);
			print ("phone should be gone now");
		}
		if (currentEmail != null) {
			currentEmail.gameObject.SetActive (false);
			print ("email should be gone now");
		}
			
		yield return new WaitForEndOfFrame();

		screenCap.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		screenCap.Apply ();

		ImageHolder.SetActive (true);
		ImageHolder.GetComponent<RawImage> ().texture = screenCap;
//		analyticsControl.screenshotTaken();
		
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
		capture.gameObject.SetActive (true);

		ImageHolder.SetActive (false);
		save.gameObject.SetActive (false);
		cancel.gameObject.SetActive (false);
		facebook.gameObject.SetActive (false);
		twitter.gameObject.SetActive (false);

		noAnimation = false;
		noPhoneEmailButtons = false;
	}

	// facebook sharing
	public void postTextureFB(){
		UM_ShareUtility.FacebookShare("@thebridebox " + "#thebridebox", screenCap);
//		analyticsControl.screenshotShare("facebook");
	}

	// twitter sharing
	public void postTextureTwitter() {
		UM_ShareUtility.TwitterShare("@thebridebox " + "#thebridebox", screenCap);
//		analyticsControl.screenshotShare("twitter");
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
