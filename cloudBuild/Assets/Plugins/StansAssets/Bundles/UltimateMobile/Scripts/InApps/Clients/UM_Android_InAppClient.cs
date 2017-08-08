using UnityEngine;
using System;
using System.Collections;



public class UM_Android_InAppClient : UM_BaseInAppClient, UM_InAppClient {


	//--------------------------------------
	// Initialization
	//--------------------------------------


	public void Connect() {
		AndroidInAppPurchaseManager.Client.Connect();

		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;	

		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
		//AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
	}



	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public override void Purchase(UM_InAppProduct product) {
		AndroidInAppPurchaseManager.Client.Purchase(product.AndroidId);
	}

	public override void Subscribe(UM_InAppProduct product) {
		AndroidInAppPurchaseManager.Client.Subscribe(product.AndroidId);
	}

	public override void Consume(UM_InAppProduct product)  {
		AndroidInAppPurchaseManager.Client.Consume (product.AndroidId);
	}


	public override void FinishTransaction(UM_InAppProduct product) {

	}


	public override bool IsProductPurchased(UM_InAppProduct product) {
		if(product ==  null) {return false;}
		if(AndroidInAppPurchaseManager.Client.Inventory == null) {return false;}

		return AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(product.AndroidId);
	}

	public void RestorePurchases() {
		//not supported for this platfroms
	}


	//--------------------------------------
	// Event Handlers
	//--------------------------------------


	private void OnProductPurchased (BillingResult result) {
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAndroidId(result.Purchase.SKU);

		if(p != null) {

			if(UltimateMobileSettings.Instance.TransactionsHandlingMode == UM_TransactionsHandlingMode.Automatic) {
				if(p.IsConsumable && result.IsSuccess) {
					AndroidInAppPurchaseManager.Client.Consume(result.Purchase.SKU);
				} 
			} 

			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = result.IsSuccess;
			r.product = p;
			r.SetResponceCode(result.Response);
			r.Google_PurchaseInfo = result.Purchase;

			SendPurchaseFinishedEvent(r);


		} else {
			SendNoTemplateEvent();
		}
	}	

	/*private void OnProductConsumed(BillingResult result) {
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAndroidId(result.Purchase.SKU);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = result.IsSuccess;
			r.product = p;
			r.SetResponceCode(result.Response);
			r.Google_PurchaseInfo = result.Purchase;
			SendPurchaseFinishedEvent(r);
		} else {
			SendNoTemplateEvent();
		}
	}*/



	private void OnBillingConnected(BillingResult result) {

		if(result.IsSuccess) {
			AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
			AndroidInAppPurchaseManager.Client.RetrieveProducDetails();

		} else {
			UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
			r.isSuccess = false;
			r.message = result.Message;
			SendServiceConnectedEvent(r);
		}
	}


	private void OnRetrieveProductsFinised(BillingResult result) {


		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = result.Message;
		r.isSuccess = result.IsSuccess;

		_IsConnected = r.isSuccess;

		if(r.isSuccess) {
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				GoogleProductTemplate tpl = AndroidInAppPurchaseManager.Client.Inventory.GetProductDetails(product.AndroidId);
				if(tpl != null) {
					product.SetTemplate(tpl);
					if(product.IsConsumable && AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(product.AndroidId)) {
						AndroidInAppPurchaseManager.Client.Consume(product.AndroidId);
					}

					if(!product.IsConsumable && AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(product.AndroidId)) {
						UM_InAppPurchaseManager.SaveNonConsumableItemPurchaseInfo(product);
					}
				}
			}
		}
			
		SendServiceConnectedEvent(r);

	}


}
