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

public class AMN_GetPurchaseProductsUpdateResponse : AMN_Result {
		
	private string _requestId = string.Empty;
	private string _userId = string.Empty;
	private string _marketplace = string.Empty;
	#if AMAZON_BILLING_ENABLED
	private List<PurchaseReceipt> _receipts;
	#endif
	private string _status = string.Empty;
	private bool _hasMore = false;

	#if AMAZON_BILLING_ENABLED
	public AMN_GetPurchaseProductsUpdateResponse(GetPurchaseUpdatesResponse data) : base(true) {
		_requestId = data.RequestId;
		_userId = data.AmazonUserData.UserId;
		_marketplace = data.AmazonUserData.Marketplace;
		_receipts = data.Receipts;
		_status = data.Status;
		_hasMore = data.HasMore;
	}
	#else
	public AMN_GetPurchaseProductsUpdateResponse() : base(true) {
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
	#if AMAZON_BILLING_ENABLED
	public List<PurchaseReceipt> Receipts {
		get {
			return _receipts;
		}
	}
	#endif
	public string Status {
		get {
			return _status;
		}
	}

	public bool HasMore {
		get {
			return _hasMore;
		}
	}
}
