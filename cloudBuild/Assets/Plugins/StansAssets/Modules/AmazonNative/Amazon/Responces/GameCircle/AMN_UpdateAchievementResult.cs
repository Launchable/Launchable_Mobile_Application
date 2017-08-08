////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class AMN_UpdateAchievementResult : AMN_Result {
	
	private string error;
	private string achievementID;
	
	public AMN_UpdateAchievementResult(bool success):base(success) {
		
	}
	
	public AMN_UpdateAchievementResult(string id, string err):base(false) {
		achievementID = id;
		error = err;
	}
	
	public AMN_UpdateAchievementResult(string id):base(true) {
		achievementID = id;
	}
	
	public string Error {
		get {
			return error;
		}
	}

	public string AchievementID {
		get {
			return achievementID;
		}
	}
}