////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;

public class AMN_InterstitialDataResult : AMN_Result {
	
	private AMN_AdProperties properties;
	private string _error_message = "no_error";
	
	public AMN_InterstitialDataResult(string error_message):base(false) {
		Error_message = error_message;
	}
	
	public AMN_InterstitialDataResult(string[] data):base(true) {
		Properties = new AMN_AdProperties(Convert.ToBoolean(data[0]), Convert.ToBoolean(data[1]), Convert.ToBoolean(data[2]), data[3]);
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public AMN_AdProperties Properties {
		get {
			return properties;
		}
		set {
			properties = value;
		}
	}
	
	public string Error_message {
		get {
			return _error_message;
		}
		set {
			_error_message = value;
		}
	}
}