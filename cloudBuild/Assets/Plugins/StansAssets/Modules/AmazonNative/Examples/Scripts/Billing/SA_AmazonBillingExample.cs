////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SA_AmazonBillingExample : MonoBehaviour  {
	
	public DefaultPreviewButton[] buttons;

	private List<string> entitlements;

	private Dictionary<string, AmazonProductTemplate> availableItems;
	private List<string> unavailableSkus;
	
	private List<SA_AmazonReceipt> listReceipts;
	
	private SA_AmazonReceipt receipt;

	private bool isInitialized = false;

	//replase with your SKU id
	private string SKU_EXAMPLE = "first_item";
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Start() {
		entitlements = AMN_PlayerData.GetAvailableSKUs ();
				
		DisableButtons ();
		
		SA_AmazonBillingManager.Instance.OnGetUserDataReceived += OnGetUserDataReceived;
		SA_AmazonBillingManager.Instance.OnPurchaseProductReceived += OnPurchaseProductReceived;
		SA_AmazonBillingManager.Instance.OnGetProductDataReceived += OnGetProductDataReceived;
		SA_AmazonBillingManager.Instance.OnGetPurchaseProductsUpdatesReceived += OnGetPurchaseProductsUpdatesReceived;
	}	
	
	void OnGUI() {
		if (isInitialized) {
			EnableButtons();
		}
	}
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	void OnGetUserDataReceived(AMN_GetUserDataResponse result) {
		string requestId   = result.RequestId;
		string userId 	   = result.UserId;
		string marketplace = result.Marketplace;
		string status      = result.Status;
		
		SA_StatusBar.text = "GetUserData status " + result.Status;

		Debug.Log(requestId + " " + userId + " " + marketplace + " " + status);
	}
	
	void OnPurchaseProductReceived (AMN_PurchaseResponse result) {
		if(result.isSuccess) {
			string _requestId = result.RequestId;
			string _userId = result.UserId;
			string _marketplace = result.Marketplace;
			string _receiptId = result.ReceiptId;
			long _cancelDate = result.CancelDate;
			long _purchaseDate = result.PurchaseDatee;
			string _sku = result.Sku;
			string _productType = result.ProductType;
			string _status = result.Status;

			SA_StatusBar.text = "PurchaseProduct status " + result.Status;

			Debug.Log (_requestId + " " + _userId + " " + _marketplace + " " + _receiptId + " " + _cancelDate + " " + _purchaseDate + " " + _sku + " " + _productType + " " + _status);
		} else {
			string _requestId = result.RequestId;
			string _status = result.Status;

			SA_StatusBar.text = "PurchaseProduct status " + result.Status;

			Debug.Log ("_status " + _status + " _requestId " + _requestId);
		}
	}
	
	void OnGetProductDataReceived (AMN_GetProductDataResponse result) {
		isInitialized = true;

		string requestId = result.RequestId;
		string status = result.Status;

		availableItems  = SA_AmazonBillingManager.Instance.availableItems;			
		unavailableSkus = SA_AmazonBillingManager.Instance.unavailableSkus;

		SA_StatusBar.text = "OnGetProductData status " + result.Status;

		Debug.Log(availableItems + " " + status + " " + requestId + " " + unavailableSkus);
	}

	void OnGetPurchaseProductsUpdatesReceived (AMN_GetPurchaseProductsUpdateResponse result) {
		string _requestId 	= result.RequestId;
		string _userId		= result.UserId;
		string _marketplace = result.Marketplace;
		string _status 	    = result.Status;
		bool _hasMore 	    = result.HasMore;

		listReceipts = SA_AmazonBillingManager.Instance.listReceipts;

		foreach(SA_AmazonReceipt receipt in listReceipts) {
			string _sku         = receipt.Sku;
			string _productType = receipt.ProductType;
			string _receiptId   = receipt.ReceiptId;
			long _purchaseDate  = receipt.PurchaseDate;
			long _cancelDate    = receipt.CancelDate;

			Debug.Log(_sku + " " + _productType + " " + _receiptId + " " + _purchaseDate + " " + _cancelDate);
		}

		Debug.Log(_requestId + " " + _userId + " " + _marketplace + " " + _status + " " + _hasMore + " " + listReceipts);
	}
			
	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE API CALL METHODS
	//--------------------------------------
	
	private void InitializeAmazonBilling() {
		SA_StatusBar.text = "Initializing Amazon Billing";
		SA_AmazonBillingManager.Instance.Initialize();
	}
	
	private void DisableButtons() {
		foreach(DefaultPreviewButton button in buttons) {
			button.DisabledButton();
		}
	}
	
	private void EnableButtons() {
		foreach(DefaultPreviewButton button in buttons) {
			button.EnabledButton();
		}
	}
	
	private void Purchase() {
		if(entitlements.Contains(SKU_EXAMPLE)) {
			Debug.Log("Already buyed!");
			return;
		}
		
		SA_AmazonBillingManager.Instance.Purchase (SKU_EXAMPLE);
	}

	private void GetUserData() {
		SA_AmazonBillingManager.Instance.GetUserData ();
	}

	private void GetProductUpdates() {
		SA_AmazonBillingManager.Instance.GetProductUpdates ();
	}
	
	private void AddEntitlement(string SKU) {
		if(!entitlements.Contains(SKU)){
			
			entitlements.Add(SKU);
			
			AMN_PlayerData.AddNewSKU(SKU);
		}
	}
}
