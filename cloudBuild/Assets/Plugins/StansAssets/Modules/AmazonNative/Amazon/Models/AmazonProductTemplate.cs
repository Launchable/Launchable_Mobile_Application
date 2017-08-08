//#define AMAZON_BILLING_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using System.Text;

#if AMAZON_BILLING_ENABLED
using com.amazon.device.iap.cpt;
#endif

[System.Serializable]
public class AmazonProductTemplate {
	
	//Editor Only
	public bool IsOpen = true;
	
	[SerializeField]
	private string _LocalizedPrice = "0.99 $";
	
	[SerializeField]
	private long   _PriceAmountMicros = 990000;
	
	[SerializeField]
	private string _PriceCurrencyCode = "USD";
	
	[SerializeField]
	private string _sku = string.Empty;
	[SerializeField]
	private AMN_InAppType _productType = AMN_InAppType.CONSUMABLE;
	[SerializeField]
	private string _price = string.Empty;
	[SerializeField]
	private string _title = string.Empty;
	[SerializeField]
	private string _description = string.Empty;
	[SerializeField]
	private string _smallIconUrl = string.Empty;
	[SerializeField]
	private Texture2D _Texture;
	
	#if AMAZON_BILLING_ENABLED
	
	public AmazonProductTemplate(ProductData item) {
		_sku 			= item.Sku;
		_productType 	= (AMN_InAppType)Enum.Parse(typeof(AMN_InAppType), item.ProductType);
		_LocalizedPrice = item.Price;
		_title 					= item.Title;
		_description 	= item.Description;
		_smallIconUrl 	= item.SmallIconUrl;
		SetPriceFromLocalizedPrice (_LocalizedPrice);
	}
	#endif

	private static bool isFloatChar(char c)	{
		return ((c >= '0' && c <= '9') || c == '.');		
	}

	private void SetPriceFromLocalizedPrice() {
			StringBuilder currency = new StringBuilder ();
			StringBuilder val = new StringBuilder ();

			foreach(char c in LocalizedPrice.ToCharArray()) {
				if (isFloatChar (c)) {
					val.Append (c);
				} else {
					currency.Append (c);
				} 
			}

			float actualVal;
			if(float.TryParse(val.ToString(), out actualVal)) {
				_price = actualVal.ToString();
				_PriceCurrencyCode = PriceCurrencyCode.ToString().Trim();
			}
	}



	public AmazonProductTemplate() {		
	}
	
	public string Sku {
		get {
			return _sku;
		}
		set {
			_sku = value;
		}
	}
	
	public AMN_InAppType ProductType {
		get {
			return _productType;
		}
		set {
			_productType = value;
		}
	}
	
	public string Price {
		get {
			return _price;
		}
	}
	
	public string Title {
		get {
			return _title;
		}
		set {
			_title = value;
		}
	}
	
	public string Description {
		get {
			return _description;
		}
		set {
			_description = value;
		}
	}
	
	public string SmallIconUrl {
		get {
			return _smallIconUrl;
		}
	}
	
	public Texture2D Texture {
		get {
			return _Texture;
		}
		set {
			_Texture = value;
		}
	}
	
	public string PriceCurrencyCode  {
		get {
			return _PriceCurrencyCode;
		}
		
		set {
			_PriceCurrencyCode = value;
		}
	}
	
	public string LocalizedPrice {
		get {
			return _LocalizedPrice;
		}
		
		set {
			_LocalizedPrice = value;
		}
	}

	public long PriceAmountMicros {
		get {
			return _PriceAmountMicros;
		}
		set {
			_PriceAmountMicros = value;
		}
	}
}
