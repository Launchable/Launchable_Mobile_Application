////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class AMN_Result  {

	private bool _isSuccess = false;

	public AMN_Result(bool success) {
		_isSuccess = success;
	}

	public bool isSuccess  {
		get {
			return _isSuccess;
		}
	}
	
	public bool isFailure {
		get {
			return !isSuccess;
		}
	}
}
