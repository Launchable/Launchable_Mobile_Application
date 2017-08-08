////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections.Generic;
using SA.Analytics;

namespace SA.Analytics.Google {

	public class UseExample : MonoBehaviour {

		void Start () {
			//This call will be ignored of you already have GoogleAnalytics gameobject on your scene, like in the example scene
			//However if you do not want to create additional object in your initial scene
			//you may use this code to initialize analytics
			//WARNING: if you do not have GoogleAnalytics gamobect and you skip StartTracking call, GoogleAnalytics gameobect will be
			//initialized on first GoogleAnalytics.Client call
			Manager.StartTracking();

			Firebase.Analytics.Init ();
		}
		

		void OnGUI () {
			if(GUI.Button(new Rect(10, 10, 150, 50), "Page Hit")) {
				Manager.Client.SendPageHit("mydemo.com ", "/home", "homepage");
			}
			
			
			if(GUI.Button(new Rect(10, 70, 150, 50), "Event Hit")) {
				Manager.Client.SendEventHit("video", "play", "holiday", 300);
			}

			
			if(GUI.Button(new Rect(10, 130, 150, 50), "Transaction Hit")) {
				Manager.Client.SendTransactionHit("12345", "westernWear", "EUR", 50.00f, 32.00f, 12.00f);
			}

			if(GUI.Button(new Rect(10, 190, 150, 50), "Item Hit")) {
				Manager.Client.SendItemHit("12345", "sofa", "u3eqds43", 300f, 2, "furniture", "EUR");
			}

			if(GUI.Button(new Rect(190, 10, 150, 50), "Social Hit")) {
				Manager.Client.SendSocialHit("like", "facebook", "/home ");
			}

			if(GUI.Button(new Rect(190, 70, 150, 50), "Exception Hit")) {
				Manager.Client.SendExceptionHit("IOException", true);
			}

			if(GUI.Button(new Rect(190, 130, 150, 50), "Timing Hit")) {
				Manager.Client.SendUserTimingHit("jsonLoader", "load", 5000, "jQuery");
			}

			if(GUI.Button(new Rect(190, 190, 150, 50), "Screen Hit")) {
				Manager.Client.SendScreenHit("MainMenu");
			}
		
			if(GUI.Button(new Rect(10, 270, 150, 50), "Firebase Event")) {
				Dictionary<string, object> data = new Dictionary<string, object> ();
				data.Add ("str_data", "hello_from_firebase");
				data.Add ("numeric_data", 10101);
				Firebase.Analytics.LogEvent ("ga_event", data);
			}
		}

