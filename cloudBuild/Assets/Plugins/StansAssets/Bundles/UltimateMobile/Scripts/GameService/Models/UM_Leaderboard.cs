using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class UM_Leaderboard  {
	
	[SerializeField]
	public string id = "new leaderboard";
	public bool IsOpen = true;
	
	[SerializeField]
	public string IOSId = string.Empty;
	[SerializeField]
	public string AndroidId = string.Empty;
	[SerializeField]
	public string AmazonId = string.Empty;

	[SerializeField]
	private string _Description = string.Empty;
	
	[SerializeField]
	private Texture2D _Texture;
	
	private GK_Leaderboard gk_Leaderboard;
	private GPLeaderBoard gp_Leaderboard;
	private GC_Leaderboard gc_Leaderboard;
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
	public void Setup(GPLeaderBoard gpLeaderboard) {
		gp_Leaderboard = gpLeaderboard;
	}
	
	public void Setup(GK_Leaderboard gkLeaderboard) {
		gk_Leaderboard = gkLeaderboard;
	}

	public void Setup(GC_Leaderboard gcLeaderboard) {
		gc_Leaderboard = gcLeaderboard;
	}
	
	public bool IsValid {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				if(UltimateMobileSettings.Instance.PlatformEngine.Equals(UM_PlatformDependencies.Android))
					return gp_Leaderboard != null;
				else
					return gc_Leaderboard != null;
			case RuntimePlatform.IPhonePlayer:
				return gk_Leaderboard != null;
			}
			return true;
		}
	}
	
	public UM_Score GetScore(int rank, UM_TimeSpan scope, UM_CollectionType collection) {
		UM_Score umScore = null;
		
		if (IsValid) {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android)) {
					GPScore gp = gp_Leaderboard.GetScore (rank, scope.Get_GP_TimeSpan (), collection.Get_GP_CollectionType ());
					if (gp != null) {
						umScore = new UM_Score (null, gp, null);
					}
				} else {
					GC_Score gc = gc_Leaderboard.GetScore(rank, scope.Get_GC_TimeSpan());
					if (gc != null) {
						umScore = new UM_Score(null, null, gc);
					}
				}
				break;
			case RuntimePlatform.IPhonePlayer:
				GK_Score gk = gk_Leaderboard.GetScore(rank, scope.Get_GK_TimeSpan(), collection.Get_GK_CollectionType());
				if (gk != null) {
					umScore = new UM_Score(gk, null, null);
				}
				break;
			}
		}
		
		return umScore;
	}
	
	public List<UM_Score> GetScoresList(UM_TimeSpan span, UM_CollectionType collection) {
		List<UM_Score> scores = new List<UM_Score>();
		
		if (IsValid) {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android)) {
					List<GPScore> gp = gp_Leaderboard.GetScoresList (span.Get_GP_TimeSpan (), collection.Get_GP_CollectionType ());
					foreach (GPScore score in gp) {
						scores.Add (new UM_Score (null, score, null));
					}
					return scores;
				} else {
					List<GC_Score> gcScores = gc_Leaderboard.GetScoresList(span.Get_GC_TimeSpan());
					foreach (GC_Score s in gcScores) {
						scores.Add(new UM_Score(null, null, s));
					}
					return scores;
				}
			case RuntimePlatform.IPhonePlayer:
				List<GK_Score> gk = gk_Leaderboard.GetScoresList(span.Get_GK_TimeSpan(), collection.Get_GK_CollectionType());
				foreach (GK_Score score in gk) {
					scores.Add(new UM_Score(score, null, null));
				}
				return scores;
			}
		}
		
		return scores;
	}
	
	public UM_Score GetScoreByPlayerId(string playerId, UM_TimeSpan span, UM_CollectionType collection){
		UM_Score umScore = null;
		if (IsValid) {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android)) {
					GPScore gp = gp_Leaderboard.GetScoreByPlayerId (playerId, span.Get_GP_TimeSpan (), collection.Get_GP_CollectionType ());
					if (gp != null) {
						umScore = new UM_Score (null, gp, null);
					}
				} else {
					GC_Score gc = gc_Leaderboard.GetScoreByPlayerId(playerId, span.Get_GC_TimeSpan());
					if (gc != null) {
						umScore = new UM_Score(null, null, gc);
					}
				}
				break;
			case RuntimePlatform.IPhonePlayer:
				GK_Score gk = gk_Leaderboard.GetScoreByPlayerId(playerId, span.Get_GK_TimeSpan(), collection.Get_GK_CollectionType());
				if (gk != null) {
					umScore = new UM_Score(gk, null, null);
				}
				break;
			}
		}
		return umScore;
	}
	
	public UM_Score GetCurrentPlayerScore(UM_TimeSpan span, UM_CollectionType collection) {
		UM_Score umScore = null;
		if (IsValid) {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android)) {
					GPScore gp = gp_Leaderboard.GetCurrentPlayerScore (span.Get_GP_TimeSpan (), collection.Get_GP_CollectionType ());
					if (gp != null) {
						umScore = new UM_Score (null, gp, null);
					}
				} else {
					GC_Score gc = gc_Leaderboard.GetCurrentPlayerScore(span.Get_GC_TimeSpan());
					if(gc != null) {
						umScore = new UM_Score(null, null, gc);
					}
				}
				break;
			case RuntimePlatform.IPhonePlayer:
				GK_Score gk = gk_Leaderboard.GetCurrentPlayerScore(span.Get_GK_TimeSpan(), collection.Get_GK_CollectionType());
				if (gk != null) {
					umScore = new UM_Score(gk, null, null);
				}
				break;
			}
		}
		
		return umScore;
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public string Id {
		get {
			if (IsValid) {
				switch (Application.platform) {
				case RuntimePlatform.Android:
					if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android))
						return gp_Leaderboard.Id;
					else
						return gc_Leaderboard.Identifier;
				case RuntimePlatform.IPhonePlayer:
					return gk_Leaderboard.Id;
				}
			}
			return string.Empty;
		}
	}
	
	public string Name {
		get {
			if (IsValid) {
				switch (Application.platform) {
				case RuntimePlatform.Android:
					if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android))
						return gp_Leaderboard.Name;
					else
						return gc_Leaderboard.Title;
				case RuntimePlatform.IPhonePlayer:
					return gk_Leaderboard.Info.Title;
				}
			}
			return string.Empty;
		}
	}

	public bool CurrentPlayerScoreLoaded {
		get {
			if (IsValid) {
				switch (Application.platform) {
				case RuntimePlatform.Android:
					if (UltimateMobileSettings.Instance.PlatformEngine.Equals (UM_PlatformDependencies.Android)) {
						return gp_Leaderboard.CurrentPlayerScoreLoaded;
					} else {
						return gc_Leaderboard.CurrentPlayerScoreLoaded;
					}
				case RuntimePlatform.IPhonePlayer:
					return gk_Leaderboard.CurrentPlayerScoreLoaded;
				}
			}
			return false;
		}
	}
	
	public GK_Leaderboard GameCenterLeaderboard {
		get {
			return gk_Leaderboard;
		}
	}
	
	public GPLeaderBoard GooglePlayLeaderboard {
		get {
			return gp_Leaderboard;
		}
	}

	public GC_Leaderboard AmazonLeaderboard {
		get {
			return gc_Leaderboard;
		}
	}
	
	public string Description {
		get {
			return _Description;
		}
		set {
			_Description = value;
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
