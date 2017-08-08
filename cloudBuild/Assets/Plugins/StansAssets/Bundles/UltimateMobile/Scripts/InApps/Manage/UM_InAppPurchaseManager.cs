//#define ATC_SUPPORT_ENABLED

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if ATC_SUPPORT_ENABLED
using CodeStage.AntiCheat.ObscuredTypes;
#endif

public class UM_InAppPurchaseManager  {
	

	private const string PREFS_KEY = "UM_InAppPurchaseManager";
	
	//--------------------------------------
	// Initializations
	//--------------------------------------


	[System.Obsolete("Instance is deprectaed, please use Client instead")]
	public static UM_InAppClient Instance { get { return Client; } }

	[System.Obsolete("instance is deprectaed, please use Client instead")]
	public static UM_InAppClient instance { get { return Client; } }

	public static UM_InAppClient _Client = null;
	public static UM_InAppClient Client {
		get {
			if(_Client ==  null) {

				switch(Application.platform) {
				case RuntimePlatform.IPhonePlayer:
				#if UNITY_5_3 || UNITY_5_4
				case RuntimePlatform.tvOS:
				#endif
					_Client = new UM_IOS_InAppClient();
					break;

				case RuntimePlatform.Android:
					if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
						_Client = new UM_Amazon_InAppClient();
					} else {
						_Client =  new UM_Android_InAppClient();
					}
					break;
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				case RuntimePlatform.WP8Player:
				#else
				case RuntimePlatform.WSAPlayerARM:
				case RuntimePlatform.WSAPlayerX64:
				case RuntimePlatform.WSAPlayerX86:
				#endif
					_Client =  new UM_WP8_InAppClient();
					break;
				default:
					if(Application.isEditor && UltimateMobileSettings.Instance.Is_InApps_EditorTestingEnabled) {
						_Client =  new UM_Editor_InAppClient();
					} else {
						_Client = new UM_Disabled_InAppClient();
					}
					break;
				}

				_Client.OnPurchaseFinished += ClientPurchaseFinishedHadnler;

					
			}
			return _Client;
		}
	}



	//--------------------------------------
	// Get / Set
	//--------------------------------------


	public static List<UM_InAppProduct> InAppProducts {
		get {
			return UltimateMobileSettings.Instance.InAppProducts;
		}
	}

	
	//--------------------------------------
	// Public Methods
	//--------------------------------------

	

		


	
	public static UM_InAppProduct GetProductById(string id) {
		return UltimateMobileSettings.Instance.GetProductById(id);
	}
	
	
	public static UM_InAppProduct GetProductByIOSId(string id) {
		return UltimateMobileSettings.Instance.GetProductByIOSId(id);
	}
	
	
	public static UM_InAppProduct GetProductByAndroidId(string id) {
		return UltimateMobileSettings.Instance.GetProductByAndroidId(id);
	}

	public static UM_InAppProduct GetProductByAmazonId(string id) {
		return UltimateMobileSettings.Instance.GetProductByAmazonId(id);
	}
	
	public static UM_InAppProduct GetProductByWp8Id(string id) {
		return UltimateMobileSettings.Instance.GetProductByWp8Id(id);
	}
	


	//--------------------------------------
	// Static Methods
	//--------------------------------------

	public static bool IsLocalPurchaseRecordExists(UM_InAppProduct product) {
		if(product == null) {
			return false;
		}

		if(product.IsConsumable) {
			return false;
		}

		#if ATC_SUPPORT_ENABLED
		return ObscuredPrefs.HasKey(PREFS_KEY + product.id);
		#else
		return PlayerPrefs.HasKey(PREFS_KEY + product.id);
		#endif
	}

	public static bool IsLocalPurchaseRecordExists(string productId) {
		if (string.IsNullOrEmpty(productId)) {
			return false;
		}

		#if ATC_SUPPORT_ENABLED
		return ObscuredPrefs.HasKey(PREFS_KEY + productId);
		#else
		return PlayerPrefs.HasKey(PREFS_KEY + productId);
		#endif
	}  

	public static void SaveNonConsumableItemPurchaseInfo(UM_InAppProduct product) {
		#if ATC_SUPPORT_ENABLED
		ObscuredPrefs.SetInt(PREFS_KEY + product.id, 1);
		#else
		PlayerPrefs.SetInt(PREFS_KEY + product.id, 1);
		#endif
	}


	public static void UpdatePlatfromsInAppSettings() {

		IOSNativeSettings.Instance.InAppProducts.Clear();
		AndroidNativeSettings.Instance.InAppProducts.Clear();
		AmazonNativeSettings.Instance.InAppProducts.Clear ();
		foreach(UM_InAppProduct prod in UltimateMobileSettings.Instance.InAppProducts) {
			AddToISNSettings(prod);
			AddToANSettings(prod);
			AddToAMMSettings(prod);
		}
	}
		

	//--------------------------------------
	// Private Methods
	//--------------------------------------


	private static void AddToANSettings(UM_InAppProduct prod) {

		if(prod.AndroidId.Equals(string.Empty)) { return; }

		GoogleProductTemplate newTpl =  new GoogleProductTemplate();
		newTpl.SKU = prod.AndroidId;
		newTpl.Title = prod.DisplayName;
		newTpl.Description = prod.Description;
		newTpl.Texture = prod.Texture;
		newTpl.LocalizedPrice = prod.LocalizedPrice;

		if(prod.Type == UM_InAppType.Consumable) {
			newTpl.ProductType = AN_InAppType.Consumable;
		}

		if(prod.Type == UM_InAppType.NonConsumable) {
			newTpl.ProductType = AN_InAppType.NonConsumable;
		}

		AndroidNativeSettings.Instance.InAppProducts.Add(newTpl);

	}

	private static void AddToISNSettings(UM_InAppProduct prod) {

		if(prod.IOSId.Equals(string.Empty)) { return; }

		var newTpl =  new SA.IOSNative.StoreKit.Product();
		newTpl.Id = prod.IOSId;
		newTpl.Description = prod.Description;
		newTpl.DisplayName = prod.DisplayName;
		newTpl.PriceTier = prod.PriceTier;
		newTpl.Texture = prod.Texture;
		newTpl.IsOpen = false;

		if(prod.Type == UM_InAppType.Consumable) {
			newTpl.Type = SA.IOSNative.StoreKit.ProductType.Consumable;
		}

		if(prod.Type == UM_InAppType.NonConsumable) {
			newTpl.Type = SA.IOSNative.StoreKit.ProductType.NonConsumable;
		}

		IOSNativeSettings.Instance.InAppProducts.Add(newTpl);
	}

	private static void AddToAMMSettings(UM_InAppProduct prod) {

		if(prod.AmazonId.Equals(string.Empty)) { return; }



		AmazonProductTemplate newTpl =  new AmazonProductTemplate();
		newTpl.Sku = prod.AmazonId;
		newTpl.Description = prod.Description;
		newTpl.Title = prod.DisplayName;
		newTpl.LocalizedPrice = prod.LocalizedPrice;
		newTpl.Texture = prod.Texture;
		newTpl.IsOpen = false;

		if(prod.Type == UM_InAppType.Consumable) {
			newTpl.ProductType = AMN_InAppType.CONSUMABLE;
		}

		if(prod.Type == UM_InAppType.NonConsumable) {
			newTpl.ProductType = AMN_InAppType.ENTITLED;
		}

		AmazonNativeSettings.Instance.InAppProducts.Add(newTpl);
	}


	

	//--------------------------------------
	// Action Handlers
	//--------------------------------------

	static void ClientPurchaseFinishedHadnler (UM_PurchaseResult result){
		if(!result.product.IsConsumable && result.isSuccess) {
			UM_InAppPurchaseManager.SaveNonConsumableItemPurchaseInfo(result.product);
		}
	}
}
