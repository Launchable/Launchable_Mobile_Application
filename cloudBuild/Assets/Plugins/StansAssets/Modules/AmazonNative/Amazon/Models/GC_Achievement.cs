//#define AMAZON_CIRCLE_ENABLED

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

public class GC_Achievement {


	//Editor use only
	public bool IsOpen = true;

	[SerializeField]
	private string _title = string.Empty;

	[SerializeField]
	private string _id = string.Empty;

	[SerializeField]
	private string _description = string.Empty;

	[SerializeField]
	private float _progress = 0f;

	[SerializeField]
	private int _pointValue = 0;

	[SerializeField]
	private bool _isHidden = false;

	[SerializeField]
	private bool _isUnlocked = false;

	[SerializeField]
	private int _position = 0;


	private DateTime _dateUnlocked = DateTime.Now;

	[SerializeField]
	private Texture2D _Texture;



	public GC_Achievement() {
		_title =  "New Achievement";
	}
	#if AMAZON_CIRCLE_ENABLED

	public GC_Achievement(AGSAchievement achieve) {
		_title 			= achieve.title;
		_id 			= achieve.id;
		_description 	= achieve.description;
		_progress 		= achieve.progress;
		_pointValue 	= achieve.pointValue;
		_isHidden 		= achieve.isHidden;
		_isUnlocked 	= achieve.isUnlocked;
		_position 		= achieve.position;
		_dateUnlocked 	= achieve.dateUnlocked;
	}

	#endif
	

	public string Title {
		get {
			return _title;
		}

		set {
			_title = value;
		}
	}

	public string Identifier {
		get {
			return _id;
		}

		set {
			_id = value;
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

	public float Progress {
		get {
			return _progress;
		}
	}

	public int PointValue {
		get {
			return _pointValue;
		}
	}

	public bool IsHidden {
		get {
			return _isHidden;
		}
	}

	public bool IsUnlocked {
		get {
			return _isUnlocked;
		}
	}

	public int Position {
		get {
			return _position;
		}
	}

	public DateTime DateUnlocked {
		get {
			return _dateUnlocked;
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
}