		public void CustomBuildersExamples() {

			//Page Tracking
			Manager.Client.CreateHit(HitType.PAGEVIEW);
			Manager.Client.SetDocumentHostName("mydemo.com");
			Manager.Client.SetDocumentPath("/home");
			Manager.Client.SetDocumentTitle("homepage");

			Manager.Client.Send();


			//Event Tracking
			Manager.Client.CreateHit(HitType.EVENT);
			Manager.Client.SetEventCategory("video");
			Manager.Client.SetEventAction("play");
			Manager.Client.SetEventLabel("holiday");
			Manager.Client.SetEventValue(300);

			Manager.Client.Send();



			//Measuring Purchases
			Manager.Client.CreateHit(HitType.PAGEVIEW);
			Manager.Client.SetDocumentHostName("mydemo.com");
			Manager.Client.SetDocumentPath("/receipt");
			Manager.Client.SetDocumentTitle("Receipt Page");

			Manager.Client.SetTransactionID("T12345");
			Manager.Client.SetTransactionAffiliation("Google Store - Online");
			Manager.Client.SetTransactionRevenue(37.39f);
			Manager.Client.SetTransactionTax(2.85f);
			Manager.Client.SetTransactionShipping(5.34f);
			Manager.Client.SetTransactionCouponCode("SUMMER2013");

			Manager.Client.SetProductAction("purchase");
			Manager.Client.SetProductSKU(1, "P12345");
			Manager.Client.SetSetProductName(1, "Android Warhol T-Shirt");
			Manager.Client.SetProductCategory(1, "Apparel");
			Manager.Client.SetProductBrand(1, "Google");
			Manager.Client.SetProductVariant(1, "Black");
			Manager.Client.SetProductPosition(1, 1);

			Manager.Client.Send();



		

			//Measuring Refunds

			// Refund an entire transaction and send with a non-interaction event.
			Manager.Client.CreateHit(HitType.EVENT);
			Manager.Client.SetEventCategory("Ecommerce");
			Manager.Client.SetEventAction("Refund");
			Manager.Client.SetNonInteractionFlag();
			Manager.Client.SetTransactionID("T12345");
			Manager.Client.SetProductAction("refund");

			Manager.Client.Send();


			// Refund a single product.
			Manager.Client.CreateHit(HitType.EVENT);
			Manager.Client.SetEventCategory("Ecommerce");
			Manager.Client.SetEventAction("Refund");
			Manager.Client.SetNonInteractionFlag();
			Manager.Client.SetTransactionID("T12345");
			Manager.Client.SetProductAction("refund");
			Manager.Client.SetProductSKU(1, "P12345");
			Manager.Client.SetProductQuantity(1, 1);

			Manager.Client.Send();




			// Measuring Checkout Steps
			Manager.Client.CreateHit(HitType.PAGEVIEW);
			Manager.Client.SetDocumentHostName("mydemo.com");
			Manager.Client.SetDocumentPath("/receipt");
			Manager.Client.SetDocumentTitle("Receipt Page");
			
			Manager.Client.SetTransactionID("T12345");
			Manager.Client.SetTransactionAffiliation("Google Store - Online");
			Manager.Client.SetTransactionRevenue(37.39f);
			Manager.Client.SetTransactionTax(2.85f);
			Manager.Client.SetTransactionShipping(5.34f);
			Manager.Client.SetTransactionCouponCode("SUMMER2013");
			
			Manager.Client.SetProductAction("purchase");
			Manager.Client.SetProductSKU(1, "P12345");
			Manager.Client.SetSetProductName(1, "Android Warhol T-Shirt");
			Manager.Client.SetProductCategory(1, "Apparel");
			Manager.Client.SetProductBrand(1, "Google");
			Manager.Client.SetProductVariant(1, "Black");
			Manager.Client.SetProductPrice(1, 29.90f);
			Manager.Client.SetProductQuantity(1, 1);
			Manager.Client.SetCheckoutStep(1);
			Manager.Client.SetCheckoutStepOption("Visa");

			Manager.Client.Send();


			//Measuring Checkout Options
			Manager.Client.CreateHit(HitType.EVENT);
			Manager.Client.SetEventCategory("Checkout");
			Manager.Client.SetEventAction("Option");
			Manager.Client.SetProductAction("checkout_option");
			Manager.Client.SetCheckoutStep(2);
			Manager.Client.SetCheckoutStepOption("FedEx");

			Manager.Client.Send();



			//Measuring Internal Promotions

			//Promotion Impressions
			Manager.Client.CreateHit(HitType.PAGEVIEW);
			Manager.Client.SetDocumentHostName("mydemo.com");
			Manager.Client.SetDocumentPath("/home");
			Manager.Client.SetDocumentTitle("homepage");
			Manager.Client.SetPromotionID(1, "PROMO_1234");
			Manager.Client.SetPromotionName(1,"Summer Sale");
			Manager.Client.SetPromotionCreative(1, "summer_banner2");
			Manager.Client.SetPromotionPosition(1, "banner_slot1");
			
			Manager.Client.Send();


			//Promotion Clicks
			Manager.Client.CreateHit(HitType.EVENT);
			Manager.Client.SetEventCategory("Internal Promotions");
			Manager.Client.SetEventAction("click");
			Manager.Client.SetEventLabel("Summer Sale");
			Manager.Client.SetPromotionAction("click");
			Manager.Client.SetPromotionID(1, "PROMO_1234");
			Manager.Client.SetPromotionName(1,"Summer Sale");
			Manager.Client.SetPromotionCreative(1, "summer_banner2");
			Manager.Client.SetPromotionPosition(1, "banner_slot1");

			
			Manager.Client.Send();
			

			//Ecommerce Tracking



			//Transaction Hit
			Manager.Client.CreateHit(HitType.TRANSACTION);
			Manager.Client.SetTransactionID("12345");
			Manager.Client.SetTransactionAffiliation("westernWear");
			Manager.Client.SetTransactionRevenue(50);
			Manager.Client.SetTransactionShipping(32f);
			Manager.Client.SetTransactionTax(12f);
			Manager.Client.SetCurrencyCode("EUR");

			Manager.Client.Send();



			//Item Hit
			Manager.Client.CreateHit(HitType.ITEM);
			Manager.Client.SetTransactionID("12345");
			Manager.Client.SetItemName("sofa");
			Manager.Client.SetItemPrice(300f);
			Manager.Client.SetItemQuantity(2);
			Manager.Client.SetItemCode("u3eqds43");
			Manager.Client.SetItemCategory("furniture");
			Manager.Client.SetCurrencyCode("EUR");

			Manager.Client.Send();

				
			//Social Interactions
			Manager.Client.CreateHit(HitType.SOCIAL);     
			Manager.Client.SetSocialAction("like");     				
			Manager.Client.SetSocialNetwork("facebook"); 
			Manager.Client.SetSocialActionTarget("/home  ");

			Manager.Client.Send();


			//Exception Tracking
			Manager.Client.CreateHit(HitType.EXCEPTION);  
			Manager.Client.SetExceptionDescription("IOException");
			Manager.Client.SetIsFatalException(true);

			Manager.Client.Send();



			//User Timing Tracking
			Manager.Client.CreateHit(HitType.TIMING); 
			Manager.Client.SetUserTimingCategory("jsonLoader");
			Manager.Client.SetUserTimingVariableName("load");
			Manager.Client.SetUserTimingTime(5000);
			Manager.Client.SetUserTimingLabel("jQuery");

			Manager.Client.SetDNSTime(100);
			Manager.Client.SetPageDownloadTime(20);
			Manager.Client.SetRedirectResponseTime(32);
			Manager.Client.SetTCPConnectTime(56);
			Manager.Client.SetServerResponseTime(12);

			Manager.Client.Send();






			//Custom builder example

			//Simple Page hit
			Manager.Client.CreateHit(HitType.PAGEVIEW);
			Manager.Client.SetDocumentHostName("mydemo.com");
			Manager.Client.SetDocumentPath("/home");
			Manager.Client.SetDocumentTitle("homepage");
			
			Manager.Client.Send();
			
			//Constructing Same page hit without plugin methods
			Manager.Client.CreateHit(HitType.PAGEVIEW);
			Manager.Client.AppendData("dh", "mydemo.com");
			Manager.Client.AppendData("dp", "/home");
			Manager.Client.AppendData("dt", "homepage");

			Manager.Client.Send();

		


		}
	}

}
