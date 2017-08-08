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
using System;

public class SA_AmazonGameCircleManager : AMN_Singleton<SA_AmazonGameCircleManager> {

	public event Action<AMN_InitializeResult> OnInitializeResult = delegate {};
	public event Action<AMN_RequestPlayerDataResult> OnRequestPlayerDataReceived = delegate {};

	public event Action<AMN_RequestAchievementsResult> OnRequestAchievementsReceived = delegate {};
	public event Action<AMN_UpdateAchievementResult> OnUpdateAchievementReceived = delegate {};

	public event Action<AMN_RequestLeaderboardsResult> OnRequestLeaderboardsReceived = delegate {};
	public event Action<AMN_SubmitLeaderboardResult> OnSubmitLeaderboardReceived = delegate {};
	public event Action<AMN_LocalPlayerScoreLoadedResult> OnLocalPlayerScoreLoaded = delegate {};
	public event Action<AMN_ScoresLoadedResult> OnScoresLoaded = delegate {};

	private GC_Player _player = null;	
	private Dictionary<string, GC_Player> _Players = new Dictionary<string, GC_Player>();

	private bool _isInitialized = false;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
		
	void Awake() {
		SubscribeToEvents ();


		DontDestroyOnLoad(gameObject);
		#if AMAZON_CIRCLE_ENABLED
		new GameObject().AddComponent<GameCircleManager>();
		#endif
	}

	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public bool IsInitialized {
		get {
			return _isInitialized;
		}
	}

	public GC_Player Player {
		get {
			return _player;
		}
		set {
			_player = value;
		}
	}

	public Dictionary<string, GC_Player> Players {
		get {
			return _Players;
		}
	}

	public List<GC_Achievement> Achievements {
		get {
			return AmazonNativeSettings.Instance.Achievements;
		}
	}

