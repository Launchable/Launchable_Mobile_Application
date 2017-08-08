////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native
// @support stans.assets@gmail.com
//
////////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class AmazonNativeMenu : EditorWindow {
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	#if UNITY_EDITOR

	//--------------------------------------
	//  GENERAL
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Amazon Native/Edit Settings")]
	public static void Edit() {
		Selection.activeObject = AmazonNativeSettings.Instance;
	}

	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Getting Started/Plugin setup")]
	public static void GettingStartedPluginSetup() {
		string url = "https://unionassets.com/amazon-native/plugin-set-up-75";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  IN-APP-Purchases
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/In-App Purchases/Billing Set up")]
	public static void GettingBillingSetup() {
		string url = "https://unionassets.com/amazon-native/billing-set-up-76";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/In-App Purchases/Coding Guidelines")]
	public static void GettingBillingGuidelines() {
		string url = "https://unionassets.com/amazon-native/coding-guidelines-77";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  GAME CIRCLE
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Game Circle/Game Circle Set up")]
	public static void GettingGameCircleSetup() {
		string url = "https://unionassets.com/amazon-native/game-circle-set-up-197";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Game Circle/Coding Guidelines")]
	public static void GettingGameCircleGuidelines() {
		string url = "https://unionassets.com/amazon-native/coding-guidelines-198";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  Advertising
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Advertising/Advertising Set up")]
	public static void GettingAdvertisingSetup() {
		string url = "https://unionassets.com/amazon-native/advertising-set-up-443";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Advertising/Coding Guidelines")]
	public static void GettingAdvertisingGuidelines() {
		string url = "https://unionassets.com/amazon-native/coding-guidelines-444";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  PLAYMAKER
	//--------------------------------------
	
	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Playmaker/Actions list")]
	public static void GettingPlaymakerActions() {
		string url = "https://unionassets.com/amazon-native/actions-list-472";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  TROUBLESHOOTING
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Amazon Native/Documentation/Troubleshooting/Troubles with Kindle devices")]
	public static void GettingTroubleshooting() {
		string url = "https://unionassets.com/amazon-native/troubles-with-kindle-devices-269";
		Application.OpenURL(url);
	}
	#endif

}
#endif
