using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class UltimateMobileSettings : ScriptableObject {

	public const string VERSION_NUMBER = "10.1/21";
	
	private const string UMSettingsAssetName = "UltimateMobileSettings";
	private const string UMSettingsAssetExtension = ".asset";


	//--------------------------------------
	// Editor
	//--------------------------------------

	public int ToolbarSelectedIndex = 0;


	//--------------------------------------
	// In Apps
	//--------------------------------------


	[SerializeField]
	public List<UM_InAppProduct> InAppProducts =  new List<UM_InAppProduct>();

	public UM_TransactionsHandlingMode  TransactionsHandlingMode = UM_TransactionsHandlingMode.Automatic;

	//Inn Apps Editor Testing
	public int InApps_EditorFillRateIndex = 4;
	public int InApps_EditorFillRate = 100;
	public bool Is_InApps_EditorTestingEnabled = true;

	//Editor Only
	public bool IsInAppSettingsProductsOpen = true;


	//--------------------------------------
	// Game Services
	//--------------------------------------


	[SerializeField]
	public List<UM_Leaderboard> Leaderboards =  new List<UM_Leaderboard>();
	[SerializeField]
	public List<UM_Achievement> Achievements =  new List<UM_Achievement>();

	public bool AutoLoadUsersSmallImages = true;
	public bool AutoLoadUsersBigImages = false;
	public bool AutoLoadLeaderboardsInfo = true;
	public bool AutoLoadAchievementsInfo = true;

	//Editor Only
	public bool IsAchievementsOpen = true;
	public bool IsLeaderBoardsOpen = true;


	//--------------------------------------
	// Advertisement
	//--------------------------------------


	public UM_IOSAdEngineOprions IOSAdEdngine = UM_IOSAdEngineOprions.Disabled;
	public UM_WP8AdEngineOprions WP8AdEdngine = UM_WP8AdEngineOprions.GoogleMobileAd;


	//--------------------------------------
	// Camera And Gallary
	//--------------------------------------

	public bool IsCameraAndGallerySettingsOpen = true;
	public bool IsCameraAndGalleryAndroidSettingsOpen = false;
	public bool IsCameraAndGalleryIOSSettingsOpen = false;


	//--------------------------------------
	// Local And Push Notifications
	//--------------------------------------

	public bool IsLP_Android_SettingsOpen = false;
	public bool IsLP_IOS_SettingsOpen = false;


	//--------------------------------------
	// Third Party Settings
	//--------------------------------------

	public bool TP_Android_SettingsOpen = false;
	public bool TP_IOS_SettingsOpen = false;



	//--------------------------------------
	// Ultimate Mobile Settings
	//--------------------------------------

	public UM_PlatformDependencies PlatformEngine = UM_PlatformDependencies.Android;



	//--------------------------------------
	// Get / Set
	//--------------------------------------
	
	private static UltimateMobileSettings instance = null;
	public static UltimateMobileSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(UMSettingsAssetName) as UltimateMobileSettings;
				
				if (instance == null) {


					
					// If not found, autocreate the asset object.
					instance = CreateInstance<UltimateMobileSettings>();
					#if UNITY_EDITOR

					SA.Common.Util.Files.CreateFolder(SA.Common.Config.SETTINGS_PATH);
					
					string fullPath = Path.Combine(Path.Combine("Assets", SA.Common.Config.SETTINGS_PATH),
					                               UMSettingsAssetName + UMSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}

	//--------------------------------------
	// IN Apps
	//--------------------------------------

	public void AddProduct(UM_InAppProduct p) {
		InAppProducts.Add(p);
	}

	public void RemoveProduct(UM_InAppProduct p) {
		InAppProducts.Remove(p);
	}


	public UM_InAppProduct GetProductById(string id) {
		foreach(UM_InAppProduct p in InAppProducts) {
			if(p.id.Equals(id)) {
				return p;
			}
		}

		return null;
	}


	public UM_InAppProduct GetProductByIOSId(string id) {
		foreach(UM_InAppProduct p in InAppProducts) {
			if(p.IOSId.Equals(id)) {
				return p;
			}
		}
		
		return null;
	}


	public UM_InAppProduct GetProductByAndroidId(string id) {
		foreach(UM_InAppProduct p in InAppProducts) {
			if(p.AndroidId.Equals(id)) {
				return p;
			}
		}
		
		return null;
	}

	public UM_InAppProduct GetProductByAmazonId(string id) {
		foreach(UM_InAppProduct p in InAppProducts) {
			if(p.AmazonId.Equals(id)) {
				return p;
			}
		}

		return null;
	}

	public UM_InAppProduct GetProductByWp8Id(string id) {
		foreach(UM_InAppProduct p in InAppProducts) {
			if(p.WP8Id.Equals(id)) {
				return p;
			}
		}
		
		return null;
	}


	//--------------------------------------
	// Achievements
	//--------------------------------------


	public void AddAchievement(UM_Achievement a) {
		Achievements.Add(a);
	}
	
	public void RemoveAchievement(UM_Achievement a) {
		Achievements.Remove(a);
	}


	public UM_Achievement GetAchievementById(string id) {
		foreach(UM_Achievement a in Achievements) {
			if(a.id.Equals(id)) {
				return a;
			}
		}
		
		return null;
	}
	
	
	public UM_Achievement GetAchievementByIOSId(string id) {
		foreach(UM_Achievement a in Achievements) {
			if(a.IOSId.Equals(id)) {
				return a;
			}
		}
		
		return null;
	}
	
	
	public UM_Achievement GetAchievementByAndroidId(string id) {
		foreach(UM_Achievement a in Achievements) {
			if(a.AndroidId.Equals(id)) {
				return a;
			}
		}
		
		return null;
	}


	
	//--------------------------------------
	// Leaderboards
	//--------------------------------------


	
	public void AddLeaderboard(UM_Leaderboard l) {
		Leaderboards.Add(l);
	}
	
	public void RemoveLeaderboard(UM_Leaderboard l) {
		Leaderboards.Remove(l);
	}
	
	
	public UM_Leaderboard GetLeaderboardById(string id) {
		foreach(UM_Leaderboard l in Leaderboards) {
			if(l.id.Equals(id)) {
				return l;
			}
		}
		
		return null;
	}
	
	
	public UM_Leaderboard GetLeaderboardByIOSId(string id) {
		foreach(UM_Leaderboard l in Leaderboards) {
			if(l.IOSId.Equals(id)) {
				return l;
			}
		}
		return null;
	}
	
	
	public UM_Leaderboard GetLeaderboardByAndroidId(string id) {
		foreach(UM_Leaderboard l in Leaderboards) {
			if(l.AndroidId.Equals(id)) {
				return l;
			}
		}
		
		return null;
	}

	public UM_Leaderboard GetLeaderboardByAmazonId(string id) {
		foreach(UM_Leaderboard l in Leaderboards) {
			if(l.AmazonId.Equals(id)) {
				return l;
			}
		}

		return null;
	}

}
