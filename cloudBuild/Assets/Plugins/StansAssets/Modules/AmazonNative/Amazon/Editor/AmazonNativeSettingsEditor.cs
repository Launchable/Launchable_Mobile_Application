#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AmazonNativeSettings))]
public class AmazonNativeSettingsEditor : Editor {

	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");

	private AmazonNativeSettings settings;

	void Awake() {
		UpdatePluginSettings();
	}

	private Texture[] _ToolbarImages = null;

	public Texture[] ToolbarImages {
		get {
			if(_ToolbarImages == null) {
				Texture2D billing =  Resources.Load("billing") as Texture2D;
				Texture2D gamecircles =  Resources.Load("gamecircles") as Texture2D;
				Texture2D ads =  Resources.Load("ads") as Texture2D;				
				Texture2D amazon =  Resources.Load("amazon") as Texture2D;
				Texture2D editorTesting = Resources.Load("editorTesting") as Texture2D;

				List<Texture2D> textures =  new List<Texture2D>();
				textures.Add(amazon);
				textures.Add(gamecircles);
				textures.Add(billing);
				textures.Add(ads);
				textures.Add(editorTesting);

				_ToolbarImages = textures.ToArray();

			}
			return _ToolbarImages;
		}
	}

	private int _Width = 500;
	public int Width {
		get {
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			Rect scale = GUILayoutUtility.GetLastRect();

			if(scale.width != 1) {
				_Width = System.Convert.ToInt32(scale.width);
			}

			return _Width;
		}
	}

