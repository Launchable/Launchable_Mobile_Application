using UnityEngine;
using System;
using System.Collections;

public class UM_Editor_InAppClient : UM_BaseInAppClient, UM_InAppClient {

	private float _RequestsSuccessRate = 100f;
	private UM_InAppProduct _CurrentProduct = null;


	//--------------------------------------
	// Initialization
	//--------------------------------------

	public UM_Editor_InAppClient() {
		_RequestsSuccessRate = UltimateMobileSettings.Instance.InApps_EditorFillRate;
	}

	public void Connect() {

		SA.Common.Util.General.Invoke(UnityEngine.Random.Range(0.5f, 3f), () => {

			bool IsSucceeded = SA_EditorTesting.HasFill(_RequestsSuccessRate);
			UM_BillingConnectionResult r =  new UM_BillingConnectionResult();


			if(IsSucceeded) {
				_IsConnected = true;
				r.isSuccess = true;
				r.message = "Editor Testing Service Connected";
				SA_EditorNotifications.ShowNotification("Billing Connected", "Connection successful", SA_EditorNotificationType.Message);
			} else {
				r.isSuccess = false;
				r.message = "Connection failed";
				SA_EditorNotifications.ShowNotification("Billing Connection Failed", "Connection Failed", SA_EditorNotificationType.Error);

			}
			SendServiceConnectedEvent(r);

		});
	}
		

	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public override void Purchase(UM_InAppProduct product) {
		_CurrentProduct = product;
		SA_EditorInApps.ShowInAppPopup(product.DisplayName, product.Description, product.LocalizedPrice, OnPurchaseComplete);
	}

	public override void Subscribe(UM_InAppProduct product) {
		Purchase(product);
	}

	public override void Consume(UM_InAppProduct product)  {}

	public override void FinishTransaction(UM_InAppProduct product) {}

	public void RestorePurchases() {

		foreach(UM_InAppProduct product in UM_InAppPurchaseManager.InAppProducts) {
			if(product.IsPurchased) {
				UM_PurchaseResult r =  new UM_PurchaseResult();
				r.isSuccess = true;
				r.product = _CurrentProduct;

				SendPurchaseFinishedEvent(r);
			}
		}

		SA.Common.Util.General.Invoke(UnityEngine.Random.Range(0.5f, 3f), () => {

			UM_BaseResult result =  new UM_BaseResult();
			result.IsSucceeded = true;

			SendRestoreFinishedEvent(result);
		});
	}



	//--------------------------------------
	// Event Handlers
	//--------------------------------------

	private void OnPurchaseComplete(bool IsSucceeded) {

		UM_PurchaseResult r =  new UM_PurchaseResult();
		r.isSuccess = IsSucceeded;
		r.product = _CurrentProduct;

		SendPurchaseFinishedEvent(r);
	}


}
