using UnityEngine;
using System.Collections;

public class UM_ShareUtility : MonoBehaviour {

	public static void TwitterShare(string status) {
		TwitterShare(status, null);
	}
	
	public static void TwitterShare(string status, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", status, texture, "twi");
			} else {
				AndroidSocialGate.StartShareIntent("Share", status, "twi");
			}


			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.Instance.TwitterPost(status, null, texture);
			break;

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			break;
		}
	}


	public static void InstagramShare(Texture2D texture) {
		InstagramShare(null, texture);
	}
	
	public static void InstagramShare(string status) {
		InstagramShare(status, null);
	}
	
	public static void InstagramShare(string status, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", status, texture, "com.instagram.android");
			} else {
				AndroidSocialGate.StartShareIntent("Share", status, "com.instagram.android");
			}
			
			
			break;
		case RuntimePlatform.IPhonePlayer:
			
			IOSSocialManager.Instance.InstagramPost(texture, status);
			break;
			
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			break;
		}
	}


	
	public static void FacebookShare(string message) {
		FacebookShare(message, null);
	}
	
	public static void FacebookShare(string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", message, texture, "facebook.katana");
			} else {
				AndroidSocialGate.StartShareIntent("Share", message, "facebook.katana");
			}

			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.Instance.FacebookPost(message, null, texture);
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			break;
		}
	}

	public static void WhatsappShare(string message, Texture2D texture = null) {
		switch (Application.platform) {
		case RuntimePlatform.Android:
			AndroidSocialGate.StartShareIntent (string.Empty, message, texture, "whatsapp");
			break;
		case RuntimePlatform.IPhonePlayer:
			if (texture == null) {
				IOSSocialManager.Instance.WhatsAppShareText (message);
			} else {
				IOSSocialManager.Instance.WhatsAppShareImage (texture);
			}
			break;
		}
	}
	
	public static void ShareMedia(string caption, string message) {
		ShareMedia(caption, message, null);
	}
	
	public static void ShareMedia(string caption, string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", message, texture);
			} else {
				AndroidSocialGate.StartShareIntent("Share", message);
			}

			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.Instance.ShareMedia(message, texture);
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			break;
		}
	}



	public static void SendMail(string subject, string body, string recipients) {
		SendMail(subject, body, recipients, null);
	}
	
	public static void SendMail(string subject, string body, string recipients, Texture2D texture) {
		
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidSocialGate.SendMail("Send Mail", body, subject, recipients, texture);
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.Instance.SendMail(subject, body, recipients, texture);
			break;
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		case RuntimePlatform.WP8Player:
		#else
		case RuntimePlatform.WSAPlayerARM:
		case RuntimePlatform.WSAPlayerX64:
		case RuntimePlatform.WSAPlayerX86:
		#endif
			break;
		}
		
	}


}
