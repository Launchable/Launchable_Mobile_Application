using UnityEngine;
using System.Collections;
using System;

public class UM_Player  {



	private GK_Player _GK_Player;
	private GooglePlayerTemplate _GP_Player;
	private GC_Player _GC_Player;

	public event Action<Texture2D> BigPhotoLoaded =  delegate {};
	public event Action<Texture2D> SmallPhotoLoaded =  delegate {};

	
	//--------------------------------------
	// Init
	//--------------------------------------

	public UM_Player(GK_Player gk, GooglePlayerTemplate gp, GC_Player gc) {
		_GK_Player = gk;
		_GP_Player = gp;
		_GC_Player = gc;

		if(_GK_Player != null) {
			_GK_Player.OnPlayerPhotoLoaded += HandleOnPlayerPhotoLoaded;
		}

		if(_GP_Player != null) {
			_GP_Player.BigPhotoLoaded += HandleBigPhotoLoaded;
			_GP_Player.SmallPhotoLoaded += HandleSmallPhotoLoaded;
		}

		if (_GC_Player != null) {
			_GC_Player.AvatarLoaded += AmazonPlayerAvatarLoaded;
		}
	}

	public void LoadBigPhoto() {
		switch(Application.platform) {

		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Android) {
				_GP_Player.LoadImage();
			}
			break;
		case RuntimePlatform.IPhonePlayer:
			_GK_Player.LoadPhoto(GK_PhotoSize.GKPhotoSizeNormal);
			break;
		}
	}


	public void LoadSmallPhoto() {
		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				_GC_Player.LoadAvatar();
			} else { 
				_GP_Player.LoadIcon();
			}
			break;
		case RuntimePlatform.IPhonePlayer:
			_GK_Player.LoadPhoto(GK_PhotoSize.GKPhotoSizeSmall);
			break;
			
		}
	}
	
	//--------------------------------------
	// Get / Set
	//--------------------------------------



	public string PlayerId {
		get {
			switch(Application.platform) {

			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
					return _GC_Player.PlayerId;
				} else {
					return _GP_Player.playerId;
				}
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.Id;

			}

			return string.Empty;
		}
	}


	public string Name {
		get {
			switch(Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
					return _GC_Player.Name;
				} else {
					return _GP_Player.name;
				}
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.Alias;
			}

			return string.Empty;
		}
	}

	[System.Obsolete("Avatar is deprectaed, please use SmallPhoto instead")]
	public Texture2D Avatar {
		get {
			switch(Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
					return _GC_Player.Avatar;
				} else {
					return _GP_Player.image;	
				}
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.SmallPhoto;
			}

			return null;
		}
	}

	public Texture2D SmallPhoto {
		get {
			switch(Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
					return _GC_Player.Avatar;
				} else {
					return _GP_Player.icon;
				}
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.SmallPhoto;
			}
			
			return null;
		}
	}

	public Texture2D BigPhoto {
		get {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
					return new Texture2D(1, 1);
				} else {
					return _GP_Player.image;	
				}
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.BigPhoto;
			}
			
			return null;
		}
	}


	public GK_Player GameCenterPlayer {
		get {
			return _GK_Player;
		}
	}
	
	
	
	public GooglePlayerTemplate GooglePlayPlayer {
		get {
			return _GP_Player;
		}
	}

	public GC_Player GameCirclePlayer {
		get {
			return _GC_Player;
		}
	}

	//--------------------------------------
	// Android Events
	//--------------------------------------

	void HandleSmallPhotoLoaded (Texture2D tex) {
		SmallPhotoLoaded(tex);
	}
	
	void HandleBigPhotoLoaded (Texture2D tex) {
		BigPhotoLoaded(tex);
	}

	//--------------------------------------
	// IOS Events
	//--------------------------------------

	void HandleOnPlayerPhotoLoaded (GK_UserPhotoLoadResult res) {
		if(res.IsSucceeded)  {
			if(res.Size == GK_PhotoSize.GKPhotoSizeSmall) {
				SmallPhotoLoaded(res.Photo);
			} else {
				BigPhotoLoaded(res.Photo);
			}
		}
	}

	//--------------------------------------
	// Amazon Events
	//--------------------------------------

	private void AmazonPlayerAvatarLoaded (Texture2D avatar)
	{
		_GC_Player.AvatarLoaded -= AmazonPlayerAvatarLoaded;

		SmallPhotoLoaded(avatar);
	}

} 
