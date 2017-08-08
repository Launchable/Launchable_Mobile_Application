////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SA.Analytics.Google {

	[System.Serializable]
	public class Profile  {
		public string Name = "[Account Name]";
		public string TrackingID = "UA-XXXXXXX-Y";
		public bool IsOpen = false;

	}
}