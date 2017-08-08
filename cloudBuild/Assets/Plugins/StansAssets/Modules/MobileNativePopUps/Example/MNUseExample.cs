////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MNUseExample : MNFeaturePreview {

	public string appleId = "itms-apps://itunes.apple.com/id375380948?mt=8";
	public string androidAppUrl = "market://details?id=com.google.earth";

	void Awake() {

	}
	
	void OnGUI() {
		
		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Pop Ups", style);
		StartY+= YLableStep;

		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events")) {

			MNRateUsPopup rateUs = new MNRateUsPopup ("rate us", "rate us, please", "Rate Us", "No, Thanks", "Later");
			rateUs.SetAppleId (appleId);
			rateUs.SetAndroidAppUrl (androidAppUrl);
			rateUs.AddDeclineListener (() => { Debug.Log("rate us declined"); });
			rateUs.AddRemindListener (() => { Debug.Log("remind me later"); });
			rateUs.AddRateUsListener (() => { Debug.Log("rate us!!!"); });
			rateUs.AddDismissListener (() => { Debug.Log("rate us dialog dismissed :("); });
			rateUs.Show ();

		}		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp")) {			

			MNPopup popup = new MNPopup ("title", "dialog message");
			popup.AddAction ("action1", () => {Debug.Log("action 1 action callback");});
			popup.AddAction ("action2", () => {Debug.Log("action 2 action callback");});
			popup.AddDismissListener (() => {Debug.Log("dismiss listener");});
			popup.Show ();

		}		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp")) {
						
			MNPopup popup = new MNPopup ("title", "dialog message");
			popup.AddAction ("Ok", () => {Debug.Log("Ok action callback");});
			popup.AddDismissListener (() => {Debug.Log("dismiss listener");});
			popup.Show ();
			
		}

		StartY += YButtonStep;
		StartX = XStartPos;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Prealoder")) {
			MNP.ShowPreloader("Title", "Message");
			Invoke("OnPreloaderTimeOut", 3f);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Hide Prealoder")) {
			MNP.HidePreloader();
		}
		
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnPreloaderTimeOut() {
		MNP.HidePreloader();
	}

}
