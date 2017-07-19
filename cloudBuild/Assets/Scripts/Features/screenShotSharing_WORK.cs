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


	public GameObject intro1;
	public GameObject intro2;
	public GameObject intro3;


	private GUIStyle style;
	private GUIStyle style2;

	private Texture2D textureForPost;
	private Texture2D tex = null;
	private byte[] fileData;

	private bool check = false;


	/* --------------------------------------------------------------------------------------------------------- */

	// defind all the things need to be done first
	void Start() {

//		activeInterface ();
//		disableInterface();
		displayMessage();
		ImageHolder.SetActive (false);
		InitStyles();
	}

	void Update()
	{
		if (Input.GetMouseButton (0)) {
			noMessage ();
		}
	}


	private void displayMessage()
	{
		intro1.gameObject.SetActive (true);
		intro2.gameObject.SetActive (true);
		intro3.gameObject.SetActive (true);
	}

	private void noMessage()
	{
		intro1.gameObject.SetActive (false);
		intro2.gameObject.SetActive (false);
		intro3.gameObject.SetActive (false);
	}

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

	// take screenshot and load it on screen using rawImage object
	// before it got load to the screen diable interface and buttons
	public void captureScreenShotLoadOnScreen()
	{
		StartCoroutine (CaptureScreen ());
//		noMessage ();
	}
		
		
	private IEnumerator CaptureScreen()
	{
		shot = "_" + "screenshot" + "_" + System.DateTime.Now.ToString ("yyyy_MM_dd_hh_mm_ss") + ".png";
		newPath = Path.Combine (Application.persistentDataPath, shot);
	
		// Wait till the last possible moment before screen rendering to hide the UI
		yield return null;

		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
		disableInterface ();
		disableALLButtons ();

		// Wait for screen rendering to complete
		yield return new WaitForEndOfFrame();

		// Take screenshot
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			Application.CaptureScreenshot (shot);
			print ("got the shot 1!");
			check = true;

		} else {
			Application.CaptureScreenshot (newPath);
			print ("got the shot 2!");
			check = true;

		}
			
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
		disableInterface ();
		bt1.gameObject.SetActive (false);
	
//		Debug.Log (Directory.GetFiles (newPath));


		yield return new WaitForEndOfFrame();
		print ("another frame passed");

		if (check == true) {
			print ("SO TRUEEEEEEE");
			yield return new WaitForSeconds (1f);

			if (File.Exists (shot)) {
				print ("i found it");

				fileData = File.ReadAllBytes (newPath);

//				yield return new WaitForSeconds (0.2f);

				tex = new Texture2D (2, 2, TextureFormat.RGB24, false);
				tex.filterMode = FilterMode.Trilinear;
				textureForPost = tex;

				tex.LoadImage (fileData); //..this will auto-resize the texture dimensions.
				ImageHolder.SetActive (true);
				ImageHolder.GetComponent<RawImage> ().texture = tex;

				activeSomeButtons ();

			} 
			if (File.Exists (newPath)) {
				File.Delete (newPath);
				print ("let me delete it since i got it already");
				check = false;
			}
			else 
			{
				bt1.gameObject.SetActive (true);
			}
		}

	}

		
	/* --------------------------------------------------------------------------------------------------------- */

	// diable all the buttons and interface
	public void takeOutButtonFirst()
	{
		disableInterface ();
		disableALLButtons ();
	}

	// save screensho to the gallery
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

	// back to the beginning interface
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
		
	public void postTextureFB(){
		UM_ShareUtility.FacebookShare("#Launchable #AR", textureForPost);
	}

	public void postTextureTwitter() {
		UM_ShareUtility.TwitterShare("#Launchable #AR", textureForPost);
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
