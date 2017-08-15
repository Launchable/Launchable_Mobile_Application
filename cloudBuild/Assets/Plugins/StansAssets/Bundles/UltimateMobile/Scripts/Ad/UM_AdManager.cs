using UnityEngine;
using System;
using System.Collections;

public static class UM_AdManager  {

	public static event Action OnInterstitialLoaded 		= delegate{};
	public static event Action OnInterstitialLoadFail 		= delegate{};
	public static event Action OnInterstitialClosed 		= delegate{};

	private static bool _IsInited = false ;

	private static bool _AmazonAdsShowOnLoad = false;
	
	//--------------------------------------
	//  Initalization
	//--------------------------------------

	public static void Init ()
	{
		switch (Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
				GoogleMobileAd.Init ();
				GoogleMobileAdInterstitialSubscribe ();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.Create ();
				SA_AmazonAdsManager.Instance.Init (AmazonNativeSettings.Instance.AppAPIKey, AmazonNativeSettings.Instance.IsTestMode);

				SA_AmazonAdsManager.Instance.OnInterstitialDataReceived += AmazonInterstitialDataReceived;
				SA_AmazonAdsManager.Instance.OnInterstitialDismissed += AmazonInterstitialDismissed;
			} else {
				GoogleMobileAd.Init ();
				GoogleMobileAdInterstitialSubscribe ();
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.Init ();
			GoogleMobileAdInterstitialSubscribe ();
			break;
		}
		
		_IsInited = true;
	}

	public static void ResetActions(){
		OnInterstitialLoaded = delegate{};
		OnInterstitialLoadFail = delegate{};
		OnInterstitialClosed = delegate{};
	}


	//--------------------------------------
	//  GET / SET
	//--------------------------------------
	
	
	public static bool IsInited {
		get {
			return _IsInited;
		}
	}


	//--------------------------------------
	//  Public Methods
	//--------------------------------------


	public static int CreateAdBanner(TextAnchor anchor, GADBannerSize size = GADBannerSize.BANNER)  {
		if(!_IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return 0;
		}

		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
				return GoogleMobileAd.CreateAdBanner (anchor, size).id;
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				return SA_AmazonAdsManager.Instance.CreateBanner (AmazonAdBanner.BannerAligns.Bottom);
			} else {
				return GoogleMobileAd.CreateAdBanner (anchor, size).id;
			}
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			return GoogleMobileAd.CreateAdBanner(anchor, size).id;
		}

		return 0;

	}
	


	public static bool IsBannerLoaded(int id) {

		if(!_IsInited) {
			Debug.LogWarning ("IsBannerLoaded shoudl be called only after Init function. Call ignored");
			return false;
		}


		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
				return GoogleMobileAd.GetBanner (id).IsLoaded;
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				return SA_AmazonAdsManager.Instance.IsBannerLoaded(id);
			} else {
				return GoogleMobileAd.GetBanner(id).IsLoaded;
			}
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			return GoogleMobileAd.GetBanner(id).IsLoaded;
		}

		return false;
		
	}

	public static bool IsBannerOnScreen(int id) {

		if(!_IsInited) {
			Debug.LogWarning ("IsBannerOnScreen shoudl be called only after Init function. Call ignored");
			return false;
		}

		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if (UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
				return GoogleMobileAd.GetBanner (id).IsOnScreen;
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				return SA_AmazonAdsManager.Instance.IsBannerOnScreen(id);
			} else {
				return GoogleMobileAd.GetBanner(id).IsOnScreen;
			}
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			return GoogleMobileAd.GetBanner(id).IsOnScreen;
		}
		
		return false;
		
	}


	public static void DestroyBanner(int id) {
		if(!_IsInited) {
			Debug.LogWarning ("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:

			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.DestroyBanner(id);
			} else {
				//iAdBannerController.instance.DestroyBanner(id);
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.DestroyBanner(id);
			} else {
				GoogleMobileAd.DestroyBanner(id);
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.DestroyBanner(id);
			break;
		}
	}


	public static void HideBanner(int id) {
		if(!_IsInited) {
			Debug.LogWarning ("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.GetBanner(id).Hide();
			} else {
				//iAdBannerController.instance.GetBanner(id).Hide();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.HideBanner(true, id);
			} else {
				GoogleMobileAd.GetBanner(id).Hide();
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.GetBanner(id).Hide();
			break;
		}
	}




	public static void ShowBanner(int id) {
		if(!_IsInited) {
			Debug.LogWarning ("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.GetBanner(id).Show();
			} else {
				//iAdBannerController.instance.GetBanner(id).Show();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.HideBanner(false, id);
			} else {
				GoogleMobileAd.GetBanner(id).Show();
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.GetBanner(id).Show();
			break;
		}
	}


	public static void RefreshBanner(int id) {
		if(!_IsInited) {
			Debug.LogWarning ("DestroyCurrentBanner shoudl be called only after Init function. Call ignored");
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.GetBanner(id).Refresh();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.RefreshBanner(id);
			} else {
				GoogleMobileAd.GetBanner(id).Refresh();
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.GetBanner(id).Refresh();
			break;
		}
	}
	


	public static void StartInterstitialAd() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.StartInterstitialAd();
			} else {
				//iAdBannerController.instance.StartInterstitialAd();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				_AmazonAdsShowOnLoad = true;
				SA_AmazonAdsManager.Instance.LoadInterstitial();
			} else {
				GoogleMobileAd.StartInterstitialAd();
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.StartInterstitialAd();
			break;
		}
	}
	
	public static void LoadInterstitialAd() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.LoadInterstitialAd();
			} else {
				//iAdBannerController.instance.LoadInterstitialAd();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.LoadInterstitial();
			} else {
				GoogleMobileAd.LoadInterstitialAd();	
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.LoadInterstitialAd();
			break;
		}
	}

	public static void ShowInterstitialAd() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:			
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd)  {
				GoogleMobileAd.ShowInterstitialAd();
			} else {
				//iAdBannerController.instance.ShowInterstitialAd();
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonAdsManager.Instance.ShowInterstitial();
			} else {
				GoogleMobileAd.ShowInterstitialAd();
			}
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			GoogleMobileAd.ShowInterstitialAd();
			break;
		}
	}

	//--------------------------------------
	//  Private Methods
	//--------------------------------------

	private static void GoogleMobileAdInterstitialSubscribe() {
		GoogleMobileAd.OnInterstitialLoaded += InterstitialLoadedHandler;
		GoogleMobileAd.OnInterstitialFailedLoading += InterstitialLoadFailHandler;
		GoogleMobileAd.OnInterstitialClosed += InterstitialClosedHandler;
	}

	//--------------------------------------
	//  Handlers
	//--------------------------------------

	private static void InterstitialLoadedHandler() {
		OnInterstitialLoaded();
	}

	private static void InterstitialLoadFailHandler() {
		OnInterstitialLoadFail();
	}

	private static void InterstitialClosedHandler() {
		OnInterstitialClosed();
	}

	private static void AmazonInterstitialDismissed (AMN_InterstitialDismissedResult result) {
		OnInterstitialClosed();
	}

	private static void AmazonInterstitialDataReceived (AMN_InterstitialDataResult result) {
		if (result.isSuccess) {
			OnInterstitialLoaded();
			if (_AmazonAdsShowOnLoad) {
				SA_AmazonAdsManager.Instance.ShowInterstitial();
			}
		} else {
			OnInterstitialLoadFail();
		}
	}

}
