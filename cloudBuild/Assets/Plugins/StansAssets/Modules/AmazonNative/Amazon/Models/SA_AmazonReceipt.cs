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

#if AMAZON_BILLING_ENABLED
using com.amazon.device.iap.cpt;
#endif

public class SA_AmazonReceipt {
	
	private string _sku = string.Empty;
	private string _productType= string.Empty;
	private string _receiptId= string.Empty;
	private long _purchaseDate = 0;
	private long _cancelDate = 0;

	#if AMAZON_BILLING_ENABLED

	public SA_AmazonReceipt(PurchaseReceipt data) {
		_sku 		  = data.Sku;
		_productType  = data.ProductType;
		_receiptId 	  = data.ReceiptId;
		_purchaseDate = data.PurchaseDate;
		_cancelDate   = data.CancelDate;
	}

	#endif
	
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

	public string ReceiptId {
		get {
			return _receiptId;
		}
	}


	public long PurchaseDate {
		get {
			return _purchaseDate;
		}
	}

	public long CancelDate {
		get {
			return _cancelDate;
		}
	}
}
