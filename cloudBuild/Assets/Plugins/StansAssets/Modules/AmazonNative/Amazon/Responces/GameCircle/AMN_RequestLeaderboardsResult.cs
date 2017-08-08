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

public class AMN_RequestLeaderboardsResult : AMN_Result {
	
	private string error;
	private List<GC_Leaderboard> leaderboardsList = null;
	
	public AMN_RequestLeaderboardsResult(bool success):base(success) {
		
	}
	
	public AMN_RequestLeaderboardsResult(string err):base(false) {
		error = err;
	}
	
	public AMN_RequestLeaderboardsResult(List<GC_Leaderboard> list):base(true) {
		leaderboardsList = list;
	}
	
	public string Error {
		get {
			return error;
		}
	}

	public List<GC_Leaderboard> LeaderboardsList {
		get {
			return leaderboardsList;
		}
	}
}