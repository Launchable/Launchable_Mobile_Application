using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UM_Amazon_InAppClient :  UM_BaseInAppClient, UM_InAppClient { 




	//--------------------------------------
	// Initialization
	//--------------------------------------

	public void Connect() {
		SA_AmazonBillingManager.Instance.Initialize();

		SA_AmazonBillingManager.Instance.OnGetProductDataReceived += HandleAmazonGetProductDataReceived;
		SA_AmazonBillingManager.Instance.OnGetPurchaseProductsUpdatesReceived += HandleAmazonGetPurchaseProductsUpdatesReceived;
		SA_AmazonBillingManager.Instance.OnGetUserDataReceived += HandleAmazonGetUserDataReceived;
		SA_AmazonBillingManager.Instance.OnPurchaseProductReceived += HandleAmazonPurchaseProductReceived;
	}


	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public override void Purchase(UM_InAppProduct product) {
		SA_AmazonBillingManager.Instance.Purchase(product.AmazonId);
	}

	public override void Subscribe(UM_InAppProduct product) {
		SA_AmazonBillingManager.Instance.Purchase(product.AmazonId);
	}

	public override void Consume(UM_InAppProduct product)  {
		
	}
		
	public override void FinishTransaction(UM_InAppProduct product) {
		
	}


	public void RestorePurchases() {
		SA_AmazonBillingManager.Instance.GetProductUpdates();
	}



		

	//--------------------------------------
	// Event Handlers
	//--------------------------------------


	private void HandleAmazonPurchaseProductReceived (AMN_PurchaseResponse response) {
		Debug.Log("[Amazon] HandleAmazonPurchaseProductReceived");

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAmazonId(response.Sku);
		if(p != null) {
			UM_PurchaseResult result = new UM_PurchaseResult();
			result.Amazon_PurchaseInfo = response;
			result.product = p;
			result.isSuccess = response.isSuccess;

			SendPurchaseFinishedEvent(result);

		} else {
			SendNoTemplateEvent();
		}

	}



	private void HandleAmazonGetPurchaseProductsUpdatesReceived (AMN_GetPurchaseProductsUpdateResponse response) {
	/*	Debug.Log("[Amazon] HandleAmazonGetPurchaseProductsUpdatesReceived");

		if (response.isSuccess) {
			foreach (KeyValuePair<string, string> product in response.Products) {
				if (IsAmazonProductExist(product.Key)) {
					UM_PurchaseResult result = new UM_PurchaseResult(product.Value);
					result.isSuccess = response.isSuccess;
					result.product = UltimateMobileSettings.Instance.GetProductByAmazonId(product.Key);

					SendPurchaseEvent(result);
				} else {
					SendNoTemplateEvent();
				}
			}
		}

*/

		UM_BaseResult res = new UM_BaseResult();
		res.IsSucceeded = response.isSuccess;

		SendRestoreFinishedEvent(res);
	}




	private void HandleAmazonGetProductDataReceived (AMN_GetProductDataResponse response) {
		Debug.Log("[Amazon] HandleAmazonGetProductDataReceived");

		_IsConnected = response.isSuccess;

		if (response.isSuccess) {
			foreach (UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				if (SA_AmazonBillingManager.Instance.availableItems.ContainsKey(product.AmazonId)) {
					product.SetTemplate(SA_AmazonBillingManager.Instance.availableItems[product.AmazonId]);
				}
			}
		}

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.isSuccess = response.isSuccess;
		r.message = response.Status;

		SendServiceConnectedEvent(r);
	}

	private void HandleAmazonGetUserDataReceived (AMN_GetUserDataResponse response) {
		Debug.Log("[Amazon] HandleAmazonGetUserDataReceived");
	}




		
}
