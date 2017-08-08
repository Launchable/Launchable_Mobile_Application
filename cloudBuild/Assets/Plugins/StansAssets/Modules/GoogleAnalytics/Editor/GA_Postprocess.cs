////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;


namespace SA.Analytics.Google {

	public class Postprocess  {


		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
			if(GA_Settings.Instance.IsTestingModeEnabled) {
				Debug.LogWarning("WARNING: Google Analytics Test Mode Enabled!");
			}
		}
	}

}
#endif
