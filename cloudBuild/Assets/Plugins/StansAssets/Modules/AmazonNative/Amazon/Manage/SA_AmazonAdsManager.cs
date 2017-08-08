#define AMAZON_ADVERTISING_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections.Generic;

public class SA_AmazonAdsManager: AMN_Singleton<SA_AmazonAdsManager> {

	public event Action<AMN_InterstitialDataResult> OnInterstitialDataReceived = delegate {};
	public event Action<AMN_InterstitialDismissedResult> OnInterstitialDismissed 	 = delegate {};
	
	public const string DATA_SPLITTER = "|";

	private bool _isInitialized = false;

	private Dictionary<int, AmazonAdBanner> _banners = new Dictionary<int, AmazonAdBanner>();
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public bool IsInitialized {
		get {
			return _isInitialized;
		}
		set {
			_isInitialized = value;
		}
	}

	public Dictionary<int, AmazonAdBanner> Banners {
		get {
			return _banners;
		}
	}
	
	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------	
	
	public void Create() {
		#if AMAZON_ADVERTISING_ENABLED
		AMN_AdvertisingProxy.GetInstance();
		
		Debug.Log ("UNITY SA_AmazonAdsManager was created");
		#endif
	}
	
	public void Init(string api_key, bool isTestMode) {	
		#if AMAZON_ADVERTISING_ENABLED
		AMN_AdvertisingProxy.Init(api_key, isTestMode);
		#endif
		IsInitialized = true;
	}

	public int CreateBanner(AmazonAdBanner.BannerAligns position) {
		#if AMAZON_ADVERTISING_ENABLED
		AmazonAdBanner banner = new AmazonAdBanner(position, SA.Common.Util.IdFactory.NextId);
		_banners.Add(banner.Id, banner);

		return banner.Id;
		#else
		return 0;
		#endif
	}

	public bool IsBannerLoaded(int id) {
		if (_banners.ContainsKey(id)) {
			return _banners[id].IsLoaded;
		} else {
			Debug.Log("There is NO Ad Banner with such an ID " + id);
			return false;
		}
	}

	public bool IsBannerOnScreen(int id) {
		if (_banners.ContainsKey(id)) {
			return _banners[id].IsOnScreen;
		} else {
			Debug.Log("There is NO Ad Banner with such an ID " + id);
			return false;
		}
	}

	public void RefreshBanner(int id) {
		#if AMAZON_ADVERTISING_ENABLED
		if (_banners.ContainsKey(id)) {
			_banners[id].Refresh();
		}
		#endif
	}
		
	public void DestroyBanner (int id) {
		#if AMAZON_ADVERTISING_ENABLED
		if (_banners.ContainsKey(id)) {
			_banners[id].Destroy();
		}
		#endif
	}

	public void HideBanner(bool hide, int id) {
		#if AMAZON_ADVERTISING_ENABLED
		if (_banners.ContainsKey(id)) {
			_banners[id].Hide(hide);
		}
		#endif
	}

	public void LoadInterstitial () {
		#if AMAZON_ADVERTISING_ENABLED
		AMN_AdvertisingProxy.LoadInterstitial();
		#endif
	}

	public void ShowInterstitial () {
		#if AMAZON_ADVERTISING_ENABLED
		AMN_AdvertisingProxy.ShowInterstitial();
		#endif
	}
		
	//--------------------------------------
	// BANNER EVENTS
	//--------------------------------------
	#if AMAZON_ADVERTISING_ENABLED
	private void OnAdLoaded (string data) {
		Debug.Log("OnAdLoaded data: " + data);

		string[] raw = data.Split(new string[] {DATA_SPLITTER}, StringSplitOptions.None);
		int id = Int32.Parse(raw[0]);

		int width = Int32.Parse(raw[1]);
		int height = Int32.Parse(raw[2]);

		bool canExpand = Boolean.Parse(raw[3]);
		bool canPlayAudio = Boolean.Parse(raw[4]);
		bool canPlayVideo = Boolean.Parse(raw[5]);
		string adType = raw[6];

		AMN_AdProperties props = new AMN_AdProperties(canExpand, canPlayAudio, canPlayVideo, adType);

		if (_banners.ContainsKey(id)) {
			_banners[id].SetProperties(width, height, props);
			_banners[id].HandleOnBannerAdLoaded();
		}
	}
	
	private void OnAdFailedToLoad (string data) {
		Debug.Log("OnBannerFailed with error " + data);

		string[] raw = data.Split(new string[] {DATA_SPLITTER}, StringSplitOptions.None);
		int id = Int32.Parse(raw[0]);
		if (_banners.ContainsKey(id)) {
			_banners[id].HandleOnBannerAdFailedToLoad();
		}
	}

	private void onAdCollapsed (string data) {
		Debug.Log("onAdCollapsed warning " + data);

		int id = Int32.Parse(data);
		if (_banners.ContainsKey(id)) {
			_banners[id].HandleOnBannerAdCollapsed();
		}
	}
	
	private void onAdDismissed (string data) {
		Debug.Log("onAdDismissed warning " + data);

		int id = Int32.Parse(data);
		if (_banners.ContainsKey(id)) {
			_banners[id].HandleOnBannerAdDismissed();
		}
	}
	
	private void onAdExpanded (string data) {
		Debug.Log("onAdExpanded warning " + data);

		int id = Int32.Parse(data);
		if (_banners.ContainsKey(id)) {
			_banners[id].HandleOnBannerAdExpanded();
		}
	}

	//--------------------------------------
	// INTERSTITIAL EVENTS
	//--------------------------------------
	
	private void OnInterstitialsLoaded (string adProperties) {
		SA_AmazonAdsExample.isInterstitialLoaded = true;

		string[] storeData = adProperties.Split(DATA_SPLITTER [0]);
		
		AMN_InterstitialDataResult result = new AMN_InterstitialDataResult (storeData);
		OnInterstitialDataReceived(result);
	}
	
	private void OnInterstitialsFailed (string error_message ) {
		SA_AmazonAdsExample.isInterstitialLoaded = false;

		Debug.Log("OnInterstitialsFailed with error " + error_message);

		AMN_InterstitialDataResult result = new AMN_InterstitialDataResult (error_message);
		OnInterstitialDataReceived(result);
	}
	
	private void OnInterstitialsDismissed (string warning_message) {
		SA_AmazonAdsExample.isInterstitialLoaded = false;

		Debug.Log("OnInterstitialsDismissed warning " + warning_message);

		AMN_InterstitialDismissedResult result = new AMN_InterstitialDismissedResult (warning_message);
		OnInterstitialDismissed(result);
	}
	#endif
	//--------------------------------------
	// PRIVATE API CALL METHODS
	//--------------------------------------

}
