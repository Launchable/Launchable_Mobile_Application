using System;
using UnityEngine;
using System.Collections;


public interface UM_InAppClient  {

	//Actions
	event Action<UM_BillingConnectionResult>  OnServiceConnected;
	event Action<UM_PurchaseResult> OnPurchaseFinished;
	event Action<UM_BaseResult> OnRestoreFinished;


	/// <summary>
	/// Connect to Android InApp service
	/// ActionBillingSetupFinished Action fired when connect
	/// is complete
	/// </summary>
	void Connect();


	/// <summary>
	/// Start purchase flow for product
	/// ActionProductPurchased Action fired when flow
	/// is complete
	/// </summary>
	/// <param name="productId">product Id you want to purchase</param>
	void Purchase(string productId);


	/// <summary>
	/// Start purchase flow for product
	/// ActionProductPurchased Action fired when flow
	/// is complete
	/// </summary>
	/// <param name="product">product you want to purchase</param>
	void Purchase(UM_InAppProduct product);

	/// <summary>
	/// Start subscribe flow for product
	/// ActionProductPurchased Action fired when flow
	/// is complete
	/// </summary>
	/// <param name="product">product you want to purchase</param>
	void Subscribe(UM_InAppProduct product);


	/// <summary>
	/// Start purchase flow for product
	/// ActionProductPurchased Action fired when flow
	/// is complete
	/// </summary>
	/// <param name="productId">product Id you want to purchase</param>
	void Subscribe(string productId);


	/// <summary>
	/// Start consume flow for product
	/// ActionProductConsumed Action fired when flow
	/// is complete
	/// </summary>
	/// <param name="productId">product Id you want to consume</param>
	void Consume(string productId);


	/// <summary>
	/// Start consume flow for product
	/// ActionProductConsumed Action fired when flow
	/// is complete
	/// </summary>
	/// <param name="product">product you want to consume</param>
	void Consume(UM_InAppProduct product);


	/// <summary>
	/// The Method will finish trsansaction for a product
	/// <param name="productId">target product Id</param>
	/// </summary>
	void FinishTransaction(string productId);


	/// <summary>
	///  The Method will finish trsansaction for a product
	/// <param name="product">target product</param>
	/// </summary>
	void FinishTransaction(UM_InAppProduct product);


	/// <summary>
	/// Restores purchases made by current user
	/// OnPurchaseFinished Action will be  fired for eatch previously purchaed product
	/// When restore flow is complete, OnRestoreFinished action fired.
	/// </summary>
	void RestorePurchases();


	/// <summary>
	/// Returns true if product with provided id owned by user
	/// <param name="productId">product Id</param>
	/// </summary>
	bool IsProductPurchased(string productId);


	/// <summary>
	/// Returns true if provided product  owned by user
	/// <param name="productId">product object</param>
	/// </summary>
	bool IsProductPurchased(UM_InAppProduct product);


	/// <summary>
	/// Can be used to determine if app is connection
	/// to the Android billing services 
	/// </summary>
	bool IsConnected {get;}


}
