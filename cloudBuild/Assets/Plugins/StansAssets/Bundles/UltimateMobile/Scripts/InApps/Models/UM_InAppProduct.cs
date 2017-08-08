using UnityEngine;
using System.Collections;


[System.Serializable]
public class UM_InAppProduct  {

	public bool IsOpen;
	public string id = "new_product";
	public UM_InAppType Type = UM_InAppType.Consumable;
	public SA.IOSNative.StoreKit.PriceTier PriceTier = SA.IOSNative.StoreKit.PriceTier.Tier1;
	public Texture2D Texture = null;
	public string DisplayName = "New Product";
	public string Description = string.Empty;


	public string IOSId = string.Empty;
	public string AndroidId  = string.Empty;
	public string WP8Id  = string.Empty;
	public string AmazonId  = string.Empty;


	private SA.IOSNative.StoreKit.Product		_IOSTemplate 		= new SA.IOSNative.StoreKit.Product();
	private WP8ProductTemplate	   				_WP8Template		= new WP8ProductTemplate();
	private GoogleProductTemplate  				_AndroidTemplate 	= new GoogleProductTemplate();
	private AmazonProductTemplate 				_AmazonTemplate 	= new AmazonProductTemplate();
	private UM_InAppProductTemplate 			_template 			= new UM_InAppProductTemplate();

	private bool _isTemplateSet = false;

	public WP8ProductTemplate WP8Template {
		get {
			return _WP8Template;
		}
	}

	public SA.IOSNative.StoreKit.Product IOSTemplate {
		get {
			return _IOSTemplate;
		}
	}

	public GoogleProductTemplate AndroidTemplate {
		get {
			return _AndroidTemplate;
		}
	}

	public AmazonProductTemplate AmazonTemplate {
		get {
			return _AmazonTemplate;
		}
	}

	public UM_InAppProductTemplate template {
		get {
			return _template;
		}
	}

	public bool IsConsumable {
		get {
			if(Type == UM_InAppType.Consumable) {
				return true;
			}

			return false;
		}
	}


	public void SetTemplate(WP8ProductTemplate tpl) {
		_WP8Template = tpl;
		_template = new UM_InAppProductTemplate();
		_template.id = tpl.ProductId;
		_template.title = tpl.Name;
		_template.description = tpl.Description;
		_template.price = tpl.Price;


		DisplayName = tpl.Name;
		Description = tpl.Description;
		_isTemplateSet = true;
	}

	public void SetTemplate(SA.IOSNative.StoreKit.Product tpl) {
		_IOSTemplate = tpl;
		_template = new UM_InAppProductTemplate();
		_template.id = tpl.Id;
		_template.title = tpl.DisplayName;
		_template.description = tpl.Description;
		_template.price = tpl.Price.ToString();

		DisplayName = tpl.DisplayName;
		Description = tpl.Description;
		_isTemplateSet = true;
	}

	public void SetTemplate(GoogleProductTemplate tpl) {
		_AndroidTemplate = tpl;
		_template = new UM_InAppProductTemplate();
		_template.id = tpl.SKU;
		_template.title = tpl.Title;
		_template.description = tpl.Description;
		_template.price = tpl.Price.ToString();

		DisplayName = tpl.Title;
		Description = tpl.Description;
		_isTemplateSet = true;
	}

	public void SetTemplate(AmazonProductTemplate tpl) {
		_AmazonTemplate = tpl;
		_template = new UM_InAppProductTemplate();
		_template.id = tpl.Sku;
		_template.title = tpl.Title;
		_template.description = tpl.Description;
		_template.price = tpl.Price.ToString();


		DisplayName = tpl.Title;
		Description = tpl.Description;
		_isTemplateSet = true;
	}
		

