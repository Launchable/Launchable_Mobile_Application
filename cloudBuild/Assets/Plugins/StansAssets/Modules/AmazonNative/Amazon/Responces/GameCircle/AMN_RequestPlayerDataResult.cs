////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class AMN_RequestPlayerDataResult : AMN_Result {
	
	private string error; 
	private GC_Player player;
	
	public AMN_RequestPlayerDataResult(bool success):base(success) {
		
	}
	
	public AMN_RequestPlayerDataResult(string err):base(false) {
		error = err;
	}

	public AMN_RequestPlayerDataResult(GC_Player pl):base(true) {
		player = pl;
	}
	
	public string Error {
		get {
			return error;
		}
	}

	public GC_Player Player {
		get {
			return player;
		}
	}
}