	public List<GC_Leaderboard> Leaderboards {
		get {
			return AmazonNativeSettings.Instance.Leaderboards;
		}
	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnServiceConnected() {
		AMN_InitializeResult result = new AMN_InitializeResult (true);

		OnInitializeResult (result);
	}
	
	private void OnServiceDisconnected(string error) {	
		AMN_InitializeResult result = new AMN_InitializeResult (error);
		
		OnInitializeResult (result);
	}

	#if AMAZON_CIRCLE_ENABLED

	private void OnPlayerDataLoaded(AGSRequestPlayerResponse response) {
		if (response.IsError ()) {
			Player = null;
			AMN_RequestPlayerDataResult result = new AMN_RequestPlayerDataResult(response.error);

			OnRequestPlayerDataReceived(result);
		} else {
			this._player = new GC_Player(response.player);
			AMN_RequestPlayerDataResult result = new AMN_RequestPlayerDataResult(Player);
			
			OnRequestPlayerDataReceived(result);
		}
	}



	private void OnRequestAchievementsCompleted(AGSRequestAchievementsResponse response) {
		if (response.IsError ()) {
			AMN_RequestAchievementsResult result = new AMN_RequestAchievementsResult(response.error);

			OnRequestAchievementsReceived(result);
		} else {
			Achievements.Clear();
			foreach(AGSAchievement achieve in response.achievements) {
				Achievements.Add(new GC_Achievement(achieve));
			}

			AMN_RequestAchievementsResult result = new AMN_RequestAchievementsResult(Achievements);
			
			OnRequestAchievementsReceived(result);
		}
	}

	private void OnUpdateAchievementCompleted (AGSUpdateAchievementResponse response ) {
		if (response.IsError()) {
			AMN_UpdateAchievementResult result = new AMN_UpdateAchievementResult(response.achievementId, response.error);

			OnUpdateAchievementReceived(result);
		} else {
			AMN_UpdateAchievementResult result = new AMN_UpdateAchievementResult(response.achievementId);
			
			OnUpdateAchievementReceived(result);
		}
	}

	private void OnRequestLeaderboardsCompleted(AGSRequestLeaderboardsResponse response) {
		if (response.IsError ()) {
			AMN_RequestLeaderboardsResult result = new AMN_RequestLeaderboardsResult(response.error);

			OnRequestLeaderboardsReceived(result);
		} else {
			Leaderboards.Clear();
			foreach(AGSLeaderboard lb in response.leaderboards) {
				Leaderboards.Add(new GC_Leaderboard(lb));
			}

			AMN_RequestLeaderboardsResult result = new AMN_RequestLeaderboardsResult(Leaderboards);
			
			OnRequestLeaderboardsReceived(result);
		}
	}

	private void OnSubmitScoreCompleted (AGSSubmitScoreResponse response ) {
		if (response.IsError ()) {
			AMN_SubmitLeaderboardResult result = new AMN_SubmitLeaderboardResult(response.leaderboardId, response.error);

			OnSubmitLeaderboardReceived(result);
		} else {
			AMN_SubmitLeaderboardResult result = new AMN_SubmitLeaderboardResult(response.leaderboardId);
			
			OnSubmitLeaderboardReceived(result);
		}
	}

	private void OnLocalPlayerScoresLoaded(AGSRequestScoreResponse response) {
		AMN_LocalPlayerScoreLoadedResult result = null;
		if (response.IsError()) {
			Debug.Log("[OnLocalPlayerScoresLoaded] error " + response.error);
			result = new AMN_LocalPlayerScoreLoadedResult(response.leaderboardId, response.scope.GetGCTimeSpan(), response.error);
		} else {
			Debug.Log("[OnLocalPlayerScoresLoaded] success " + response.rank + " " + response.score);
			result = new AMN_LocalPlayerScoreLoadedResult(response.leaderboardId, response.scope.GetGCTimeSpan(), response.rank, response.score);

			GC_Leaderboard leaderboard = GetLeaderboard(response.leaderboardId);
			if (leaderboard != null) {
				GC_Score score = new GC_Score(SA_AmazonGameCircleManager.Instance.Player.PlayerId, response.leaderboardId, response.rank, response.score, response.scope.GetGCTimeSpan());
				leaderboard.UpdateCurrentPlayerScore(score);
			}
		}

		OnLocalPlayerScoreLoaded(result);
	}

	private void OnTopScoresLoaded(AGSRequestScoresResponse response) {
		AMN_ScoresLoadedResult result = null;
		if (response.IsError()) {
			Debug.Log("[OnTopScoresLoaded] error " + response.error);
			result = new AMN_ScoresLoadedResult(response.leaderboardId, response.error);
		} else {
			Debug.Log("[OnTopScoresLoaded] " + response.scores.Count + " scores loaded");
			GC_Leaderboard lb = GetLeaderboard(response.leaderboardId);
			if (lb != null) {
				foreach (AGSScore score in response.scores) {
					Debug.Log(string.Format("[OnTopScoresLoaded] AGSScore {0}|{1}|{2}|{3}|{4}",
						score.player.playerId,
						response.leaderboardId,
						score.rank,
						score.scoreValue,
						response.scope.GetGCTimeSpan().ToString()));

					GC_Player player = new GC_Player(score.player);
					AddPlayer(player);

					GC_Score s = new GC_Score(score.player.playerId, response.leaderboardId, score.rank, score.scoreValue, response.scope.GetGCTimeSpan());
					lb.UpdateScore(s);
				}
			}
			result = new AMN_ScoresLoadedResult(lb);
		}

		OnScoresLoaded(result);
	}

	#endif
	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------

	public void Connect() {
		if(!_isInitialized) {
			Init();
		}
	}
	
	public void Disconnect() {
		//amazon disconnect
	}
	
	public void ShowGCOverlay() {
		#if AMAZON_CIRCLE_ENABLED
		AGSClient.ShowGameCircleOverlay ();
		#endif

	}
	
	public void ShowSignInPage() {
		#if AMAZON_CIRCLE_ENABLED
		AGSClient.ShowSignInPage ();
		#endif
	}

	public void RetrieveLocalPlayer() {
		#if AMAZON_CIRCLE_ENABLED
		AGSPlayerClient.RequestLocalPlayer ();
		#endif
	}

	public void ShowAchievementsOverlay() {
		#if AMAZON_CIRCLE_ENABLED
		AGSAchievementsClient.ShowAchievementsOverlay ();
		#endif
	}

	public void RequestAchievements() {
		#if AMAZON_CIRCLE_ENABLED
		AGSAchievementsClient.RequestAchievements ();
		#endif
	}

	public GC_Achievement GetAchievement(string id) {
		#if AMAZON_CIRCLE_ENABLED
		foreach (GC_Achievement achievement in Achievements) {
			if (achievement.Identifier.Equals(id)) {
				return achievement;
			}
		}
		#endif
		return null;
	}

	//submit progress
	public void UpdateAchievementProgress(string achieve_id, float score) {
		#if AMAZON_CIRCLE_ENABLED
		AGSAchievementsClient.UpdateAchievementProgress(achieve_id, score);
		#endif
	}	

	public void ShowLeaderboardsOverlay() {
		#if AMAZON_CIRCLE_ENABLED
		AGSLeaderboardsClient.ShowLeaderboardsOverlay ();
		#endif
	}

	public void RequestLeaderboards() {
		#if AMAZON_CIRCLE_ENABLED
		AGSLeaderboardsClient.RequestLeaderboards ();
		#endif
	}

	public void SubmitLeaderBoardProgress(string leaderBId, long score) {
		#if AMAZON_CIRCLE_ENABLED
		AGSLeaderboardsClient.SubmitScore(leaderBId, score);
		#endif
	}

	public GC_Leaderboard GetLeaderboard(string id) {
		#if AMAZON_CIRCLE_ENABLED
		foreach (GC_Leaderboard lb in Leaderboards) {
			if (lb.Identifier.Equals(id)) {
				return lb;
			}
		}
		#endif
		return null;
	}

	public void LoadLocalPlayerScores(string id, GC_ScoreTimeSpan timeSpan) {
		#if AMAZON_CIRCLE_ENABLED
		AGSLeaderboardsClient.RequestLocalPlayerScore(id, timeSpan.GetLeaderboardScope());
		#endif
	}

	public void LoadTopScores(string id, GC_ScoreTimeSpan timeSpan) {
		#if AMAZON_CIRCLE_ENABLED
		AGSLeaderboardsClient.RequestScores(id, timeSpan.GetLeaderboardScope());
		#endif
	}

	public void AddPlayer(GC_Player player) {
		if (!_Players.ContainsKey(player.PlayerId)) {
			_Players.Add(player.PlayerId, player);
		}
	}

	public GC_Player GetPlayerById(string id) {
		if (_Players.ContainsKey(id)) {
			return _Players[id];
		}
		return null;
	}

	//--------------------------------------
	// PRIVATE API CALL METHODS
	//--------------------------------------
	
	private void Init() {
		_isInitialized = true;

		#if AMAZON_CIRCLE_ENABLED
		AGSClient.Init (true, true, true);
		#endif
	}	

	private void SubscribeToEvents () {
		#if AMAZON_CIRCLE_ENABLED
		AGSClient.ServiceReadyEvent += OnServiceConnected;
		AGSClient.ServiceNotReadyEvent += OnServiceDisconnected;
		AGSPlayerClient.RequestLocalPlayerCompleted += OnPlayerDataLoaded;

		AGSAchievementsClient.RequestAchievementsCompleted += OnRequestAchievementsCompleted;
		AGSAchievementsClient.UpdateAchievementCompleted += OnUpdateAchievementCompleted;	

		AGSLeaderboardsClient.RequestLeaderboardsCompleted += OnRequestLeaderboardsCompleted;
		AGSLeaderboardsClient.SubmitScoreCompleted += OnSubmitScoreCompleted;
		AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted += OnLocalPlayerScoresLoaded;
		AGSLeaderboardsClient.RequestScoresCompleted += OnTopScoresLoaded;
		#endif
	}
}
