using UnityEngine;
using System.Collections;

public class SA_AmazonAdsExample : MonoBehaviour {

	public string api_key  = "f06565f7696840d7adce3d08ea18d742";
	public bool isTestMode = true;	
	public static bool isInterstitialLoaded = false;

	private float RefreshInterval = 30f;
	private bool IsBannerCreated = false;
	private string message;

	private int bannerId;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Start () {
		SA_AmazonAdsManager.Instance.Create();

		SubscribeToEvents();

		InvokeRepeating ("RefreshBanner", RefreshInterval, RefreshInterval);
	}

	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE API CALL METHODS
	//--------------------------------------

	private void InitializeAmazonAds() {
		if (!SA_AmazonAdsManager.Instance.IsInitialized) {
			SA_StatusBar.text = "Initializing Amazon Ads";
			SA_AmazonAdsManager.Instance.Init(api_key, isTestMode);
		}
	}

	private void CreateBanner() {
		if (SA_AmazonAdsManager.Instance.IsInitialized) {
			bannerId = SA_AmazonAdsManager.Instance.CreateBanner(AmazonAdBanner.BannerAligns.Bottom);

			IsBannerCreated = true;
		}
		else {
			Debug.Log ("Amazon ads are not initialized yet");
		}
	}

	private void RefreshBanner() {
		if (SA_AmazonAdsManager.Instance.IsInitialized) {
			SA_AmazonAdsManager.Instance.RefreshBanner(bannerId);
		}
		else {
			Debug.Log ("Amazon ads are not initialized yet");
		}
	}

	private void DestroyBanner() {
		if (SA_AmazonAdsManager.Instance.IsInitialized) {
			if (IsBannerCreated) {
				SA_AmazonAdsManager.Instance.DestroyBanner(bannerId);
				
				IsBannerCreated = false;
			}
			else {
				Debug.Log ("Banner is not created yet!");
			}
		}
		else {
			Debug.Log ("Amazon ads are not initialized yet");
		}
	}

	private void HideBanner() {
		HideBanner(true);
	}
	
	private void ShowBanner() {
		HideBanner(false);
	}

	private void HideBanner(bool hide){
		if (SA_AmazonAdsManager.Instance.IsInitialized) {
			if (IsBannerCreated) {
				SA_AmazonAdsManager.Instance.HideBanner(hide, bannerId);
			}
			else {
				Debug.Log ("Banner is not created yet!");
			}
		}
		else {
			Debug.Log ("Amazon ads are not initialized yet");
		}
	}

	private void LoadInterstitial() {
		if (SA_AmazonAdsManager.Instance.IsInitialized) {
			SA_AmazonAdsManager.Instance.LoadInterstitial();
		}
		else {
			Debug.Log ("Amazon ads are not initialized yet");
		}
	}

	public void ShowInterstitial() {		
		if (SA_AmazonAdsManager.Instance.IsInitialized) {
			if (isInterstitialLoaded) {				
				SA_AmazonAdsManager.Instance.ShowInterstitial();

				isInterstitialLoaded = false;
			}
		}
		else {
			Debug.Log ("Amazon ads are not initialized yet");
		}
	}
	
	private void SubscribeToEvents () {
		SA_AmazonAdsManager.Instance.OnInterstitialDataReceived += OnInterstitialDataReceived;
		SA_AmazonAdsManager.Instance.OnInterstitialDismissed += OnInterstitialDismissed;
	}

	//--------------------------------------
	// INTERSTITIAL EVENTS
	//--------------------------------------

	private void OnInterstitialDataReceived (AMN_InterstitialDataResult result) {
		AMN_AdProperties properties = result.Properties;
		
		Debug.Log("OnInterstitialDataReceived with result success " + result.isSuccess + " " + properties);
	}

	private void OnInterstitialDismissed (AMN_InterstitialDismissedResult result) {
		message = result.Error_message;
		
		Debug.Log("OnInterstitialDismissed with result success " + result.isSuccess + " and message " + message);
	}
}
