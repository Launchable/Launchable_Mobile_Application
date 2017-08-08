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

public class AMN_InitializeResult : AMN_Result {

	private string error; 

	public AMN_InitializeResult(bool success):base(success) {
		
	}

	public AMN_InitializeResult(string err):base(false) {
		error = err;
	}

	public string Error {
		get {
			return error;
		}
	}
}
