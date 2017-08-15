using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class AmazonNativeSettings : ScriptableObject {

	public const string VERSION_NUMBER = "2.7/21";

	public int ToolbarIndex = 0;
	public bool ShowActions = true;
	public bool ShowStoreParams = false;

	public bool IsGameCircleEnabled = false;
	public bool IsBillingEnabled = false;
	public bool IsAdvertisingEnabled = false;
	//--------------------------------------
	// IN APPS
	//--------------------------------------

	public List<AmazonProductTemplate> InAppProducts = new List<AmazonProductTemplate>();

	//--------------------------------------
	// ADVERTISING
	//--------------------------------------

	public string AppAPIKey = string.Empty;
	public bool IsTestMode = true;
	public AMN_BannerAlign AdvertisingBannerAlign = AMN_BannerAlign.Bottom;

	//--------------------------------------
	// GAME CIRCLE
	//--------------------------------------

	public bool ShowLeaderboards = true;
	public List<GC_Leaderboard> Leaderboards = new  List<GC_Leaderboard>();

	public bool ShowAchievementsParams = true;
	public List<GC_Achievement> Achievements = new  List<GC_Achievement>();

	//--------------------------------------
	// LINKS
	//--------------------------------------

	public string AmazonDeveloperConsoleLink = "https://goo.gl/EKAKSJ";

	public string GameCircleDownloadLink = "https://db.tt/71Rgmuqw";
	public string BillingDownloadLink = "https://db.tt/vBh98Yvt";
	public string AdvertisingDownloadLink = "https://db.tt/AkvhCMTk";

	//--------------------------------------
	// PATHS
	//--------------------------------------

	private const string AMNSettingsAssetName = "AmazonNativeSettings";
	private const string AMNSettingsPath = SA.Common.Config.SETTINGS_PATH;
	private const string AMNSettingsAssetExtension = ".asset";

	private static AmazonNativeSettings instance = null;


	public static AmazonNativeSettings Instance {

		get {
			if (instance == null) {
				instance = Resources.Load(AMNSettingsAssetName) as AmazonNativeSettings;

				if (instance == null) {

					// If not found, autocreate the asset object.
					instance = CreateInstance<AmazonNativeSettings>();
					#if UNITY_EDITOR

					SA.Common.Util.Files.CreateFolder(AMNSettingsPath);


					string fullPath = Path.Combine(Path.Combine("Assets", AMNSettingsPath),
					                               AMNSettingsAssetName + AMNSettingsAssetExtension
					                               );

					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}


}
