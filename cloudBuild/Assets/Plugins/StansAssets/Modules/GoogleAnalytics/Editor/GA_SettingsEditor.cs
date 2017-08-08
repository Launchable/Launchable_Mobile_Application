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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace SA.Analytics.Google {

	[CustomEditor(typeof(GA_Settings))]
	public class SettingsEditor : UnityEditor.Editor {
		
		
		
		private static GUIContent acountNameLabel = new GUIContent("Account Name [?]:", "Name of Google Analytics Account");
		private static GUIContent appNameLabel = new GUIContent("App Name [?]:", "For your own use and organization.");
		
		private static GUIContent testingModeLabel = new GUIContent("Testing [?]:", "This account will be used if testing mode enabled");
		private static GUIContent prodModeLabel = new GUIContent("Production [?]:", "This account will be used if testing mode disabled.");
		
		private static GUIContent appVersionLabel = new GUIContent("App Version [?]:", "Your application build version.");
		private static GUIContent appTrackingIdLabel = new GUIContent("Tracking Id [?]:", "The ID that distinguishes to which Google Analytics property to send data.");
		
		private static GUIContent newLevelTrackingLabel = new GUIContent("Levels [?]:", "Screen Hit will be sent automaticaly when new level is loaded");
		private static GUIContent exTrackingLabel = new GUIContent("Exceptions [?]:", "Application exceptions reports will be sent automatically ");
		private static GUIContent systemInfoTrackingLabel = new GUIContent("SystemInfo [?]:", "System info will be automatically submitted on first launch ");
		private static GUIContent quitTrackingLabel = new GUIContent("App Quit [?]:", "Automatically track app quit.");
		private static GUIContent pauseTrackingLabel = new GUIContent("App [?]:", "Automatically track when app goes background and start / end session on this event ");
		private static GUIContent CampaignTrackingLabel = new GUIContent("Campaigns [?]:", "Auto Campaign Tracking. Avaliable on Android Only");
		
		
		private static GUIContent levelPostfix = new GUIContent("Level Postfix [?]:", "Postfix for loaded scene name ");
		private static GUIContent levelPrefix = new GUIContent("Level Prefix [?]:", "Prefix for loaded scene name");
		
		private static GUIContent sdkVersion = new GUIContent("SDK Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
		
		private static GUIContent UseHttpsLabel = new GUIContent("Use HTTPS [?]", "Enable data send  over SSL");
		private static GUIContent StringEscape  = new GUIContent("String Escape [?]", "Enable All strings  escaping using WWW.EscapeURL, Escaping of strings is safer, but adds additional RAM consummation");
		private static GUIContent EditorAnalytics  = new GUIContent("Send From Editor [?]", "Enable sending analytics while you testing your game in Unity Editor");
		
		
		private static GUIContent EnableCachingLabel  = new GUIContent("Enable Caching [?]", "When Internet Connection is not available event hits will be saved and sanded when connection is recovered.");
		private static GUIContent EnableQueueTimeLabel  = new GUIContent("Enable Queue Time [?]", "Queue Time to report time when hit occurred. But values greater than four hours may lead to hits not being processed by Google.");
		
		
		
		private static GUIContent UsePlaterSettingLabel  = new GUIContent("Use Player Settings [?]", "Use Player setting as app name and version info");

		void Awake() {
			UpdateLibsInstalation();
		}

		
		public static void DrawAnalyticsSettings() {
			GUI.changed = false;
			
			if(GA_Settings.Instance.IsDisabled) {
				GUI.enabled = false;
			} else {
				GUI.enabled = true;
			}
			
			Messages ();
			EditorGUILayout.Space ();
			Accounts ();
			EditorGUILayout.Space ();
			GeneralOptions ();
			EditorGUILayout.Space ();
			AdvancedTracking ();
			EditorGUILayout.Space ();
			AutoTracking ();
			EditorGUILayout.Space ();
			FirebaseSettings ();
			EditorGUILayout.Space ();
			AboutGUI ();
			
			ButtonsGUI();
			
			if(GUI.changed) {
				DirtyEditor();
			}
		}
		
		
		
		public override void OnInspectorGUI() {
			DrawAnalyticsSettings();
		}

		private static void FirebaseSettings() {
			EditorGUILayout.HelpBox("Firebase Analytics", MessageType.None);
			EditorGUI.BeginChangeCheck ();
			GA_Settings.Instance.EnableFirebase = EditorGUILayout.Toggle (new GUIContent("Enable Firebase Analitycs [?]",
				"Sets whether the Firebase Analytics collection is enabled for this application"),
				GA_Settings.Instance.EnableFirebase);
			GA_Settings.Instance.FirebaseAppId = EditorGUILayout.TextField (new GUIContent("App ID [?]", "Use this number to identify your app when you contact Firebase support."),
				GA_Settings.Instance.FirebaseAppId);
			if(EditorGUI.EndChangeCheck()) {				
				UpdateLibsInstalation();
			}
		}
		
		private static void Messages() {
			#if UNITY_WEBPLAYER  
			EditorGUILayout.HelpBox("Sending analytics in Editor is disabled because you using Web Player Mode. To find out more about analytics usgae in web player, please click the link bellow", MessageType.Warning);
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.Space();
			if(GUILayout.Button("Using With Web Player",  GUILayout.Width(150))) {
				Application.OpenURL("http://goo.gl/5lbHLd");
			}
			
			EditorGUILayout.EndHorizontal();
			
			#endif
			
		}

		private static void UpdateLibsInstalation() {
			if(GA_Settings.Instance.AutoCampaignTracking) {
				SA.Common.Editor.Instalation.EnableAndroidCampainAPI();
			} else {
				SA.Common.Editor.Instalation.DisableAndroidCampainAPI();
			}

			SA.Manifest.Manager.Refresh();

			SA.Manifest.Template Manifest =  SA.Manifest.Manager.GetManifest();
			SA.Manifest.ApplicationTemplate application =  Manifest.ApplicationTemplate;
			SA.Manifest.ActivityTemplate launcherActivity = application.GetLauncherActivity();

			SA.Manifest.PropertyTemplate targetSdk = Manifest.GetOrCreatePropertyWithTag ("uses-sdk");
			targetSdk.SetValue ("android:targetSdkVersion", "25");

			if(launcherActivity.Name == "com.androidnative.AndroidNativeBridge") {
				launcherActivity.SetName("com.unity3d.player.UnityPlayerNativeActivity");
			}

			foreach (KeyValuePair<int, SA.Manifest.ActivityTemplate> a in application.Activities) {
				if (a.Value.Name.Equals("com.unity3d.player.UnityPlayerNativeActivity") && !a.Value.IsLauncher) {
					application.RemoveActivity(a.Value);
					break;
				}
			}

			SA.Manifest.PropertyTemplate internalReceiver = application.GetOrCreatePropertyWithName ("receiver", "com.google.firebase.iid.FirebaseInstanceIdInternalReceiver");
			SA.Manifest.PropertyTemplate provider = application.GetOrCreatePropertyWithName ("provider", "com.google.firebase.provider.FirebaseInitProvider");
			SA.Manifest.PropertyTemplate service = application.GetOrCreatePropertyWithName ("service", "com.google.firebase.iid.FirebaseInstanceIdService");
			SA.Manifest.PropertyTemplate receiver = application.GetOrCreatePropertyWithName ("receiver", "com.google.firebase.iid.FirebaseInstanceIdReceiver");

			////////////////////////
			//Firebase Analytics
			////////////////////////
			if (GA_Settings.Instance.EnableFirebase) {
				SA.Common.Editor.Instalation.GAEnableFirebaseAPI (GA_Settings.Instance.FirebaseAppId);

				internalReceiver.SetValue ("android:exported", "false");

				receiver.SetValue ("android:permission", "com.google.android.c2dm.permission.SEND");
				receiver.SetValue ("android:exported", "true");
				SA.Manifest.PropertyTemplate filter = receiver.GetOrCreateIntentFilterWithName ("com.google.android.c2dm.intent.RECEIVE");
				filter.GetOrCreatePropertyWithName ("action", "com.google.android.c2dm.intent.REGISTRATION");
				filter.GetOrCreatePropertyWithName ("category", SA.Common.Editor.Tools.ApplicationIdentifier);

				service.SetValue ("android:exported", "true");
				SA.Manifest.PropertyTemplate intentFilter = service.GetOrCreateIntentFilterWithName ("com.google.firebase.INSTANCE_ID_EVENT");
				intentFilter.SetValue ("android:priority", "-500");

				provider.SetValue ("android:initOrder", "100");
				provider.SetValue ("android:exported", "false");
				provider.SetValue ("android:authorities", SA.Common.Editor.Tools.ApplicationIdentifier + ".firebaseinitprovider");
			} else {
				SA.Common.Editor.Instalation.GADisableFirebaseAPI ();

				application.RemoveProperty (internalReceiver);
				application.RemoveProperty (provider);
				application.RemoveProperty (service);
				application.RemoveProperty (receiver);
			}

			SA.Manifest.Manager.SaveManifest();
		}
		
		
		private static void Accounts() {
			EditorGUILayout.HelpBox("(Required) Platfroms and Accounts", MessageType.None);
			if (GA_Settings.Instance.accounts.Count == 0) {
				EditorGUILayout.HelpBox("Setup at least one Google Analytics Account", MessageType.Error);
			}
			
			
			GA_Settings.Instance.showAccounts = EditorGUILayout.Foldout(GA_Settings.Instance.showAccounts, "Google Analytics Account");
			if(GA_Settings.Instance.showAccounts) {
				
				
				foreach(Profile profile in GA_Settings.Instance.accounts) {
					
					EditorGUI.indentLevel = 1;
					EditorGUILayout.BeginVertical (GUI.skin.box);
					profile.IsOpen = EditorGUILayout.Foldout(profile.IsOpen, profile.Name);
					if(profile.IsOpen) {
						EditorGUI.indentLevel = 2;
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(acountNameLabel);
						profile.Name	 	= EditorGUILayout.TextField(profile.Name);
						EditorGUILayout.EndHorizontal();
						
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(appTrackingIdLabel);
						profile.TrackingID	 	= EditorGUILayout.TextField(profile.TrackingID);
						EditorGUILayout.EndHorizontal();
						
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						
						if(GUILayout.Button("Set For All Platfroms",  GUILayout.Width(150))) {
							int options = EditorUtility.DisplayDialogComplex(
								"Setting Account",
								"Setting " + profile.Name + " for all platfroms",
								"Set As Testing",
								"Cancel",
								"Set As Production"
								);
							
							switch(options) {
							case 0:
								
								foreach(RuntimePlatform platfrom in (RuntimePlatform[]) System.Enum.GetValues(typeof(RuntimePlatform)) ) {
									GA_Settings.Instance.SetProfileIndexForPlatfrom(platfrom, GA_Settings.Instance.GetProfileIndex(profile), true);
								}
								
								break;
							case 2:
								foreach(RuntimePlatform platfrom in (RuntimePlatform[]) System.Enum.GetValues(typeof(RuntimePlatform)) ) {
									GA_Settings.Instance.SetProfileIndexForPlatfrom(platfrom, GA_Settings.Instance.GetProfileIndex(profile), false);
								}
								break;
								
								
							}
							
							DirtyEditor();
						}
						
						
						if(GUILayout.Button("Remove",  GUILayout.Width(80))) {
							GA_Settings.Instance.RemoveProfile(profile);
							return;
						}
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space();
						
					}
					
					EditorGUILayout.EndVertical ();
					
				}
				
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.Space();
				if(GUILayout.Button("Add New Account",  GUILayout.Width(120))) {
					GA_Settings.Instance.AddProfile(new Profile());
				}
				
				EditorGUILayout.EndHorizontal();
				
			}
			
			GA_Settings.Instance.showPlatfroms = EditorGUILayout.Foldout(GA_Settings.Instance.showPlatfroms, "Platfroms Settings");
			if(GA_Settings.Instance.showPlatfroms) {
				
				if (GA_Settings.Instance.accounts.Count == 0) {
					EditorGUILayout.HelpBox("Setup at least one Google Analytics Profile", MessageType.Error);
				} else {
					foreach(RuntimePlatform platfrom in (RuntimePlatform[]) System.Enum.GetValues(typeof(RuntimePlatform)) ) {
						
						EditorGUI.indentLevel = 1;
						EditorGUILayout.BeginVertical (GUI.skin.box);
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUI.indentLevel = 0;
						EditorGUILayout.LabelField(platfrom.ToString());
						EditorGUILayout.EndHorizontal();
						
						
						EditorGUI.indentLevel = 1;
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(prodModeLabel);
						EditorGUI.BeginChangeCheck();
						
						
						int index = GA_Settings.Instance.GetProfileIndexForPlatfrom(platfrom, false);
						index = EditorGUILayout.Popup(index, GA_Settings.Instance.GetProfileNames());
						
						
						if(EditorGUI.EndChangeCheck()) {
							GA_Settings.Instance.SetProfileIndexForPlatfrom(platfrom, index, false);
							DirtyEditor();
						}
						EditorGUILayout.EndHorizontal();
						
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(testingModeLabel);
						EditorGUI.BeginChangeCheck();
						
						
						index = GA_Settings.Instance.GetProfileIndexForPlatfrom(platfrom, true);
						index = EditorGUILayout.Popup(index, GA_Settings.Instance.GetProfileNames());
						
						
						if(EditorGUI.EndChangeCheck()) {
							GA_Settings.Instance.SetProfileIndexForPlatfrom(platfrom, index, true);
							DirtyEditor();
						}
						
						EditorGUILayout.EndHorizontal();
						
						
						
						
						
						
						
						
						
						EditorGUILayout.EndVertical ();
					}
				}
				
			}
			
			
			GA_Settings.Instance.showTestingMode = EditorGUILayout.Foldout(GA_Settings.Instance.showTestingMode, "Testing Mode");
			if(GA_Settings.Instance.showTestingMode) {
				if (GA_Settings.Instance.accounts.Count == 0) {
					EditorGUILayout.HelpBox("Setup at least one Google Analytics Profile", MessageType.Error);
				} else {
					EditorGUILayout.HelpBox("If Testing mode is enabled, testing account will be used on all platfroms. You will also get build warning if building with testing mode enabled. Make sure you will disable it with the production build", MessageType.Info);
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(testingModeLabel);
					GA_Settings.Instance.IsTestingModeEnabled	 	= EditorGUILayout.Toggle(GA_Settings.Instance.IsTestingModeEnabled);
					EditorGUILayout.EndHorizontal();
					
				}
			}
			
			
			
		}
		
		
		
		private static void GeneralOptions() {
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("(Required) Application Info", MessageType.None);
			
			
			if(GA_Settings.Instance.UsePlayerSettingsForAppInfo) {
				GUI.enabled = false;
				
				GA_Settings.Instance.UpdateVersionAndName();
				
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(appNameLabel);
			GA_Settings.Instance.AppName	 	= EditorGUILayout.TextField(GA_Settings.Instance.AppName);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(appVersionLabel);
			GA_Settings.Instance.AppVersion	 	= EditorGUILayout.TextField(GA_Settings.Instance.AppVersion);
			EditorGUILayout.EndHorizontal();
			
			GUI.enabled = true;
			GA_Settings.Instance.UsePlayerSettingsForAppInfo = EditorGUILayout.Toggle(UsePlaterSettingLabel, GA_Settings.Instance.UsePlayerSettingsForAppInfo);
			
			
			EditorGUILayout.Space();
			
		}
		
		
		
		
		
		private static void AdvancedTracking() {
			EditorGUILayout.HelpBox("(Optional) Edit the advanced tracking parameters", MessageType.None);
			GA_Settings.Instance.showAdvancedParams = EditorGUILayout.Foldout(GA_Settings.Instance.showAdvancedParams, "Advanced Tracking");
			if(GA_Settings.Instance.showAdvancedParams) {
				GA_Settings.Instance.UseHTTPS = EditorGUILayout.Toggle(UseHttpsLabel, GA_Settings.Instance.UseHTTPS);
				GA_Settings.Instance.StringEscaping = EditorGUILayout.Toggle(StringEscape, GA_Settings.Instance.StringEscaping);
				GA_Settings.Instance.EditorAnalytics = EditorGUILayout.Toggle(EditorAnalytics, GA_Settings.Instance.EditorAnalytics);
			}
			
			GA_Settings.Instance.showCParams = EditorGUILayout.Foldout(GA_Settings.Instance.showCParams, "Requests Caching");
			if(GA_Settings.Instance.showCParams) {
				
				GA_Settings.Instance.IsRequetsCachingEnabled = EditorGUILayout.Toggle(EnableCachingLabel,   GA_Settings.Instance.IsRequetsCachingEnabled);
				GA_Settings.Instance.IsQueueTimeEnabled 	 = EditorGUILayout.Toggle(EnableQueueTimeLabel, GA_Settings.Instance.IsQueueTimeEnabled);
			}
		}
		
		
		private static void AutoTracking() {
			EditorGUILayout.HelpBox("(Optional) Edit the automatic tracking parameters", MessageType.None);
			GA_Settings.Instance.showAdditionalParams = EditorGUILayout.Foldout(GA_Settings.Instance.showAdditionalParams, "Automatic Tracking");
			if (GA_Settings.Instance.showAdditionalParams) {
				
				
				GA_Settings.Instance.AutoExceptionTracking = EditorGUILayout.Toggle(exTrackingLabel, GA_Settings.Instance.AutoExceptionTracking);
				GA_Settings.Instance.SubmitSystemInfoOnFirstLaunch = EditorGUILayout.Toggle(systemInfoTrackingLabel, GA_Settings.Instance.SubmitSystemInfoOnFirstLaunch);
				GA_Settings.Instance.AutoAppResumeTracking = EditorGUILayout.Toggle(pauseTrackingLabel, GA_Settings.Instance.AutoAppResumeTracking);
				GA_Settings.Instance.AutoAppQuitTracking = EditorGUILayout.Toggle(quitTrackingLabel, GA_Settings.Instance.AutoAppQuitTracking);

				EditorGUI.BeginChangeCheck ();
				GA_Settings.Instance.AutoCampaignTracking = EditorGUILayout.Toggle(CampaignTrackingLabel, GA_Settings.Instance.AutoCampaignTracking);
				if(EditorGUI.EndChangeCheck()) {
					UpdateLibsInstalation();
				}
				
				GA_Settings.Instance.AutoLevelTracking = EditorGUILayout.Toggle(newLevelTrackingLabel, GA_Settings.Instance.AutoLevelTracking);
				
				if(GA_Settings.Instance.AutoLevelTracking) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(levelPrefix);
					EditorGUILayout.LabelField(levelPostfix);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					GA_Settings.Instance.LevelPrefix =  EditorGUILayout.TextField(GA_Settings.Instance.LevelPrefix);
					GA_Settings.Instance.LevelPostfix = EditorGUILayout.TextField(GA_Settings.Instance.LevelPostfix);
					EditorGUILayout.EndHorizontal();
				}				
			}
		}
		
		private static void ButtonsGUI() {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.Space();
			if(GUILayout.Button("Refresh Client Id",  GUILayout.Width(120))) {
				PlayerPrefs.DeleteKey(Manager.GOOGLE_ANALYTICS_CLIENTID_PREF_KEY);
			}
			
			
			GUI.enabled = true;
			
			Color c = GUI.color;
			string text = "";
			if(GA_Settings.Instance.IsDisabled) {
				text = "Enable Analytics";
				GUI.color = Color.green;
			} else {
				text = "Disable Analytics";
				GUI.color = Color.red;
			}
			
			
			if(GUILayout.Button(text, GUILayout.Width(120))) {
				GA_Settings.Instance.IsDisabled = !GA_Settings.Instance.IsDisabled;
			}
			
			GUI.color = c;
			
			
			EditorGUILayout.EndHorizontal();
		}
		
		
		private static void AboutGUI() {
			
			EditorGUILayout.HelpBox("About the Plugin", MessageType.None);

			SA.Common.Editor.Tools.SelectableLabelField(sdkVersion,   GA_Settings.VERSION_NUMBER);
			SA.Common.Editor.Tools.SupportMail();
			
			SA.Common.Editor.Tools.DrawSALogo();
			
			
		}
		
		private static void SelectableLabelField(GUIContent label, string value) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
			EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
			EditorGUILayout.EndHorizontal();
		}
		
		
		
		private static void DirtyEditor() {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(GA_Settings.Instance);
			#endif
		}
		
		
	}

}
#endif
