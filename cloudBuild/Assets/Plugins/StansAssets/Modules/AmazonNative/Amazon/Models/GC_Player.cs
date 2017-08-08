//#define AMAZON_CIRCLE_ENABLED

////////////////////////////////////////////////////////////////////////////////
//  
// @module Amazon Native Plugin for Unity3D 
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;

public class GC_Player  {

	private string _playerId 	= string.Empty;
	private string _name 		= string.Empty;
	private string _avatarUrl 	= string.Empty;

	private Texture2D _avatar = null;

	public event Action<Texture2D> AvatarLoaded =  delegate {};

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	#if AMAZON_CIRCLE_ENABLED
	public GC_Player(AGSPlayer player) {
		this._name = player.alias;
		this._playerId = player.playerId;
		this._avatarUrl = player.avatarUrl;
	}
	#endif

	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public void LoadAvatar() {
		if(_avatar != null) {
			AvatarLoaded(_avatar);
			return;
		}

		Debug.Log("Amazon Player Avatar Started to Load!");
			
		SA.Common.Models.WWWTextureLoader loader = SA.Common.Models.WWWTextureLoader.Create();
		loader.OnLoad += OnProfileImageLoaded;
		loader.LoadTexture(_avatarUrl);
	}

	//--------------------------------------
	// GET / SET
	//--------------------------------------
		
	public string PlayerId {
		get {
			return _playerId;
		}
	}
	
	public string Name {
		get {
			return _name;
		}
	}
	
	public string AvatarUrl {
		get {
			return _avatarUrl;
		}
	}

	public Texture2D Avatar {
		get {
			return _avatar;
		}
	}

	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------

	private void OnProfileImageLoaded(Texture2D texture) {
		Debug.Log("Amazon Player OnProfileImageLoaded" + texture);

		_avatar = texture;
		AvatarLoaded(_avatar);
	}

}
