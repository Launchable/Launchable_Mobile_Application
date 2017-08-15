////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SA.Analytics.Google {

	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	public class GA_Settings : ScriptableObject {

		public static string VERSION_NUMBER = "4.7/21";
		
		[SerializeField]
		public List<Profile> accounts =  new List<Profile>();

		[SerializeField]
		public List<PlatfromBound> platfromBounds =  new List<PlatfromBound>();

		public bool showAdditionalParams = false;
		public bool showAdvancedParams = false;
		public bool showCParams = false;
		
		public bool showAccounts = true;
		public bool showPlatfroms = false;
		public bool showTestingMode = false;

		public string AppName = "My App";
		public string AppVersion = "0.0.1";

		public bool UseHTTPS = false;
		public bool StringEscaping = true;
		public bool EditorAnalytics = true;
		public bool IsDisabled = false;

		public bool IsTestingModeEnabled = false;
		public int TestingModeAccIndex = 0;

		public bool IsRequetsCachingEnabled= true;
		public bool IsQueueTimeEnabled = true;		

		public bool AutoLevelTracking = true;
		public string LevelPrefix = "Level_";
		public string LevelPostfix = "";

		public bool AutoAppQuitTracking = true;
		public bool AutoCampaignTracking  = false;
		public bool AutoExceptionTracking = true;
		public bool AutoAppResumeTracking = true;
		public bool SubmitSystemInfoOnFirstLaunch = true;

		public bool UsePlayerSettingsForAppInfo = true;

		public bool EnableFirebase = false;
		public string FirebaseAppId = string.Empty;

		private const string AnalyticsSettingsAssetName = "GA_Settings";
		private const string AnalyticsSettingsAssetExtension = ".asset";

		private static GA_Settings instance = null;

		public static GA_Settings Instance {

			get {

				if (instance == null) {
					instance = Resources.Load(AnalyticsSettingsAssetName) as GA_Settings;
					
					if (instance == null) {

						// If not found, autocreate the asset object.
						instance = CreateInstance<GA_Settings>();
						#if UNITY_EDITOR
						SA.Common.Util.Files.CreateFolder(SA.Common.Config.SETTINGS_PATH);
						
						string fullPath = Path.Combine(Path.Combine("Assets", SA.Common.Config.SETTINGS_PATH),
						                               AnalyticsSettingsAssetName + AnalyticsSettingsAssetExtension
						                               );
						
						AssetDatabase.CreateAsset(instance, fullPath);
						#endif
					}
				}
				return instance;
			}
		}


		public void UpdateVersionAndName() {

			#if UNITY_EDITOR
				AppName = PlayerSettings.productName;
				
				#if UNITY_ANDROID
				AppVersion = PlayerSettings.bundleVersion + "-" + PlayerSettings.Android.bundleVersionCode;
				#else
				AppVersion = PlayerSettings.bundleVersion;
				#endif

			#endif
		}

		public void AddProfile(Profile p) {
			accounts.Add(p);
		}

		public void RemoveProfile(Profile p) {
			accounts.Remove(p);
		}

		public void SetProfileIndexForPlatfrom(RuntimePlatform platfrom, int index, bool IsTesting) {
			foreach(PlatfromBound pb in platfromBounds) {
				if(pb.platfrom.Equals(platfrom)) {

					if(IsTesting) {
						pb.profileIndexTestMode = index;
					} else {
						pb.profileIndex = index;
					}

					return;
				}
			}

			PlatfromBound bound =  new PlatfromBound();
			bound.platfrom = platfrom;
			bound.profileIndex = 0;
			bound.profileIndexTestMode = 0;
			if(IsTesting) {
				bound.profileIndexTestMode = index;
			} else {
				bound.profileIndex = index;
			}

			platfromBounds.Add(bound);

		}

		public int GetProfileIndexForPlatfrom(RuntimePlatform platfrom, bool IsTestMode) {
			foreach(PlatfromBound pb in platfromBounds) {
				if(pb.platfrom.Equals(platfrom)) {
					int index =  pb.profileIndex;
					if(IsTestMode) {
						index = pb.profileIndexTestMode;
					} 

					if(index < accounts.Count) {
						return index;
					} else {
						return 0;
					}
				}
			}

			return 0;
		}
		
		public string[] GetProfileNames() {
			List<string> names =  new List<string>();
			foreach(Profile p in accounts) {
				names.Add(p.Name);
			}

			return names.ToArray();
		}

		public int GetProfileIndex(Profile p ) {
			int index = 0;
			string[] names = GetProfileNames();

			foreach(string name in names) {
				if(name.Equals(p.Name)) {
					return index;
				}

				index++;
			}

			return 0;

		}




		public Profile GetCurentProfile() {
			return GetPrfileForPlatfrom(Application.platform, IsTestingModeEnabled);
		}

		public Profile GetPrfileForPlatfrom(RuntimePlatform platfrom, bool IsTestMode) {

			if (accounts.Count == 0) {
				return new Profile();
			}



			return accounts[GetProfileIndexForPlatfrom(platfrom, IsTestMode)];

		}

	}
}
