using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AMN_ProxyPool  {
	
	#if UNITY_ANDROID
	private static Dictionary<string, AndroidJavaObject> pool = new Dictionary<string, AndroidJavaObject>();

	private static bool isInitialized = false;
	#endif

	public static void CallStatic(string className, string methodName, params object[] args) {
		#if UNITY_ANDROID		
		
		if(Application.platform != RuntimePlatform.Android) {
			return;
		}
		
		try {
			
			AndroidJavaObject bridge;
			if(pool.ContainsKey(className)) {
				bridge = pool[className];
			} else {
				AndroidJavaClass jc = new AndroidJavaClass("com.amazonnative.AMNMobileAd");
				bridge = jc.CallStatic<AndroidJavaObject>("getInstance");
				pool.Add(className, bridge);
			}

			if(isInitialized) {
				bridge.Call(methodName, args);
			}
			else {
				isInitialized = true;
			}
			
		} catch(System.Exception ex) {
			Debug.LogWarning(ex.Message);
		}
		#endif
	}
//	com.unity3d.player.UnityPlayer
}
