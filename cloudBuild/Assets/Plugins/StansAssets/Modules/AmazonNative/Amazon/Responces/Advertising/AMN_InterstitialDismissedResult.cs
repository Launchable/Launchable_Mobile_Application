////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

public class AMN_InterstitialDismissedResult : AMN_Result {
	
	private string _error_message = "no_error";
	
	public AMN_InterstitialDismissedResult(string error_message):base(false) {
		Error_message = error_message;
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public string Error_message {
		get {
			return _error_message;
		}
		set {
			_error_message = value;
		}
	}
}