	public string LocalizedPrice  {
		get {


			if(!_isTemplateSet) {
				return GetPriceByTier().ToString() + " $";
			}

			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				if(UltimateMobileSettings.Instance.PlatformEngine.Equals(UM_PlatformDependencies.Android))
					return  _AndroidTemplate.LocalizedPrice;
				else
					return  _AmazonTemplate.LocalizedPrice;
				
			case RuntimePlatform.IPhonePlayer:
				return _IOSTemplate.LocalizedPrice;

			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			case RuntimePlatform.WP8Player:
			#else
			case RuntimePlatform.WSAPlayerARM:
			case RuntimePlatform.WSAPlayerX64:
			case RuntimePlatform.WSAPlayerX86:
			#endif
				return _WP8Template.Price;
			}

			return GetPriceByTier().ToString() + " $";
		}
	}

	public string CurrencyCode {
		get {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				if(UltimateMobileSettings.Instance.PlatformEngine.Equals(UM_PlatformDependencies.Android))
					return _isTemplateSet ? _AndroidTemplate.PriceCurrencyCode : "USD";
				else
					return _isTemplateSet ? _AmazonTemplate.PriceCurrencyCode : "USD";
			case RuntimePlatform.IPhonePlayer:
				return _isTemplateSet ? _IOSTemplate.CurrencyCode : "USD";
			}
			return string.Empty;
		}
	}


	public long PriceAmountMicros  {

		get {
			if(!_isTemplateSet) {
				return System.Convert.ToInt64(GetPriceByTier() * 1000000f);
			}


			switch(Application.platform) {

			case RuntimePlatform.Android:
				if(UltimateMobileSettings.Instance.PlatformEngine.Equals(UM_PlatformDependencies.Android))
					return  _AndroidTemplate.PriceAmountMicros;
				else
					return  System.Convert.ToInt64(GetPriceByTier() * 1000000f);

			case RuntimePlatform.IPhonePlayer:
				return _IOSTemplate.PriceInMicros;

			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			case RuntimePlatform.WP8Player:
			#else
			case RuntimePlatform.WSAPlayerARM:
			case RuntimePlatform.WSAPlayerX64:
			case RuntimePlatform.WSAPlayerX86:
			#endif
				return System.Convert.ToInt64(GetPriceByTier() * 1000000f);
			}

			return System.Convert.ToInt64(GetPriceByTier() * 1000000f);
		}
			
	}
	
	public string ActualPrice {
		get {
			return ActualPriceValue.ToString();
		}		
	}

	public float ActualPriceValue {
		get {
			return PriceAmountMicros / 1000000f;
		}
	}


	public bool IsPurchased {
		get {
			return UM_InAppPurchaseManager.Client.IsProductPurchased(this);
		}
	}

	public string Title  {
		get {
			switch(Application.platform) {

			case RuntimePlatform.Android:
				if(UltimateMobileSettings.Instance.PlatformEngine.Equals(UM_PlatformDependencies.Android))
					return _AndroidTemplate.Title;
				else 
					return _AmazonTemplate.Title;
			case RuntimePlatform.IPhonePlayer:
				return _IOSTemplate.DisplayName;
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			case RuntimePlatform.WP8Player:
			#else
			case RuntimePlatform.WSAPlayerARM:
			case RuntimePlatform.WSAPlayerX64:
			case RuntimePlatform.WSAPlayerX86:
			#endif
				return _WP8Template.Name;
			}

			return string.Empty;
		}

	}  

	private float GetPriceByTier() {
		int tierint = (int) PriceTier;

		float FloatPrice = tierint + 1f - 0.01f;
		return FloatPrice;
	}

	public override string ToString () {
		return string.Format ("[UM_InAppProduct: template={0}, Title={1}, Description={2}, Price={3}, WP8Template={4}, IOSTemplate={5}, AndroidTemplate={6}, AndroidTemplate={7}]", template, DisplayName, Description, LocalizedPrice, WP8Template, IOSTemplate, AndroidTemplate, AmazonTemplate);
	}
		
}