	public override void OnInspectorGUI() {
		settings = target as AmazonNativeSettings;

		GUI.changed = false;


		GUI.SetNextControlName ("toolbar");
		GUILayoutOption[] toolbarSize = new GUILayoutOption[]{GUILayout.Width(Width-5), GUILayout.Height(30)};
		settings.ToolbarIndex = GUILayout.Toolbar (AmazonNativeSettings.Instance.ToolbarIndex, ToolbarImages, toolbarSize);// ToolbarHeaders, new GUILayoutOption[]{ GUILayout.Height(25f)});

		switch(AmazonNativeSettings.Instance.ToolbarIndex) {
		case 0: 
			GeneralOptions();	
			EditorGUILayout.Space();
			AboutGUI();	
			break;
		case 1:
			GameCircleSettings();			 
			break;
		case 2:
			BillingSettings();
			break;
		case 3:
			AdvertisingSettings();
			break;
		}



		if(GUI.changed) {
			DirtyEditor();
		}
	}

	
	private void GeneralOptions() {
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("General Settings", MessageType.None);
		
		
		AmazonNativeSettings.Instance.ShowActions = EditorGUILayout.Foldout(AmazonNativeSettings.Instance.ShowActions, "Actions");
		if(AmazonNativeSettings.Instance.ShowActions) {
			
			EditorGUILayout.BeginHorizontal();
			
			
			
			if(GUILayout.Button("Amazon Developer Console",  GUILayout.Width(200))) {
				Application.OpenURL(AmazonNativeSettings.Instance.AmazonDeveloperConsoleLink);
			}
			
			if(GUILayout.Button("Remove Amazon Libraries",  GUILayout.Width(200))) {
				SA.Common.Util.Files.DeleteFolder("Plugins/Amazon");
				SA.Common.Util.Files.DeleteFolder("Plugins/AmazonCommon");
				SA.Common.Util.Files.DeleteFolder("Plugins/AmazonGameCirclePlugin");
				SA.Common.Util.Files.DeleteFolder("Plugins/Android/Advertising_Res");
				SA.Common.Util.Files.DeleteFolder("Plugins/Android/assets");
				SA.Common.Util.Files.DeleteFolder("Plugins/Android/Billing_Res");
				SA.Common.Util.Files.DeleteFolder("Plugins/Android/GameCircle_Res");

				SA.Common.Util.Files.DeleteFile("Plugins/AmazonGameCircleLicense.txt");
				SA.Common.Util.Files.DeleteFile("Plugins/AmazonGameCircleNotice.txt");
				SA.Common.Util.Files.DeleteFile("Plugins/AmazonGameCirclePluginVersion.txt");

				SA.Common.Util.Files.DeleteFile("Plugins/Android/libs/armeabi/libAmazonIapV2Bridge.so");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/libs/armeabi/libAmazonMobileAdsBridge.so");

				SA.Common.Util.Files.DeleteFile("Plugins/Android/libs/armeabi-v7a/libAmazonIapV2Bridge.so");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/libs/armeabi-v7a/libAmazonMobileAdsBridge.so");

				SA.Common.Util.Files.DeleteFile("Plugins/Android/libs/x86/libAmazonIapV2Bridge.so");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/libs/x86/libAmazonMobileAdsBridge.so");

				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonCptPluginsUtils-1.0.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonIapV2Client.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonIapV2JavaService-1.0.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonInsights-android-sdk.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonMobileAdsAndroidSDK3P-1.1.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonMobileAdsClient.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/AmazonMobileAdsJavaService-1.0.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/amazonnative.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/gamecirclesdk.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/gcunity.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/gson-2.2.4.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/in-app-purchasing-2.0.61.jar");
				SA.Common.Util.Files.DeleteFile("Plugins/Android/login-with-amazon-sdk.jar");



				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAd.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAd.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAd.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAdPair.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAdPair.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAdPosition.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAdPosition.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAdShown.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAdShown.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAds.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsBridge.mm");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsBridge.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsImpl.h");

				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsImpl.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsImplStub.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsImplStub.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsJsonFromObject.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsJsonFromObject.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsObjectFromJson.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsObjectFromJson.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsObjectiveCController.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsObjectiveCControllerImpl.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONAmazonMobileAdsObjectiveCControllerImpl.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONApplicationKey.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONApplicationKey.h");

				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONEnums.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONEnums.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AmazonInsightsSDK.a");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsEqual.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsEqual.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsReady.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsReady.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONLoadingStarted.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONLoadingStarted.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONPlacement.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONPlacement.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONShouldEnable.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONShouldEnable.m");

				SA.Common.Util.Files.DeleteFile("Plugins/IOS/GameCircle.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/GameCircle.a");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/libGameCircleUnityProxy.a");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsEqual.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsEqual.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsReady.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONIsReady.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONLoadingStarted.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONLoadingStarted.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONPlacement.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONPlacement.m");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONShouldEnable.h");
				SA.Common.Util.Files.DeleteFile("Plugins/IOS/AMAZONShouldEnable.m");

				SA.Common.Util.Files.DeleteFolder("Plugins/IOS/GameCircle.bundle");

