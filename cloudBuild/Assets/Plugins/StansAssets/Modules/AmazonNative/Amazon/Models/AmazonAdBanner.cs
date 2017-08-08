using UnityEngine;
using System;
using System.Collections;

public class AmazonAdBanner {

	public enum BannerAligns { Top = 0, TopLeft = 1, TopRight = 2,
		Bottom = 3, BottomLeft = 4, BottomRight = 5};

	private int _id;
	private BannerAligns _position;

	private AMN_AdProperties _properties;

	private bool _isLoaded = false;
	private bool _isOnScreen = false;
	private int _width 	= 0;
	private int _height = 0;

	public event Action<AmazonAdBanner> OnLoadedAction 				= delegate {};
	public event Action<AmazonAdBanner> OnFailedLoadingAction 		= delegate {};
	public event Action<AmazonAdBanner> OnExpandedAction 			= delegate {};
	public event Action<AmazonAdBanner> OnDismissedAction 			= delegate {};
	public event Action<AmazonAdBanner> OnCollapsedAction 			= delegate {};

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public AmazonAdBanner(BannerAligns position, int id) {
		_id = id;
		_position = position;

		AMN_AdvertisingProxy.CreateBanner(GetPosition(_position), _id);
	}

	public void SetProperties(int width, int height, AMN_AdProperties props) {
		_width = width;
		_height = height;
		_properties = props;
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void Hide(bool hide) {
		AMN_AdvertisingProxy.HideBanner(hide, _id);
	}

	public void Destroy() {
		AMN_AdvertisingProxy.DestroyBanner(_id);
	}

	public void Refresh() {
		AMN_AdvertisingProxy.RefreshBanner(_id);
	}


	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public int Id {
		get {
			return _id;
		}
	}

	public bool IsLoaded {
		get {
			return _isLoaded;
		}
	}

	public bool IsOnScreen {
		get {
			return _isOnScreen;
		}
	}

	public int Width {
		get {
			return _width;
		}
	}

	public int Height {
		get {
			return _height;
		}
	}

	public AMN_AdProperties Properties {
		get {
			return _properties;
		}
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------



	public void HandleOnBannerAdLoaded()  {
		_isLoaded = true;
		OnLoadedAction(this);
	}

	public void HandleOnBannerAdFailedToLoad() {
		OnFailedLoadingAction(this);
	}

	public void HandleOnBannerAdExpanded() {
		_isOnScreen = true;
		OnExpandedAction(this);
	}

	public void HandleOnBannerAdDismissed() {
		_isOnScreen = false;
		OnDismissedAction(this);
	}

	public void HandleOnBannerAdCollapsed() {
		_isOnScreen = false;
		OnCollapsedAction(this);
	}


	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------

	private string GetPosition(BannerAligns BannerAlign) {
		string position = "BM";

		switch(BannerAlign) {
		case BannerAligns.Top:
			position = "TM";
			break;
		case BannerAligns.TopLeft:
			position = "TL";
			break;
		case BannerAligns.TopRight:
			position = "TR";
			break;
		case BannerAligns.Bottom:
			position = "BM";
			break;
		case BannerAligns.BottomLeft:
			position = "BL";
			break;
		case BannerAligns.BottomRight:
			position = "BR";
			break;
		}

		return position;
	}

	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
