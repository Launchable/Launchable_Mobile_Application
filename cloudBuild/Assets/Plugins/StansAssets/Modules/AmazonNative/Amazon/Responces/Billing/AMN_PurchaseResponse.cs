//#define AMAZON_BILLING_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if AMAZON_BILLING_ENABLED
using com.amazon.device.iap.cpt;
#endif
public class AMN_PurchaseResponse : AMN_Result {

	private string _requestId	= string.Empty;
	private string _userId		= string.Empty;
	private string _marketplace	= string.Empty;
	private string _receiptId	= string.Empty;
	private long _cancelDate 	= 0;
	private long _purchaseDate 	= 0;
	private string _sku			= string.Empty;
	private string _productType	= string.Empty;
	private string _status		= string.Empty;

	public AMN_PurchaseResponse() : base(true) {
		
	}

	#if AMAZON_BILLING_ENABLED
	public AMN_PurchaseResponse(PurchaseResponse data) : base(true) {
		_requestId = data.RequestId;
		_userId = data.AmazonUserData.UserId;
		_marketplace = data.AmazonUserData.Marketplace;
		_status = data.Status;

		if(data.PurchaseReceipt != null) {			
			_receiptId = data.PurchaseReceipt.ReceiptId;
			_cancelDate = data.PurchaseReceipt.CancelDate;
			_purchaseDate = data.PurchaseReceipt.PurchaseDate;
			_sku = data.PurchaseReceipt.Sku;
			_productType = data.PurchaseReceipt.ProductType;
		}
	}

	public AMN_PurchaseResponse(PurchaseResponse data, string sku) : base(false) {
		_requestId = data.RequestId;
		_userId = data.AmazonUserData.UserId;
		_marketplace = data.AmazonUserData.Marketplace;
		_status = data.Status;
		_sku = sku;
	}

	#endif

	public string RequestId {
		get {
			return _requestId;
		}
	}
	
	public string UserId {
		get {
			return _userId;
		}
	}
	
	public string Marketplace {
		get {
			return _marketplace;
		}
	}

	public string ReceiptId {
		get {
			return _receiptId;
		}
	}

	public long CancelDate {
		get {
			return _cancelDate;
		}
	}

	public long PurchaseDatee {
		get {
			return _purchaseDate;
		}
	}

	public string Sku {
		get {
			return _sku;
		}
	}

	public string ProductType {
		get {
			return _productType;
		}
	}

	public string Status {
		get {
			return _status;
		}
	}
}
