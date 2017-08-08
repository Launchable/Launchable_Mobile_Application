using UnityEngine;
using System;
using System.Collections;

public class UM_WP8_InAppClient : UM_BaseInAppClient, UM_InAppClient {


	//--------------------------------------
	// Initialization
	//--------------------------------------

	public void Connect() {
		WP8InAppPurchasesManager.Instance.Init();

		WP8InAppPurchasesManager.OnInitComplete += OnInitComplete;
		WP8InAppPurchasesManager.OnPurchaseFinished += OnProductPurchased;
	}


	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public override void Purchase(UM_InAppProduct product) {
		WP8InAppPurchasesManager.Instance.Purchase(product.WP8Id);
	}

	public override void Subscribe(UM_InAppProduct product) {
		WP8InAppPurchasesManager.Instance.Purchase(product.WP8Id);
	}

	public override void Consume(UM_InAppProduct product)  { }
	public override void FinishTransaction(UM_InAppProduct product) {}
	public void RestorePurchases() { }



	//--------------------------------------
	// Event Handlers
	//--------------------------------------


	private void OnInitComplete(WP8_InAppsInitResult result) {

		_IsConnected = true;

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = "Inited";
		r.isSuccess = true;


		foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {


			WP8ProductTemplate tpl =  WP8InAppPurchasesManager.Instance.GetProductById(product.WP8Id);
			if(tpl != null) {
				product.SetTemplate(tpl);
				if(product.WP8Template.IsPurchased && !product.IsConsumable) {
					UM_InAppPurchaseManager.SaveNonConsumableItemPurchaseInfo(product);
				}
			}

		}

		SendServiceConnectedEvent(r);
	}

	private void OnProductPurchased(WP8PurchseResponce resp) {
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByWp8Id(resp.ProductId);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.product = p;
			r.WP8_PurchaseInfo = resp;
			r.isSuccess = resp.IsSuccses;

			SendPurchaseFinishedEvent(r);
		} else {
			SendNoTemplateEvent();
		}
	}

}
