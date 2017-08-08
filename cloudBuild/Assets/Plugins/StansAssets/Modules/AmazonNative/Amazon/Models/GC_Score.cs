using UnityEngine;
using System.Collections;

public class GC_Score {

	private string _playerId = string.Empty;
	private string _leaderboardId = string.Empty;

	private int _rank = 0;
	private long _score = 0;
	private GC_ScoreTimeSpan _timeSpan = GC_ScoreTimeSpan.ALL_TIME;

	public GC_Score(string playerId, string leaderboardId, int rank, long score, GC_ScoreTimeSpan timeSpan) {
		_playerId = playerId;
		_leaderboardId = leaderboardId;
		_rank = rank;
		_score = score;
		_timeSpan = timeSpan;
	}

	public string PlayerId {
		get {
			return _playerId;
		}
	}

	public GC_Player Player {
		get {
			return SA_AmazonGameCircleManager.Instance.GetPlayerById(_playerId);
		}
	}

	public string LeaderboardId {
		get {
			return _leaderboardId;
		}
	}

	public int Rank {
		get {
			return _rank;
		}
	}

	public long Score {
		get {
			return _score;
		}
	}

	public float CurrencyScore {
		get {
			return _score / 100.0f;
		}
	}

	public System.TimeSpan TimeScore {
		get {
			return System.TimeSpan.FromMilliseconds(_score);
		}
	}

	public GC_ScoreTimeSpan TimeSpan {
		get {
			return _timeSpan;
		}
	}
}
