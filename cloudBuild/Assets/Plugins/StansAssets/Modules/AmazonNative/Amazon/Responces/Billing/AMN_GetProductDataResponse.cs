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

public class AMN_GetProductDataResponse : AMN_Result {

	private string _requestId = string.Empty;

	#if AMAZON_BILLING_ENABLED
	private Dictionary<string, ProductData> _productDataMap;
	#endif
	private List<string> _unavailableSkus = null;
	private string _status = string.Empty;

	#if AMAZON_BILLING_ENABLED
	public AMN_GetProductDataResponse(GetProductDataResponse data) : base(true) {
		_requestId = data.RequestId;
		_productDataMap = data.ProductDataMap;
		_unavailableSkus = data.UnavailableSkus;
		_status = data.Status;
	}
	#else
	public AMN_GetProductDataResponse() : base(true) {

	}
	#endif

	public string RequestId {
		get {
			return _requestId;
		}
	}
	#if AMAZON_BILLING_ENABLED
	public Dictionary<string, ProductData> ProductDataMap {
		get {
			return _productDataMap;
		}
	}
	#endif
	public List<string> UnavailableSkus {
		get {
			return _unavailableSkus;
		}
	}

	public string Status {
		get {
			return _status;
		}
	}
}
