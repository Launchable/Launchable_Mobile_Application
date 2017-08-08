using UnityEngine;
using System;
using System.Collections;

using SA.IOSNative.StoreKit;

public class UM_IOS_InAppClient : UM_BaseInAppClient, UM_InAppClient {

	//--------------------------------------
	// Initialization
	//--------------------------------------

	public void Connect() {


		PaymentManager.Instance.LoadStore();

		PaymentManager.OnStoreKitInitComplete += IOS_OnStoreKitInitComplete;
		PaymentManager.OnTransactionComplete  += IOS_OnTransactionComplete;
		PaymentManager.OnRestoreComplete += IOS_OnRestoreComplete;
	}


	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public override void Purchase(UM_InAppProduct product) {
		PaymentManager.Instance.BuyProduct(product.IOSId);
	}

	public override void Subscribe(UM_InAppProduct product) {
		PaymentManager.Instance.BuyProduct(product.IOSId);
	}
		
	public override void Consume(UM_InAppProduct product)  {
		
	}


	public override void FinishTransaction(UM_InAppProduct product)  {
		PaymentManager.Instance.FinishTransaction (product.IOSId);
	}


	public void RestorePurchases() {
		PaymentManager.Instance.RestorePurchases();
	}




	//--------------------------------------
	// Event Handlers
	//--------------------------------------


	private void IOS_OnTransactionComplete (PurchaseResult responce) {

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByIOSId(responce.ProductIdentifier);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.product = p;
			r.IOS_PurchaseInfo = responce;


			switch(r.IOS_PurchaseInfo.State) {
				case PurchaseState.Purchased:
				case PurchaseState.Restored:
					r.isSuccess = true;
					break;
				case PurchaseState.Deferred:
				case PurchaseState.Failed:
					r.isSuccess = false;
					break;
			}

			SendPurchaseFinishedEvent(r);
		} else {
			SendNoTemplateEvent();
		}


	}

	private void IOS_OnStoreKitInitComplete (SA.Common.Models.Result res) {

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		_IsConnected = res.IsSucceeded;
		r.isSuccess = res.IsSucceeded;
		if(res.IsSucceeded) {
			r.message = "Inited";

			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {

				Product tpl = PaymentManager.Instance.GetProductById(product.IOSId); 
				if(tpl != null) {
					product.SetTemplate(tpl);
				}

			}

			SendServiceConnectedEvent(r);
		} else {

			if(res.Error != null) {
				r.message = res.Error.Message;
			}

			SendServiceConnectedEvent(r);
		}

	}

	void IOS_OnRestoreComplete (RestoreResult res) {
		Debug.Log("IOS_OnRestoreComplete");

		UM_BaseResult result =  new UM_BaseResult();
		result.IsSucceeded = res.IsSucceeded;

		SendRestoreFinishedEvent(result);
	}	


}
