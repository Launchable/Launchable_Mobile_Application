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
using System.Collections.Generic;

public class GC_Leaderboard {

	//Editor use only
	public bool IsOpen = true;

	[SerializeField]
	private string _name = string.Empty;

	[SerializeField]
	private string _id = string.Empty;

	[SerializeField]
	private string _displayText = string.Empty;

	[SerializeField]
	private string _scoreFormat = string.Empty;

	[SerializeField]
	private string _imageUrl = string.Empty;


	[SerializeField]
	private Texture2D _Texture;

	private bool _CurrentPlayerScoreLoaded = false;

	private List<GC_Score> _CurrentPlayerScores = new List<GC_Score>();
	private Dictionary<int, GC_Score> _AllTimeCollection = new Dictionary<int, GC_Score>();
	private Dictionary<int, GC_Score> _WeekCollection = new Dictionary<int, GC_Score>();
	private Dictionary<int, GC_Score> _TodayCollection = new Dictionary<int, GC_Score>();

	public GC_Leaderboard() {
		_name =  "New Leaderboard";
	}

	#if AMAZON_CIRCLE_ENABLED
	public GC_Leaderboard(AGSLeaderboard lb) {
		_id 			= lb.id;
		_name 			= lb.name;
		_displayText 	= lb.displayText;
		_scoreFormat 	= lb.scoreFormat;
		_imageUrl 		= lb.imageUrl;
	}
	#endif

	public List<GC_Score> GetScoresList(GC_ScoreTimeSpan timeSpan) {
		List<GC_Score> scores = new List<GC_Score>();

		switch (timeSpan) {
		case GC_ScoreTimeSpan.ALL_TIME:
			scores.AddRange(_AllTimeCollection.Values);
			break;
		case GC_ScoreTimeSpan.WEEK:
			scores.AddRange(_WeekCollection.Values);
			break;
		case GC_ScoreTimeSpan.TODAY:
			scores.AddRange(_TodayCollection.Values);
			break;
		default: break;
		}

		return scores;
	}

	public GC_Score GetScoreByPlayerId(string id, GC_ScoreTimeSpan timeSpan) {
		foreach (GC_Score score in GetScoresList(timeSpan)) {
			if (score.PlayerId.Equals(id)) {
				return score;
			}
		}
		return null;
	}

	public GC_Score GetScore(int rank, GC_ScoreTimeSpan timeSpan) {
		switch (timeSpan) {
		case GC_ScoreTimeSpan.ALL_TIME:
			if (_AllTimeCollection.ContainsKey(rank)) {
				return _AllTimeCollection[rank];
			}
			break;
		case GC_ScoreTimeSpan.WEEK:
			if (_WeekCollection.ContainsKey(rank)) {
				return _WeekCollection[rank];
			}
			break;
		case GC_ScoreTimeSpan.TODAY:
			if (_TodayCollection.ContainsKey(rank)) {
				return _TodayCollection[rank];
			}
			break;
		default: break;
		}

		return null;
	}

	public GC_Score GetCurrentPlayerScore(GC_ScoreTimeSpan timeSpan) {
		foreach (GC_Score score in _CurrentPlayerScores) {
			if (score.TimeSpan == timeSpan) {
				return score;
			}
		}
		return null;
	}

	public void UpdateCurrentPlayerScore(GC_Score newScore) {
		GC_Score score = GetCurrentPlayerScore(newScore.TimeSpan);
		if (score != null) {
			_CurrentPlayerScores.Remove(score);
		}
		_CurrentPlayerScores.Add(newScore);
		_CurrentPlayerScoreLoaded = true;

		Debug.Log(string.Format("[Current Player Score Updated] {0}|{1}|{2}", newScore.PlayerId, newScore.Rank, newScore.Score));
	}

	public void UpdateScore(GC_Score newScore) {
		Dictionary<int, GC_Score> collection;
		switch (newScore.TimeSpan) {
		case GC_ScoreTimeSpan.ALL_TIME:
			collection = _AllTimeCollection;
			break;
		case GC_ScoreTimeSpan.WEEK:
			collection = _WeekCollection;
			break;
		case GC_ScoreTimeSpan.TODAY:
			collection = _TodayCollection;
			break;
		default:
			collection = _AllTimeCollection;
			break;
		}

		if (collection.ContainsKey(newScore.Rank)) {
			collection[newScore.Rank] = newScore;
		} else {
			collection.Add(newScore.Rank, newScore);
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

	public string Title {
		get {
			return _name;
		}

		set {
			_name = value;
		}
	}

	public string Description {
		get {
			return _displayText;
		}

		set {
			_displayText = value;
		}
	}

	public string ScoreFormat {
		get {
			return _scoreFormat;
		}
	}

	public string ImageUrl {
		get {
			return _imageUrl;
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

	public bool CurrentPlayerScoreLoaded {
		get {
			return _CurrentPlayerScoreLoaded;
		}
	}
}
