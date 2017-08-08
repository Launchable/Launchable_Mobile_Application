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
public class AMN_GetUserDataResponse : AMN_Result {
	
	private string _requestId 	= string.Empty;
	private string _userId 		= string.Empty;
	private string _marketplace = string.Empty;
	private string _status      = string.Empty;
	#if AMAZON_BILLING_ENABLED
	public AMN_GetUserDataResponse(GetUserDataResponse data):base(true) {
		_requestId   = data.RequestId;
		_userId      = data.AmazonUserData.UserId;
		_marketplace = data.AmazonUserData.Marketplace;
		_status      = data.Status;
	}
	#else
	public AMN_GetUserDataResponse():base(true) {
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

	public string Status {
		get {
			return _status;
		}
	}
}
