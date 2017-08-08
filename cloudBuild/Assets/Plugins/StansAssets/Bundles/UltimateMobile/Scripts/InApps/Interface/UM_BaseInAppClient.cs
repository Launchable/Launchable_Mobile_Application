using UnityEngine;
using System;
using System.Collections;

public abstract class UM_BaseInAppClient  {


	public event Action<UM_BillingConnectionResult>  	OnServiceConnected 	= delegate {};
	public event Action<UM_PurchaseResult> 				OnPurchaseFinished 	= delegate {};
	public event Action<UM_BaseResult> 					OnRestoreFinished 	= delegate {};


	protected bool _IsConnected = false;


	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public void Purchase(string productId) {
		UM_InAppProduct product = UM_InAppPurchaseManager.GetProductById(productId);
		if(product != null) {
			Purchase(product);
		} else {
			SendNoTemplateEvent();
		}
	}

	public abstract void Purchase(UM_InAppProduct product) ;

	public void Subscribe(string productId) {
		UM_InAppProduct product = UM_InAppPurchaseManager.GetProductById(productId);
		if(product != null) {
			Subscribe(product);
		} else {
			SendNoTemplateEvent();
		}
	}

	public abstract void Subscribe(UM_InAppProduct product) ;


	public void Consume(string productId) {
		UM_InAppProduct product = UM_InAppPurchaseManager.GetProductById(productId);
		if(product != null) {
			Consume(product);
		} else {
			SendNoTemplateEvent();
		}
	}

	public abstract void Consume(UM_InAppProduct product) ;


	public void FinishTransaction(string productId) {
		UM_InAppProduct product = UM_InAppPurchaseManager.GetProductById(productId);
		if(product != null) {
			FinishTransaction(product);
		} else {
			SendNoTemplateEvent();
		}
	}

	public abstract void FinishTransaction(UM_InAppProduct product) ;




	public bool IsProductPurchased(string productId) {
		UM_InAppProduct product = UM_InAppPurchaseManager.GetProductById(productId);
		if(product != null) {
			return IsProductPurchased(product);
		} else {
			return false;
		}
	}

	public virtual bool IsProductPurchased(UM_InAppProduct product) {
		return UM_InAppPurchaseManager.IsLocalPurchaseRecordExists(product);
	}


	//--------------------------------------
	// Get / Set
	//--------------------------------------

	public bool IsConnected {
		get {
			return _IsConnected;
		}
	}





	//--------------------------------------
	// Protected Methods
	//--------------------------------------

	protected void SendNoTemplateEvent() {
		Debug.LogWarning("UM: Product tamplate not found");
		UM_PurchaseResult r =  new UM_PurchaseResult();
		SendPurchaseFinishedEvent(r);
	}


	protected void SendServiceConnectedEvent(UM_BillingConnectionResult e) {
		OnServiceConnected(e);
	}

	protected void SendPurchaseFinishedEvent(UM_PurchaseResult e) {
		OnPurchaseFinished(e);
	}

	protected void SendRestoreFinishedEvent(UM_BaseResult e) {
		OnRestoreFinished(e);
	}


}
