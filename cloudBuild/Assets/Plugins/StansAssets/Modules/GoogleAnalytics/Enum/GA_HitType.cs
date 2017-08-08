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


namespace SA.Analytics.Google {

	public enum HitType  {
		PAGEVIEW,
		APPVIEW,
		EVENT,
		TRANSACTION,
		ITEM,
		SOCIAL,
		EXCEPTION,
		TIMING,
		SCREENVIEW
	}
}