				AssetDatabase.Refresh();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Load Example Settings",  GUILayout.Width(200))) {
				LoadExampleSettings();
			}
			
			EditorGUILayout.EndHorizontal();			
		}
	}

	public static void LoadExampleSettings()  {
		AmazonNativeSettings.Instance.InAppProducts =  new List<AmazonProductTemplate>();	


		AmazonNativeSettings.Instance.InAppProducts.Add(new AmazonProductTemplate(){ Sku = "first_Item", Title = "ItemTitle", IsOpen = false});		
		
		AmazonNativeSettings.Instance.AppAPIKey = "f06565f7696840d7adce3d08ea18d742";
		AmazonNativeSettings.Instance.AdvertisingBannerAlign = AMN_BannerAlign.Bottom;
		
		SA.Common.Editor.Tools.ApplicationIdentifier = "com.unionassets.android.plugin.preview";
	}

	private static string SA_AmazonGameCircleManager_Path = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Manage/SA_AmazonGameCircleManager.cs";
	private static string GC_Achievement_Path			  = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Models/GC_Achievement.cs";
	private static string GC_Leaderboard_Path             = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Models/GC_Leaderboard.cs";
	private static string GC_Player_Path				  = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Models/GC_Player.cs";
	private static string GC_ScoreTimeSpan_Path           = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Enums/GC_ScoreTimeSpan.cs";

	private static string SA_AmazonAdsManager_Path = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Manage/SA_AmazonAdsManager.cs";

	private static string SA_AmazonBillingManager_Path 					 = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Manage/SA_AmazonBillingManager.cs";
	private static string Billing_GetUserDataResponse_Path 				 = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Responces/Billing/AMN_GetUserDataResponse.cs";
	private static string Billing_PurchaseResponse_Path 				 = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Responces/Billing/AMN_PurchaseResponse.cs";
	private static string Billing_GetProductDataResponse_Path 			 = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Responces/Billing/AMN_GetProductDataResponse.cs";
	private static string Billing_GetPurchaseProductsUpdateResponse_Path = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Responces/Billing/AMN_GetPurchaseProductsUpdateResponse.cs";
	private static string Billing_AmazonProductTemplate_Path 		 	 = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Models/AmazonProductTemplate.cs";
	private static string Billing_AmazonReceipt_Path 		 	 		 = SA.Common.Config.MODULS_PATH + "AmazonNative/Amazon/Models/SA_AmazonReceipt.cs";

	public static void UpdatePluginSettings() {
		if(AmazonNativeSettings.Instance.IsGameCircleEnabled) {
			if(!SA.Common.Util.Files.IsFolderExists("Plugins/Android/GameCircle_Res")) {
				
				bool res = EditorUtility.DisplayDialog("Game Circle Native Library Not Found!", "Amazon Native wasn't able to locate Game Circle Native Library in your project. Would you like to donwload and install it?", "Download", "No Thanks");
				if(res) {
					Application.OpenURL(AmazonNativeSettings.Instance.GameCircleDownloadLink);
				}
				
				AmazonNativeSettings.Instance.IsGameCircleEnabled = false;
			} 
			Update_GC_State();
		}

		if(AmazonNativeSettings.Instance.IsAdvertisingEnabled) {
			if(!SA.Common.Util.Files.IsFolderExists("Plugins/Android/Advertising_Res")) {
				
				bool res = EditorUtility.DisplayDialog("Advertising Native Library Not Found!", "Amazon Native wasn't able to locate Advertising Native Library in your project. Would you like to donwload and install it?", "Download", "No Thanks");
				if(res) {
					Application.OpenURL(AmazonNativeSettings.Instance.AdvertisingDownloadLink);
				}
				
				AmazonNativeSettings.Instance.IsAdvertisingEnabled = false;
			} 
			Update_Advertising_State();
		}

		if(AmazonNativeSettings.Instance.IsBillingEnabled) {
			if(!SA.Common.Util.Files.IsFolderExists("Plugins/Android/Billing_Res")) {
				
				bool res = EditorUtility.DisplayDialog("Billing Native Libraries Not Found!", "Amazon Native wasn't able to locate Billing Native Libraries in your project. Would you like to donwload and install it?", "Download", "No Thanks");
				if(res) {
					Application.OpenURL(AmazonNativeSettings.Instance.BillingDownloadLink);
				}
				
				AmazonNativeSettings.Instance.IsBillingEnabled = false;
			} 
			Update_Billing_State();
		}
	}

	private static void Update_GC_State() {
		SA.Common.Util.Files.DeleteFile("Plugins/Android/AndroidManifest 1.xml");
		SA.Common.Util.Files.DeleteFile("Plugins/Android/AndroidManifest 2.xml");

		SA.Common.Editor.Tools.ChnageDefineState(SA_AmazonGameCircleManager_Path, "AMAZON_CIRCLE_ENABLED", AmazonNativeSettings.Instance.IsGameCircleEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(GC_Achievement_Path, "AMAZON_CIRCLE_ENABLED", AmazonNativeSettings.Instance.IsGameCircleEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(GC_Leaderboard_Path, "AMAZON_CIRCLE_ENABLED", AmazonNativeSettings.Instance.IsGameCircleEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(GC_Player_Path, "AMAZON_CIRCLE_ENABLED", AmazonNativeSettings.Instance.IsGameCircleEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(GC_ScoreTimeSpan_Path, "AMAZON_CIRCLE_ENABLED", AmazonNativeSettings.Instance.IsGameCircleEnabled);
	}

	private static void Update_Advertising_State() {		
		SA.Common.Util.Files.DeleteFile("Plugins/Android/AndroidManifest 1.xml");
		SA.Common.Util.Files.DeleteFile("Plugins/Android/AndroidManifest 2.xml");

		SA.Common.Editor.Tools.ChnageDefineState(SA_AmazonAdsManager_Path, "AMAZON_ADVERTISING_ENABLED", AmazonNativeSettings.Instance.IsAdvertisingEnabled);
	}
	
	private static void Update_Billing_State() {
		SA.Common.Util.Files.DeleteFile("Plugins/Android/AndroidManifest 1.xml");
		SA.Common.Util.Files.DeleteFile("Plugins/Android/AndroidManifest 2.xml");
		
		SA.Common.Editor.Tools.ChnageDefineState(SA_AmazonBillingManager_Path, 					"AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(Billing_GetUserDataResponse_Path, 				"AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(Billing_PurchaseResponse_Path, 					"AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(Billing_GetProductDataResponse_Path, 			"AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(Billing_GetPurchaseProductsUpdateResponse_Path, "AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(Billing_AmazonProductTemplate_Path, 			"AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
		SA.Common.Editor.Tools.ChnageDefineState(Billing_AmazonReceipt_Path, 					"AMAZON_BILLING_ENABLED", AmazonNativeSettings.Instance.IsBillingEnabled);
	}

	GUIContent L_IdDLabel 			= new GUIContent("Leaderboard ID[?]:", "A unique alphanumeric identifier that you create for this leaderboard. Can also contain periods and underscores.");
	GUIContent L_DisplayNameLabel  	= new GUIContent("Display Name[?]:", "The display name of the leaderboard.");
	GUIContent L_DescriptionLabel  	= new GUIContent("Description[?]:", "The description of your leaderboard.");
	
	GUIContent A_IdDLabel 			= new GUIContent("Achievement ID[?]:", "A unique alphanumeric identifier that you create for this achievement. Can also contain periods and underscores.");
	GUIContent A_DisplayNameLabel  	= new GUIContent("Display Name[?]:", "The display name of the achievement.");
	GUIContent A_DescriptionLabel 	= new GUIContent("Description[?]:", "The description of your achievement.");

	void GameCircleSettings () {

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Game Circle Configuration", MessageType.None);



		EditorGUI.BeginChangeCheck();
		AmazonNativeSettings.Instance.IsGameCircleEnabled = SA.Common.Editor.Tools.ToggleFiled("Game Circle API", AmazonNativeSettings.Instance.IsGameCircleEnabled);
		if(EditorGUI.EndChangeCheck())  {
			UpdatePluginSettings();
		}
	
		GUI.enabled = AmazonNativeSettings.Instance.IsGameCircleEnabled;
		
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Leaderboards Info", MessageType.None);
		
		
		EditorGUI.indentLevel++; {
			
			EditorGUILayout.BeginVertical (GUI.skin.box);
			
			EditorGUILayout.BeginHorizontal();
			AmazonNativeSettings.Instance.ShowLeaderboards = EditorGUILayout.Foldout(AmazonNativeSettings.Instance.ShowLeaderboards, "Leaderboards");
			
			
			
			EditorGUILayout.EndHorizontal();
			
			
			if(AmazonNativeSettings.Instance.ShowLeaderboards) {
				EditorGUILayout.Space();
				
				foreach(GC_Leaderboard leaderboard in AmazonNativeSettings.Instance.Leaderboards) {
					
					
					EditorGUILayout.BeginVertical (GUI.skin.box);
					
					EditorGUILayout.BeginHorizontal();
					
					
					
					if(leaderboard.Texture != null) {
						GUILayout.Box(leaderboard.Texture, ImageBoxStyle, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)});
					}
					
					leaderboard.IsOpen 	= EditorGUILayout.Foldout(leaderboard.IsOpen, leaderboard.Title);
					
					
					
					bool ItemWasRemoved = SA.Common.Editor.Tools.SrotingButtons((object) leaderboard, AmazonNativeSettings.Instance.Leaderboards);
					if(ItemWasRemoved) {
						return;
					}
					
					
					EditorGUILayout.EndHorizontal();
					
					if(leaderboard.IsOpen) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(L_IdDLabel);
						leaderboard.Identifier	 	= EditorGUILayout.TextField(leaderboard.Identifier);
						if(leaderboard.Identifier.Length > 0) {
							leaderboard.Identifier 		= leaderboard.Identifier.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(L_DisplayNameLabel);
						leaderboard.Title	 	= EditorGUILayout.TextField(leaderboard.Title);
						EditorGUILayout.EndHorizontal();
						
						
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(L_DescriptionLabel);
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						leaderboard.Description	 = EditorGUILayout.TextArea(leaderboard.Description,  new GUILayoutOption[]{GUILayout.Height(60), GUILayout.Width(200)} );
						leaderboard.Texture = (Texture2D) EditorGUILayout.ObjectField("", leaderboard.Texture, typeof (Texture2D), false);
						EditorGUILayout.EndHorizontal();
						
					}
					
					EditorGUILayout.EndVertical();
					
				}
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add new", EditorStyles.miniButton, GUILayout.Width(250))) {
					GC_Leaderboard lb =  new GC_Leaderboard();
					AmazonNativeSettings.Instance.Leaderboards.Add(lb);
				}
				
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}
			
			EditorGUILayout.EndVertical();
			
		}EditorGUI.indentLevel--;
		
		
		
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Achievements Info", MessageType.None);
		
		EditorGUI.indentLevel++; {
			
			EditorGUILayout.BeginVertical (GUI.skin.box);
			
			EditorGUILayout.BeginHorizontal();
			AmazonNativeSettings.Instance.ShowAchievementsParams = EditorGUILayout.Foldout(AmazonNativeSettings.Instance.ShowAchievementsParams, "Achievements");
			
			
			
			EditorGUILayout.EndHorizontal();
			
			
			if(AmazonNativeSettings.Instance.ShowAchievementsParams) {
				EditorGUILayout.Space();
				
				foreach(GC_Achievement achievement in AmazonNativeSettings.Instance.Achievements) {
					
					
					EditorGUILayout.BeginVertical (GUI.skin.box);
					
					EditorGUILayout.BeginHorizontal();
					
					
					
					if(achievement.Texture != null) {
						GUILayout.Box(achievement.Texture, ImageBoxStyle, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)});
					}
					
					achievement.IsOpen 	= EditorGUILayout.Foldout(achievement.IsOpen, achievement.Title);
					
					
					
					bool ItemWasRemoved = SA.Common.Editor.Tools.SrotingButtons((object) achievement, AmazonNativeSettings.Instance.Achievements);
					if(ItemWasRemoved) {
						return;
					}
					
					
					EditorGUILayout.EndHorizontal();
					
					if(achievement.IsOpen) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(A_IdDLabel);
						achievement.Identifier	 	= EditorGUILayout.TextField(achievement.Identifier);
						if(achievement.Identifier.Length > 0) {
							achievement.Identifier 		= achievement.Identifier.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(A_DisplayNameLabel);
						achievement.Title	 	= EditorGUILayout.TextField(achievement.Title);
						EditorGUILayout.EndHorizontal();
						
						
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(A_DescriptionLabel);
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						achievement.Description	 = EditorGUILayout.TextArea(achievement.Description,  new GUILayoutOption[]{GUILayout.Height(60), GUILayout.Width(200)} );
						achievement.Texture = (Texture2D) EditorGUILayout.ObjectField("", achievement.Texture, typeof (Texture2D), false);
						EditorGUILayout.EndHorizontal();
						
					}
					
					EditorGUILayout.EndVertical();
					
				}
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add new", EditorStyles.miniButton, GUILayout.Width(250))) {
					GC_Achievement achievement =  new GC_Achievement();
					AmazonNativeSettings.Instance.Achievements.Add(achievement);
				}
				
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}
			
			EditorGUILayout.EndVertical();
			
			
			
			
		}EditorGUI.indentLevel--;


		GUI.enabled  = true;
	}

	GUIContent ProductIdDLabel 		= new GUIContent("ProductId[?]:", "A unique identifier that will be used for reporting. It can be composed of letters and numbers.");
	GUIContent IsConsLabel 			= new GUIContent("Is Consumable[?]:", "Is prodcut allowed to be purchased more than once?");
	GUIContent DisplayNameLabel  	= new GUIContent("Display Name[?]:", "This is the name of the In-App Purchase that will be seen by customers (if this is their primary language). For automatically renewable subscriptions, don’t include a duration in the display name. The display name can’t be longer than 75 characters.");
	GUIContent DescriptionLabel 	= new GUIContent("Description[?]:", "This is the description of the In-App Purchase that will be used by App Review during the review process. If indicated in your code, this description may also be seen by customers. For automatically renewable subscriptions, do not include a duration in the description. The description cannot be longer than 255 bytes.");

	private void BillingSettings() {
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Billing Settings", MessageType.None);

		EditorGUI.BeginChangeCheck();
		AmazonNativeSettings.Instance.IsBillingEnabled = SA.Common.Editor.Tools.ToggleFiled("Billing API", AmazonNativeSettings.Instance.IsBillingEnabled);
		if(EditorGUI.EndChangeCheck())  {
			UpdatePluginSettings();
		}

		GUI.enabled = AmazonNativeSettings.Instance.IsBillingEnabled;
	
		EditorGUI.indentLevel++;
		{
			EditorGUILayout.BeginVertical (GUI.skin.box);			
			
			EditorGUILayout.BeginHorizontal();
			AmazonNativeSettings.Instance.ShowStoreParams = EditorGUILayout.Foldout(AmazonNativeSettings.Instance.ShowStoreParams, "Products");			
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			
			if(AmazonNativeSettings.Instance.ShowStoreParams) {

				if(settings.InAppProducts.Count == 0) {
					EditorGUILayout.HelpBox("No In-App Products Added", MessageType.Warning);
				}

				foreach(AmazonProductTemplate product in AmazonNativeSettings.Instance.InAppProducts) {
					
					EditorGUILayout.BeginVertical (GUI.skin.box);
					
					EditorGUILayout.BeginHorizontal();
					
					GUIStyle s =  new GUIStyle();
					s.padding =  new RectOffset();
					s.margin =  new RectOffset();
					s.border =  new RectOffset();
					
					if(product.Texture != null) {
						GUILayout.Box(product.Texture, s, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)});
					}
					
					product.IsOpen 	= EditorGUILayout.Foldout(product.IsOpen, product.Title);
					
					
					EditorGUILayout.LabelField(product.Price + "$");
					bool ItemWasRemoved = DrawSortingButtons((object) product, AmazonNativeSettings.Instance.InAppProducts);
					if(ItemWasRemoved) {
						return;
					}
					
					EditorGUILayout.EndHorizontal();
					
					if(product.IsOpen) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(ProductIdDLabel);
						product.Sku	 	= EditorGUILayout.TextField(product.Sku);
						if(product.Sku.Length > 0) {
							product.Sku = product.Sku.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(DisplayNameLabel);
						product.Title	 	= EditorGUILayout.TextField(product.Title);
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(IsConsLabel);
						//check what type is coming
						product.ProductType	= (AMN_InAppType) EditorGUILayout.EnumPopup(product.ProductType);
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(DescriptionLabel);
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						product.Description	 = EditorGUILayout.TextArea(product.Description,  new GUILayoutOption[]{GUILayout.Height(60), GUILayout.Width(200)} );
						product.Texture = (Texture2D) EditorGUILayout.ObjectField("", product.Texture, typeof (Texture2D), false);
						EditorGUILayout.EndHorizontal();
					}
					
					
					EditorGUILayout.EndVertical();
					
				}
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add new", EditorStyles.miniButton, GUILayout.Width(250))) {
					AmazonProductTemplate product = new AmazonProductTemplate();
					AmazonNativeSettings.Instance.InAppProducts.Add(product);
				}
				
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}
			
			EditorGUILayout.EndVertical();
		}
		EditorGUI.indentLevel--;
		
		GUI.enabled  = true;
	}	

	GUIContent APIKeyLabel = new GUIContent("APIKey[?]:", "A unique identifier that will be used for reporting. It can be composed of letters and numbers.");
	GUIContent IsTestingModeLabel = new GUIContent("Is Test Mode[?]:", "Is Test Mode activated?");
	GUIContent BannerAlignLabel = new GUIContent("Banner align[?]:", "Select preffered banner align");

	void AdvertisingSettings () {		
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Advertising Settings", MessageType.None);

		EditorGUI.BeginChangeCheck();
		AmazonNativeSettings.Instance.IsAdvertisingEnabled = SA.Common.Editor.Tools.ToggleFiled("Advertising API", AmazonNativeSettings.Instance.IsAdvertisingEnabled);
		if(EditorGUI.EndChangeCheck())  {
			UpdatePluginSettings();
		}

		GUI.enabled = AmazonNativeSettings.Instance.IsAdvertisingEnabled;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(APIKeyLabel);
		AmazonNativeSettings.Instance.AppAPIKey = EditorGUILayout.TextField(AmazonNativeSettings.Instance.AppAPIKey);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(IsTestingModeLabel);
		AmazonNativeSettings.Instance.IsTestMode = EditorGUILayout.Toggle(AmazonNativeSettings.Instance.IsTestMode);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(BannerAlignLabel);
		AmazonNativeSettings.Instance.AdvertisingBannerAlign = (AMN_BannerAlign) EditorGUILayout.EnumPopup(AmazonNativeSettings.Instance.AdvertisingBannerAlign);
		EditorGUILayout.EndHorizontal();

		GUI.enabled  = true;
	}

	private bool DrawSortingButtons(object currentObject, IList ObjectsList) {
		
		int ObjectIndex = ObjectsList.IndexOf(currentObject);
		if(ObjectIndex == 0) {
			GUI.enabled = false;
		} 
		
		bool up = GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(20));
		if(up) {
			object c = currentObject;
			ObjectsList[ObjectIndex]  		= ObjectsList[ObjectIndex - 1];
			ObjectsList[ObjectIndex - 1] 	=  c;
		}
		
		
		if(ObjectIndex >= ObjectsList.Count -1) {
			GUI.enabled = false;
		} else {
			GUI.enabled = true;
		}
		
		bool down 		= GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(20));
		if(down) {
			object c = currentObject;
			ObjectsList[ObjectIndex] =  ObjectsList[ObjectIndex + 1];
			ObjectsList[ObjectIndex + 1] = c;
		}
		
		
		GUI.enabled = true;
		bool r 			= GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20));
		if(r) {
			ObjectsList.Remove(currentObject);
		}
		
		return r;
	}

	private void AboutGUI() {

		EditorGUILayout.HelpBox("Version Info", MessageType.None);
		EditorGUILayout.Space();
		
		SelectableLabelField(SdkVersion, AmazonNativeSettings.VERSION_NUMBER);
		SA.Common.Editor.Tools.SupportMail();
		SA.Common.Editor.Tools.DrawSALogo();
	}
	
	private void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}

	private  GUIStyle _ImageBoxStyle = null;
	public GUIStyle ImageBoxStyle {
		get {
			if(_ImageBoxStyle == null) {
				_ImageBoxStyle =  new GUIStyle();
				_ImageBoxStyle.padding =  new RectOffset();
				_ImageBoxStyle.margin =  new RectOffset();
				_ImageBoxStyle.border =  new RectOffset();
			}
			
			return _ImageBoxStyle;
		}
	}

	public static void UpdateVersionInfo() {
		SA.Common.Util.Files.Write(SA.Common.Config.AMN_VERSION_INFO_PATH, AmazonNativeSettings.VERSION_NUMBER);
	}

	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(AmazonNativeSettings.Instance);
		#endif
	}
}
#endif
