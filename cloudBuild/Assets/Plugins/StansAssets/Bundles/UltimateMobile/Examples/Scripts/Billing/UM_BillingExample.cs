////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UM_BillingExample : BaseIOSFeaturePreview {

	public const string CONSUMABLE_PRODUCT_ID 		= "coins_bonus";
	public const string NON_CONSUMABLE_PRODUCT_ID 	=	"coins_pack";

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------	

	void Awake() {
		UM_ExampleStatusBar.text = "Unified billing exmple scene loaded";

		UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
		UM_InAppPurchaseManager.Client.OnServiceConnected += OnConnectFinished;
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	void OnGUI() {
		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "In-App Purchases", style);

		StartY+= YLableStep;


		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Init")) {

			//subscribign on intit fisigh action
			UM_InAppPurchaseManager.Client.OnServiceConnected += OnBillingConnectFinishedAction;
			UM_InAppPurchaseManager.Client.Connect();
			UM_ExampleStatusBar.text = "Initializing billing...";
		}


		if(UM_InAppPurchaseManager.Client.IsConnected) {
			GUI.enabled = true;
		} else  {
			GUI.enabled = false;
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Buy Consumable Item")) {
			UM_InAppPurchaseManager.Client.Purchase(CONSUMABLE_PRODUCT_ID);

			UM_ExampleStatusBar.text = "Start purchsing " + CONSUMABLE_PRODUCT_ID + " product";
		}

		StartX += XButtonStep;


		bool e = GUI.enabled;
		string msg = "";
		if(UM_InAppPurchaseManager.Client.IsProductPurchased(NON_CONSUMABLE_PRODUCT_ID)) {
			msg = "Already purchased";
			GUI.enabled = false;
		} else {
			msg = "Not yet purchased";
		}
		
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Buy Non-Consumable Item \n" + msg)) {
			UM_ExampleStatusBar.text = "Start purchsing " + NON_CONSUMABLE_PRODUCT_ID + " product";

			UM_InAppPurchaseManager.Client.Purchase(NON_CONSUMABLE_PRODUCT_ID);
		}

		GUI.enabled = e;

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Restore Purshases \n For IOS Only")) {
			UM_InAppPurchaseManager.Client.RestorePurchases ();
		}



	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnConnectFinished(UM_BillingConnectionResult result) {

		if(result.isSuccess) {
			UM_ExampleStatusBar.text = "Billing init Success";
		} else  {
			UM_ExampleStatusBar.text = "Billing init Failed";
		}
	}

	private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
		UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
		if(result.isSuccess) {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Success";
		} else  {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Failed";
		}
	}

	private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) {
		UM_InAppPurchaseManager.Client.OnServiceConnected -= OnBillingConnectFinishedAction;
		if(result.isSuccess) {
			Debug.Log("Connected");
		} else {
			Debug.Log("Failed to connect");
		}
	}


	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------



}
