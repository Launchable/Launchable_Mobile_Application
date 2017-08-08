//#define AMAZON_BILLING_ENABLED


////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if AMAZON_BILLING_ENABLED
using com.amazon.device.iap.cpt;
#endif

public class SA_AmazonBillingManager : AMN_Singleton<SA_AmazonBillingManager> {

	public event Action<AMN_GetUserDataResponse> OnGetUserDataReceived = delegate {};
	public event Action<AMN_PurchaseResponse> OnPurchaseProductReceived = delegate {};
	public event Action<AMN_GetProductDataResponse> OnGetProductDataReceived = delegate {};
	public event Action<AMN_GetPurchaseProductsUpdateResponse> OnGetPurchaseProductsUpdatesReceived = delegate {};	

	public string currentSKU = "";
	private bool _isInitialized = false;

	#if AMAZON_BILLING_ENABLED

	private IAmazonIapV2 iapService;

	#endif

	public Dictionary<string, AmazonProductTemplate> availableItems;
	public List<string> unavailableSkus = null;
	public List<SA_AmazonReceipt> listReceipts = null;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public enum status {
		SUCCESSFUL,
		FAILED
	}

	void Awake() {

		#if AMAZON_BILLING_ENABLED

		// Obtain object used to interact with plugin
		iapService = AmazonIapV2Impl.Instance;

		SubscribeToEvents ();

		DontDestroyOnLoad(gameObject);

		#endif
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public bool IsInitialized {
		get {
			return _isInitialized;
		}
	}
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------

	#if AMAZON_BILLING_ENABLED

	private void GetUserDataHandler(GetUserDataResponse data) {
		AMN_GetUserDataResponse result = new AMN_GetUserDataResponse (data);

		OnGetUserDataReceived(result);
	}

	private void PurchaseProductHandler(PurchaseResponse data) {
		AMN_PurchaseResponse result;

		if (data.Status.Equals (status.SUCCESSFUL.ToString())) {			
			result = new AMN_PurchaseResponse (data);
			iapService.NotifyFulfillment(new NotifyFulfillmentInput() {
				ReceiptId = data.PurchaseReceipt.ReceiptId,
				FulfillmentResult = "FULFILLED"
			});
		} else {
			result = new AMN_PurchaseResponse (data, currentSKU);
		}

		OnPurchaseProductReceived(result);
	}

	private void GetProductDataHandler(GetProductDataResponse data)	{		
		availableItems = new Dictionary<string, AmazonProductTemplate>();

		// for each item in the productDataMap you can get the following values for all your SKU`s
		foreach(KeyValuePair<string, ProductData> entry in data.ProductDataMap) {
			AmazonProductTemplate product = new AmazonProductTemplate(entry.Value);
			availableItems.Add(entry.Key, product);
		}

		unavailableSkus = data.UnavailableSkus;

		AMN_GetProductDataResponse result = new AMN_GetProductDataResponse (data);
		OnGetProductDataReceived(result);
	}

	private void GetPurchaseUpdatesHandler(GetPurchaseUpdatesResponse data) {
		listReceipts = new List<SA_AmazonReceipt> ();

		foreach (PurchaseReceipt rec in data.Receipts) {
			SA_AmazonReceipt receipt = new SA_AmazonReceipt(rec);
			listReceipts.Add(receipt);		
		}
		
		AMN_GetPurchaseProductsUpdateResponse result = new AMN_GetPurchaseProductsUpdateResponse (data);
		OnGetPurchaseProductsUpdatesReceived (result);
	}

	#endif

	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------

	public void Initialize() {
		Initialize(AmazonNativeSettings.Instance.InAppProducts);
	}

	public void Initialize(List<AmazonProductTemplate> product_ids) {
		if(!_isInitialized) {
			Init(product_ids);
		}
	}

	public void AddProduct(string sku) {
		AmazonProductTemplate product = new AmazonProductTemplate() {Sku = sku};
		int index = IsExistsInSettings(product);
		if (index != -1) {
			AmazonNativeSettings.Instance.InAppProducts.RemoveAt(index);
		}
		AmazonNativeSettings.Instance.InAppProducts.Add(product);
		Debug.Log("AddProduct(string sku)" + sku);
	}
		
	public void GetUserData ()	{

		#if AMAZON_BILLING_ENABLED

		// Call synchronous operation with no input
		RequestOutput response = iapService.GetUserData();
		
		// Get return value
		string requestIdString = response.RequestId;
	
		Debug.Log ("requestIdString " + requestIdString);
		#endif
	}

	public void Purchase(string SKU) {
		#if AMAZON_BILLING_ENABLED
		currentSKU = SKU;

		// Construct object passed to operation as input
		SkuInput request = new SkuInput();
		
		// Set input value
		request.Sku = SKU;
		
		// Call synchronous operation with input object
		RequestOutput response = iapService.Purchase(request);
		
		// Get return value
		string requestIdString = response.RequestId;

		Debug.Log ("requestIdString " + requestIdString);

		#endif
	}

	public void GetProductUpdates () {
		#if AMAZON_BILLING_ENABLED
		// Construct object passed to operation as input
		ResetInput request = new ResetInput();
		
		// Set input value
		request.Reset = true;
		
		// Call synchronous operation with input object
		RequestOutput response = iapService.GetPurchaseUpdates(request);
		
		// Get return value
		string requestIdString = response.RequestId;

		Debug.Log ("requestIdString " + requestIdString);
		#endif
	}
	
	//--------------------------------------
	// PRIVATE API CALL METHODS
	//--------------------------------------

	private void Init(List<AmazonProductTemplate> product_ids) {
		#if AMAZON_BILLING_ENABLED
		List<string> products = new List<string> ();
		foreach(AmazonProductTemplate tpl in product_ids) {
			products.Add(tpl.Sku);
		}

		SkusInput request = new SkusInput();

		request.Skus = products;
		
		// Call synchronous operation with input object
		RequestOutput response = iapService.GetProductData(request);
		
		// Get return value
		string requestIdString = response.RequestId;		
		
		Debug.Log ("requestIdString " + requestIdString);
		#endif
	}

	private void SubscribeToEvents () {
		#if AMAZON_BILLING_ENABLED
		iapService.AddGetUserDataResponseListener(GetUserDataHandler);
		iapService.AddPurchaseResponseListener(PurchaseProductHandler);
		iapService.AddGetProductDataResponseListener(GetProductDataHandler);
		iapService.AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesHandler);
		#endif
	}

	private int IsExistsInSettings(AmazonProductTemplate product) {
		foreach(AmazonProductTemplate p in AmazonNativeSettings.Instance.InAppProducts) {
			if (p.Sku.Equals(product.Sku)) {
				return AmazonNativeSettings.Instance.InAppProducts.IndexOf(p);
			}
		}

		return -1;
	}
}
