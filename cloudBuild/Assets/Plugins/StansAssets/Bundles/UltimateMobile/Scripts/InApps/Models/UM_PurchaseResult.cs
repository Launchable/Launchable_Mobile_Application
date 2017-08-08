using UnityEngine;
using System.Collections;

public class UM_PurchaseResult  {
	
	public bool isSuccess =  false;
	public UM_InAppProduct product =  new UM_InAppProduct();

	private int _ResponceCode = -1;
	private string EditorDummyId = string.Empty;


	public GooglePurchaseTemplate Google_PurchaseInfo = null;
	public SA.IOSNative.StoreKit.PurchaseResult IOS_PurchaseInfo = null;
	public WP8PurchseResponce WP8_PurchaseInfo = null;
	public AMN_PurchaseResponse Amazon_PurchaseInfo = null;





	
	public void SetResponceCode(int code) {
		
		_ResponceCode = code;
	}
	
	public string TransactionId {
		get {

			if(Application.isEditor) {
				if(string.IsNullOrEmpty(EditorDummyId)) {
					EditorDummyId = SA.Common.Util.IdFactory.NextId.ToString();
				}

				return EditorDummyId;
			}

			switch(Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
					return Amazon_PurchaseInfo.ReceiptId;
				} else {
					return Google_PurchaseInfo.OrderId;
				}
			case RuntimePlatform.IPhonePlayer:
				return IOS_PurchaseInfo.TransactionIdentifier;
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			case RuntimePlatform.WP8Player:
			#else
			case RuntimePlatform.WSAPlayerARM:
			case RuntimePlatform.WSAPlayerX64:
			case RuntimePlatform.WSAPlayerX86:
			#endif
				return WP8_PurchaseInfo.TransactionId;
			default:
				return string.Empty;
			}
				
		}
	}
	
	public int ResponceCode {
		get {
			return _ResponceCode;
		}
	}
}
