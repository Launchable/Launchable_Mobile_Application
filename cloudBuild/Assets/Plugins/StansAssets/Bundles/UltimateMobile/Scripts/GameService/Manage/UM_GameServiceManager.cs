using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UM_GameServiceManager : SA.Common.Pattern.Singleton<UM_GameServiceManager> {
	
	
	public static event Action OnPlayerConnected = delegate {};
	public static event Action OnPlayerDisconnected = delegate {};
	public static event Action<UM_ConnectionState> OnConnectionStateChnaged = delegate {};

	
	public static event Action<UM_LeaderboardResult> ActionScoreSubmitted = delegate {};
	public static event Action<UM_LeaderboardResult> ActionScoresListLoaded = delegate {};
	
	
	public static event Action<UM_Result> ActionFriendsListLoaded = delegate {};

	public static event Action<UM_Result> ActionAchievementsInfoLoaded = delegate {};
	public static event Action<UM_Result> ActionLeaderboardsInfoLoaded = delegate {};
	
	
	
	private bool _IsInitedCalled = false;
	private bool _IsDataLoaded = false;
	
	private int _DataEventsCount = 0;
	private int _CurrentEventsCount = 0;
	

	private UM_Player _Player = null ;
	private UM_ConnectionState _ConnectionSate = UM_ConnectionState.UNDEFINED;
	
	private static List<string> _FriendsList = new List<string>();
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	
	private void Init() {
		
		_IsInitedCalled = true;
		_DataEventsCount = 0;


		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:

			if(UltimateMobileSettings.Instance.AutoLoadAchievementsInfo) {
				_DataEventsCount+=1;
				GameCenterManager.OnAchievementsLoaded += OnGameCenterServiceDataLoaded;
			}

			if(UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo) {
				_DataEventsCount += UltimateMobileSettings.Instance.Leaderboards.Count;
			}
			GameCenterManager.OnLeadrboardInfoLoaded += OnGameCenterServiceLeaderDataLoaded;
			
			foreach(UM_Achievement achievment in UltimateMobileSettings.Instance.Achievements) {
				GameCenterManager.RegisterAchievement(achievment.IOSId);
			}
			
			GameCenterManager.OnAuthFinished += OnAuthFinished;

			
			GameCenterManager.OnScoreSubmitted += IOS_HandleOnScoreSubmitted;
			GameCenterManager.OnScoresListLoaded  += IOS_HandleOnScoresListLoaded;
			GameCenterManager.OnFriendsListLoaded += IOS_OnFriendsListLoaded;

			GameCenterManager.OnAchievementsLoaded  += IOS_AchievementsDataLoaded;
			GameCenterManager.OnLeadrboardInfoLoaded += IOS_LeaderboardsDataLoaded;
			
			
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				_DataEventsCount++;

				if (UltimateMobileSettings.Instance.AutoLoadAchievementsInfo) {
					_DataEventsCount++;
					SA_AmazonGameCircleManager.Instance.OnRequestAchievementsReceived += OnAmazonGameCircleRequestAchievementsReceived;
				}

				if (UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo) {
					//Important: For every Leaderboard 3 requests will be performed + 1 request for Leaderboards Metadata Request
					_DataEventsCount += UltimateMobileSettings.Instance.Leaderboards.Count * 3 + 1;
					SA_AmazonGameCircleManager.Instance.OnRequestLeaderboardsReceived += OnAmazonGameCircleRequestLeaderboardsReceived;
				}

				SA_AmazonGameCircleManager.Instance.OnInitializeResult += OnAmazonInitializeResult;
				SA_AmazonGameCircleManager.Instance.OnRequestPlayerDataReceived += OnAmazonRequestPlayerDataReceived;
				SA_AmazonGameCircleManager.Instance.OnRequestAchievementsReceived += OnAmazonRequestAchievementsReceived;
				SA_AmazonGameCircleManager.Instance.OnRequestLeaderboardsReceived += OnAmazonRequestLeaderboardsReceived;
				SA_AmazonGameCircleManager.Instance.OnSubmitLeaderboardReceived += OnAmazonSubmitLeaderboardReceived;
				SA_AmazonGameCircleManager.Instance.OnScoresLoaded += OnAmazonScoresLoaded;
			}
			else {
				if (UltimateMobileSettings.Instance.AutoLoadAchievementsInfo) {
					_DataEventsCount += 1;
					GooglePlayManager.ActionAchievementsLoaded += OnGooglePlayServiceDataLoaded;
				}
				
				if (UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo) {
					_DataEventsCount += 1;
				}
				GooglePlayManager.ActionLeaderboardsLoaded += OnGooglePlayLeaderDataLoaded;

				GooglePlayConnection.ActionPlayerConnected += OnAndroidPlayerConnected;
				GooglePlayConnection.ActionPlayerDisconnected += OnAndroidPlayerDisconnected;
				GooglePlayManager.ActionScoreSubmited += Android_HandleActionScoreSubmited;
				GooglePlayManager.ActionScoresListLoaded += Android_HandleActionScoresListLoaded;
				
				
				GooglePlayManager.ActionFriendsListLoaded += Android_ActionFriendsListLoaded;
				GooglePlayManager.ActionAchievementsLoaded += Android_AchievementsDataLoaded;
				GooglePlayManager.ActionLeaderboardsLoaded += Android_LeaderboardsDataLoaded;
			}
			break;
		}
	}

	//--------------------------------------
	// Connection
	//--------------------------------------
	
	public void Connect() {
		
		
		if(!_IsInitedCalled) {
			Init();
		}
		
		if(_ConnectionSate == UM_ConnectionState.CONNECTED || _ConnectionSate == UM_ConnectionState.CONNECTING) {
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.Init();
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.Connect ();
			} 
			else {
				GooglePlayConnection.Instance.Connect ();
			}
			break;
		}

		SetConnectionState(UM_ConnectionState.CONNECTING);
	}
	
	public void Disconnect() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.Disconnect();
			} else {
				GooglePlayConnection.Instance.Disconnect();
			}
			break;
		}
		
	}
	
	//--------------------------------------
	// Friends
	//--------------------------------------
	
	public void LoadFriends() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.RetrieveFriends();
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				//Just do nothing for Amazon
			} else {
				GooglePlayManager.Instance.LoadFriends();
			}
			break;
		}
	}
	
	public List<string> FriendsList {
		get {
			return _FriendsList;
		}
	}

	public bool IsParticipantFriend(UM_TBM_Participant playerParticipant) {
		return FriendsList.Contains (playerParticipant.Playerid);
	}
	
	public UM_Player GetPlayer(string playerId) {
		
		UM_Player player =  null;
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GK_Player gk_player =  GameCenterManager.GetPlayerById(playerId);
			if(gk_player != null) {
				player =  new UM_Player(gk_player, null, null);
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				GC_Player gc_player = SA_AmazonGameCircleManager.Instance.GetPlayerById(playerId);
				if (gc_player != null) {
					player = new UM_Player(null, null, gc_player);
				}
			} else {
				GooglePlayerTemplate gp_player = GooglePlayManager.Instance.GetPlayerById(playerId);
				if(gp_player != null) {
					player =  new UM_Player(null, gp_player, null);
				}
			}
			break;
		}
		
		return player;
		
		
	}
	
	
	//--------------------------------------
	// Achievements
	//--------------------------------------



	public void LoadAchievementsInfo() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.LoadAchievements();
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.RequestAchievements ();
			} else {
				GooglePlayManager.Instance.LoadAchievements ();
			}
			break;
		}
	}
	
	
	public void ShowAchievementsUI() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.ShowAchievements();
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.ShowAchievementsOverlay ();
			} else {
				GooglePlayManager.Instance.ShowAchievementsUI ();
			}
			break;
		}
	}
	
	public void RevealAchievement(string id) {
		RevealAchievement(UltimateMobileSettings.Instance.GetAchievementById(id));
	}
	
	public void RevealAchievement(UM_Achievement achievement) {
		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				//Just do nothing for Amazon
			} else {
				GooglePlayManager.Instance.RevealAchievementById(achievement.AndroidId);
			}
			break;
		}
	}
	
	[Obsolete("ReportAchievement is deprecated, please use UnlockAchievement instead.")]
	public void ReportAchievement(string id) {
		UnlockAchievement(id);
	}
	
	[Obsolete("ReportAchievement is deprecated, please use UnlockAchievement instead.")]
	public void ReportAchievement(UM_Achievement achievement) {
		ReportAchievement(achievement);
	}
	
	
	public void UnlockAchievement(string id) {
		UM_Achievement achievement = UltimateMobileSettings.Instance.GetAchievementById(id);
		if (achievement == null) {
			Debug.LogError("Achievment not found with id: " + id);
			return;
		}
		
		UnlockAchievement(achievement);
	}
	
	
	private void UnlockAchievement(UM_Achievement achievement) {
		
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.SubmitAchievement(100f, achievement.IOSId);
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.UpdateAchievementProgress(achievement.AmazonId, 100.0f);
			} else {
				GooglePlayManager.Instance.UnlockAchievementById(achievement.AndroidId);
			}
			break;
		}
	}
	
	
	public void IncrementAchievement(string id,  float percentages) {
		UM_Achievement achievement = UltimateMobileSettings.Instance.GetAchievementById(id);
		if (achievement == null) {
			Debug.LogError("Achievment not found with id: " + id);
			return;
		}
		
		IncrementAchievement(achievement, percentages);
	}
	
	
	public void IncrementAchievement(UM_Achievement achievement, float percentages) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.SubmitAchievement(percentages, achievement.IOSId);
			break;
		case RuntimePlatform.Android:

			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.UpdateAchievementProgress(achievement.AmazonId, percentages);
			} else {
				GPAchievement a = GooglePlayManager.Instance.GetAchievement(achievement.AndroidId);
				if(a != null) {
					if(a.Type == GPAchievementType.TYPE_INCREMENTAL) {
						int steps = Mathf.CeilToInt(( (float) a.TotalSteps / 100f) * percentages);
						GooglePlayManager.Instance.IncrementAchievementById(achievement.AndroidId, steps);
					} else {
						GooglePlayManager.Instance.UnlockAchievementById(achievement.AndroidId);
					}
				}	
			}
			break;
		}
	}

	public void IncrementAchievementByCurrentSteps(string id, int steps) {
		UM_Achievement achievement = UltimateMobileSettings.Instance.GetAchievementById(id);
		if (achievement == null) {
			Debug.LogError("Achievement not found with id: " + id);
			return;
		}
		
		IncrementAchievementByCurrentSteps(achievement, steps);
	}
	
	public void IncrementAchievementByCurrentSteps(UM_Achievement achievement, int steps) {
		switch (Application.platform) {
		case RuntimePlatform.IPhonePlayer: {
			float percentage = ((float)steps / (float)achievement.Steps) * 100f;
			GameCenterManager.SubmitAchievement(percentage, achievement.IOSId);
			break;
		}
		case RuntimePlatform.Android: {
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				float percentage = ((float)steps / (float)achievement.Steps) * 100f;
				SA_AmazonGameCircleManager.Instance.UpdateAchievementProgress(achievement.AmazonId, percentage);
			} else {
				GPAchievement a = GooglePlayManager.Instance.GetAchievement(achievement.AndroidId);
				if (a != null) {
					if (a.Type == GPAchievementType.TYPE_INCREMENTAL) {
						GooglePlayManager.Instance.IncrementAchievementById(a.Id, steps - a.CurrentSteps);
					} else {
						GooglePlayManager.Instance.UnlockAchievementById(a.Id);
					}
				}
			}
			
			break;
		}
		}
	}
	
	public void ResetAchievements() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.ResetAchievements();
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				//Just do nothing for Amazon
			} else {
				GooglePlayManager.Instance.ResetAllAchievements();	
			}
			break;
		}
	}
	
	
	public float GetAchievementProgress(string id) {
		UM_Achievement achievement = UltimateMobileSettings.Instance.GetAchievementById(id);
		if (achievement == null) {
			Debug.LogError("Achievment not found with id: " + id);
			return 0f;
		}
		
		return GetAchievementProgress(achievement);
	}
	
	public float GetAchievementProgress(UM_Achievement achievement) {
		if(achievement == null) {
			return 0f;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			return GameCenterManager.GetAchievementProgress(achievement.IOSId);
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				GC_Achievement ach = SA_AmazonGameCircleManager.Instance.GetAchievement(achievement.AmazonId);
				if (ach != null) {
					return ach.Progress;
				}
			} else {
				GPAchievement a = GooglePlayManager.Instance.GetAchievement(achievement.AndroidId);
				if(a != null) {
					if(a.Type == GPAchievementType.TYPE_INCREMENTAL) {
						return  ((float)a.CurrentSteps / a.TotalSteps) * 100f;
					} else {
						if(a.State == GPAchievementState.STATE_UNLOCKED) {
							return 100f;
						} else {
							return 0f;
						}
					}
				}
			}
			break;
		}
		
		return 0f;
	}
	
	public int GetAchievementProgressInSteps(string id) {
		UM_Achievement achievement = UltimateMobileSettings.Instance.GetAchievementById(id);
		if (achievement == null) {
			Debug.LogError("Achievement not found with id: " + id);
			return 0;
		}
		
		return GetAchievementProgressInSteps(achievement);
	}
	
	public int GetAchievementProgressInSteps(UM_Achievement achievement) {
		if (achievement == null) {
			Debug.LogError("Achievement is null. No progress can be retrieved.");
			return 0;
		}
		
		switch (Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			float percentage = GameCenterManager.GetAchievementProgress(achievement.IOSId);
			return Mathf.CeilToInt(((float)achievement.Steps / 100f) * percentage);
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				GC_Achievement ac = SA_AmazonGameCircleManager.Instance.GetAchievement(achievement.AmazonId);
				if (ac != null) {
					return Mathf.CeilToInt(((float)achievement.Steps / 100f) * ac.Progress);
				}
			} else {
				GPAchievement a = GooglePlayManager.Instance.GetAchievement(achievement.AndroidId);
				if (a != null) {
					if (a.Type == GPAchievementType.TYPE_INCREMENTAL) {
						return (a.CurrentSteps);
					} else {
						return (a.State == GPAchievementState.STATE_UNLOCKED) ? 1 : 0;
					}
				}
			}				
			break;
		}
		
		return 0;
	}
	
	//--------------------------------------
	// Leader-Boards
	//--------------------------------------

	private bool _WaitingForLeaderboardsData = false;
		
	private int _LeaderboardsDataEventsCount = 0;
	private int _CurrentLeaderboardsEventsCount = 0;

	public void LoadLeaderboardsInfo() {

		if(_WaitingForLeaderboardsData) {
			return;
		}

		_WaitingForLeaderboardsData = true;
		_LeaderboardsDataEventsCount = 0;
		_CurrentLeaderboardsEventsCount = 0;

		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			_LeaderboardsDataEventsCount = UltimateMobileSettings.Instance.Leaderboards.Count;
			foreach(UM_Leaderboard leaderboard in UltimateMobileSettings.Instance.Leaderboards) {
				GameCenterManager.LoadLeaderboardInfo(leaderboard.IOSId);
			}
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.RequestLeaderboards ();
			} else {
				GooglePlayManager.Instance.LoadLeaderBoards ();
			}
			break;
		}
	}
	
	public UM_Leaderboard GetLeaderboard(string leaderboardId) {
		return UltimateMobileSettings.Instance.GetLeaderboardById(leaderboardId);
	}
	
	
	public void ShowLeaderBoardsUI() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.ShowLeaderboards();
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.ShowLeaderboardsOverlay ();
			} else {
				GooglePlayManager.Instance.ShowLeaderBoardsUI ();
			}
			break;
		}
	}
	
	
	public void ShowLeaderBoardUI(string id) {
		ShowLeaderBoardUI(UltimateMobileSettings.Instance.GetLeaderboardById(id));
	}
	
	public void ShowLeaderBoardUI(UM_Leaderboard leaderboard) {
		if(leaderboard == null) {
			return;
		}
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.ShowLeaderboard(leaderboard.IOSId);
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.ShowLeaderboardsOverlay ();
			} else {
				GooglePlayManager.Instance.ShowLeaderBoardById(leaderboard.AndroidId);
			}
			break;
		}
	}
	
	
	public void SubmitScore(string LeaderboardId, long score, long context = 0) {
		SubmitScore(UltimateMobileSettings.Instance.GetLeaderboardById(LeaderboardId), score);
	}
	
	public void SubmitScore(UM_Leaderboard leaderboard, long score, long context = 0) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.ReportScore(score, leaderboard.IOSId, context);
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.SubmitLeaderBoardProgress(leaderboard.AmazonId, score);
			} else {
				GooglePlayManager.Instance.SubmitScoreById(leaderboard.AndroidId, score, context.ToString());
			}
			break;
		}
	}
	
	
	public UM_Score GetCurrentPlayerScore(string leaderBoardId, UM_TimeSpan timeSpan = UM_TimeSpan.ALL_TIME, UM_CollectionType collection = UM_CollectionType.GLOBAL) {
		return GetCurrentPlayerScore(UltimateMobileSettings.Instance.GetLeaderboardById(leaderBoardId), timeSpan, collection);
	} 
	
	public UM_Score GetCurrentPlayerScore(UM_Leaderboard leaderboard, UM_TimeSpan timeSpan = UM_TimeSpan.ALL_TIME, UM_CollectionType collection = UM_CollectionType.GLOBAL) {
		
		if(leaderboard != null) {
			return leaderboard.GetCurrentPlayerScore(timeSpan, collection);
		}
		
		return null;
	} 
	
	
	public void LoadPlayerCenteredScores(string leaderboardId, int maxResults, UM_TimeSpan timeSpan = UM_TimeSpan.ALL_TIME, UM_CollectionType collection = UM_CollectionType.GLOBAL) {
		UM_Leaderboard leaderboard = UltimateMobileSettings.Instance.GetLeaderboardById(leaderboardId);
		LoadPlayerCenteredScores(leaderboard, maxResults, timeSpan, collection);
	}
	
	public void LoadPlayerCenteredScores(UM_Leaderboard leaderboard, int maxResults, UM_TimeSpan timeSpan = UM_TimeSpan.ALL_TIME, UM_CollectionType collection = UM_CollectionType.GLOBAL) {
		if(leaderboard == null) {
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			
			UM_Score score = GetCurrentPlayerScore(leaderboard, timeSpan, collection);
			int rank = 0;
			if(score != null) {
				rank = score.Rank;
			}
			
			int startIndex = Math.Max(1, rank - maxResults / 2);
			int endIndex = startIndex + maxResults - 1;
			GameCenterManager.LoadScore(leaderboard.IOSId, startIndex, endIndex, timeSpan.Get_GK_TimeSpan(), collection.Get_GK_CollectionType());
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				//Just do nothing for Amazon
			} else {
				GooglePlayManager.Instance.LoadPlayerCenteredScores(leaderboard.AndroidId, timeSpan.Get_GP_TimeSpan(), collection.Get_GP_CollectionType(), maxResults);	
			}
			break;
			
		}
	}
	
	
	public void LoadTopScores(string leaderboardId, int maxResults, UM_TimeSpan timeSpan = UM_TimeSpan.ALL_TIME, UM_CollectionType collection = UM_CollectionType.GLOBAL) {
		UM_Leaderboard leaderboard = UltimateMobileSettings.Instance.GetLeaderboardById(leaderboardId);
		LoadTopScores(leaderboard, maxResults, timeSpan, collection);
	}
	
	public void LoadTopScores(UM_Leaderboard leaderboard, int maxResults, UM_TimeSpan timeSpan = UM_TimeSpan.ALL_TIME, UM_CollectionType collection = UM_CollectionType.GLOBAL) {
		
		if(leaderboard == null) {
			return;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.LoadScore(leaderboard.IOSId, 1, maxResults, timeSpan.Get_GK_TimeSpan(), collection.Get_GK_CollectionType());
			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {
				SA_AmazonGameCircleManager.Instance.LoadTopScores(leaderboard.AmazonId, timeSpan.Get_GC_TimeSpan());
			} else {
				GooglePlayManager.Instance.LoadTopScores(leaderboard.AndroidId, timeSpan.Get_GP_TimeSpan(), collection.Get_GP_CollectionType(), maxResults);
			}
			break;
			
		}
	}
	
	
	
	
	//--------------------------------------
	// Get / Set
	//--------------------------------------
	
	
	public UM_ConnectionState ConnectionSate {
		get {
			return _ConnectionSate;
		}
	}
	
	public bool IsConnected {
		get {
			return ConnectionSate == UM_ConnectionState.CONNECTED;
		}
	}
	
	[System.Obsolete("player is deprectaed, plase use Player instead")]
	public UM_Player player {
		get {
			return _Player;
		}
	}
	
	public UM_Player Player {
		get {
			return _Player;
		}
	}
	
	
	
	//--------------------------------------
	// Events
	//--------------------------------------
	
	private void OnServiceConnected() {
		
		if(_IsDataLoaded || _DataEventsCount <= 0) {
			_IsDataLoaded = true;
			OnAllLoaded();
			return;
		}
		
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			if(UltimateMobileSettings.Instance.AutoLoadAchievementsInfo) {
				GameCenterManager.LoadAchievements();
			}

			if(UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo) {
				foreach(UM_Leaderboard leaderboard in UltimateMobileSettings.Instance.Leaderboards) {
					GameCenterManager.LoadLeaderboardInfo(leaderboard.IOSId);
				}
			}

			break;
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon) {

				Debug.Log("Start To Load Amazon Player");
				SA_AmazonGameCircleManager.Instance.RetrieveLocalPlayer();
				
				if (UltimateMobileSettings.Instance.AutoLoadAchievementsInfo) {
					SA_AmazonGameCircleManager.Instance.RequestAchievements ();
				}

				if (UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo) {
					SA_AmazonGameCircleManager.Instance.RequestLeaderboards ();
				}

			} else {
				if (UltimateMobileSettings.Instance.AutoLoadAchievementsInfo) {
					GooglePlayManager.Instance.LoadAchievements ();
				}
			
				if (UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo) {
					GooglePlayManager.Instance.LoadLeaderBoards ();
				}
			}
			break;
		}
	}
	
	private void OnGooglePlayServiceDataLoaded(GooglePlayResult result) {

		_CurrentEventsCount++;
		if(_CurrentEventsCount == _DataEventsCount) {
			OnAllLoaded();
		}
			
	}
	
	
	void OnGooglePlayLeaderDataLoaded (GooglePlayResult res) {
		
		foreach(GPLeaderBoard lb in GooglePlayManager.Instance.LeaderBoards)  {
			UM_Leaderboard leaderboard =  UltimateMobileSettings.Instance.GetLeaderboardByAndroidId(lb.Id);
			if(leaderboard != null) {
				leaderboard.Setup(lb);
			}
		}
		
		OnGooglePlayServiceDataLoaded(res);
		
	}
	
	
	private void OnGameCenterServiceDataLoaded(SA.Common.Models.Result e) {
		_CurrentEventsCount++;
		if(_CurrentEventsCount == _DataEventsCount) {
			OnAllLoaded();
		}
	}
	
	private void OnGameCenterServiceLeaderDataLoaded(GK_LeaderboardResult res) {
		if (res.IsSucceeded && res.Leaderboard != null) {
			UM_Leaderboard leaderboard =  UltimateMobileSettings.Instance.GetLeaderboardByIOSId(res.Leaderboard.Id);
			if(leaderboard != null) {
				leaderboard.Setup(res.Leaderboard);
			}
		}
		
		OnGameCenterServiceDataLoaded(res);
	}
	
	
	
	private void OnAllLoaded() {
		Debug.Log("All Data Loaded! We are ready to GO!");

		_IsDataLoaded = true;

		_Player =  new UM_Player(GameCenterManager.Player, GooglePlayManager.Instance.player, SA_AmazonGameCircleManager.Instance.Player);
		
		SetConnectionState(UM_ConnectionState.CONNECTED);
		OnPlayerConnected();
	}


	//--------------------------------------
	// United Events
	//--------------------------------------
	
	void OnAchievementsDataLoaded (UM_Result result) {
		ActionAchievementsInfoLoaded(result);
	}

	void OnLeaderboardsDataLoaded (UM_Result result) {
		_WaitingForLeaderboardsData = false;
		ActionLeaderboardsInfoLoaded(result);
	}
	
	
	//--------------------------------------
	// IOS Events
	//--------------------------------------
	
	
	
	private void OnAuthFinished (SA.Common.Models.Result res) {
		if(res.IsSucceeded) {
			OnServiceConnected();
		} else {
			SetConnectionState(UM_ConnectionState.DISCONNECTED);
			OnPlayerDisconnected();
		}
	}
	
	
	void IOS_HandleOnScoreSubmitted (GK_LeaderboardResult res) {
		
		UM_Leaderboard leaderboard =  UltimateMobileSettings.Instance.GetLeaderboardByIOSId(res.Leaderboard.Id);
		if(leaderboard != null) {
			leaderboard.Setup(res.Leaderboard);
			
			UM_LeaderboardResult result =  new UM_LeaderboardResult(leaderboard, res);
			ActionScoreSubmitted(result);
		}
		
		
	}
	
	
	void IOS_HandleOnScoresListLoaded (GK_LeaderboardResult res) {
		UM_Leaderboard leaderboard =  UltimateMobileSettings.Instance.GetLeaderboardByIOSId(res.Leaderboard.Id);
		if(leaderboard != null) {
			leaderboard.Setup(res.Leaderboard);
			
			UM_LeaderboardResult result =  new UM_LeaderboardResult(leaderboard, res);
			ActionScoresListLoaded(result);
		}
	}
	
	void IOS_OnFriendsListLoaded (SA.Common.Models.Result res) {
		SetFriendList(GameCenterManager.FriendsList);
		ActionFriendsListLoaded( new UM_Result(res));
	}

	void IOS_AchievementsDataLoaded (SA.Common.Models.Result res) {
		UM_Result result = new UM_Result(res);
		OnAchievementsDataLoaded(result);
	}	


	void IOS_LeaderboardsDataLoaded (GK_LeaderboardResult res){
		if(_WaitingForLeaderboardsData) {
			_CurrentLeaderboardsEventsCount ++;
			if(_CurrentLeaderboardsEventsCount >= _LeaderboardsDataEventsCount) {
				UM_Result result = new UM_Result();
				OnLeaderboardsDataLoaded(result);
			}
		}
	}
	
	//--------------------------------------
	// Android Events
	//--------------------------------------
	
	private void OnAndroidPlayerConnected() {
		OnServiceConnected();
	}
	
	private void OnAndroidPlayerDisconnected() {
		SetConnectionState(UM_ConnectionState.DISCONNECTED);
		OnPlayerDisconnected();
	}
	
	
	void Android_HandleActionScoresListLoaded (GP_LeaderboardResult res) {
		UM_Leaderboard leaderboard =  UltimateMobileSettings.Instance.GetLeaderboardByAndroidId(res.Leaderboard.Id);
		if(leaderboard != null) {
			leaderboard.Setup(res.Leaderboard);
			
			UM_LeaderboardResult result =  new UM_LeaderboardResult(leaderboard, res);
			ActionScoresListLoaded(result);
		}
	}
	
	
	
	void Android_HandleActionScoreSubmited (GP_LeaderboardResult res) {
		UM_Leaderboard leaderboard =  UltimateMobileSettings.Instance.GetLeaderboardByAndroidId(res.Leaderboard.Id);
		if(leaderboard != null) {
			leaderboard.Setup(res.Leaderboard);
			
			UM_LeaderboardResult result =  new UM_LeaderboardResult(leaderboard, res);
			ActionScoreSubmitted(result);
		}
	}
	
	
	void Android_ActionFriendsListLoaded (GooglePlayResult res) {
		SetFriendList(GooglePlayManager.Instance.friendsList);
		ActionFriendsListLoaded(new UM_Result(res));
	}

	void Android_AchievementsDataLoaded (GooglePlayResult res) {
		UM_Result result =  new UM_Result(res);
		OnAchievementsDataLoaded(result);
	}

	void Android_LeaderboardsDataLoaded (GooglePlayResult res) {
		UM_Result result =  new UM_Result(res);
		OnLeaderboardsDataLoaded(result);
	}

	//--------------------------------------
	// Amazon Events
	//--------------------------------------

	void OnAmazonInitializeResult(AMN_InitializeResult result) {
		if(result.isSuccess) {
			OnServiceConnected();
		} else {
			SetConnectionState(UM_ConnectionState.DISCONNECTED);
			OnPlayerDisconnected();
		}
	}

	void OnAmazonRequestPlayerDataReceived(AMN_RequestPlayerDataResult result) {
		_CurrentEventsCount++;
		if (_CurrentEventsCount == _DataEventsCount) {
			OnAllLoaded();
		}
	}

	private void OnAmazonGameCircleRequestLeaderboardsReceived (AMN_RequestLeaderboardsResult result)
	{
		SA_AmazonGameCircleManager.Instance.OnRequestLeaderboardsReceived -= OnAmazonGameCircleRequestLeaderboardsReceived;

		_CurrentEventsCount++;
		if (_CurrentEventsCount == _DataEventsCount) {
			OnAllLoaded();
		} else {
			SA_AmazonGameCircleManager.Instance.OnLocalPlayerScoreLoaded += OnAmazonLocalPlayerScoreLoaded;
			foreach (UM_Leaderboard leaderboard in UltimateMobileSettings.Instance.Leaderboards) {
				SA_AmazonGameCircleManager.Instance.LoadLocalPlayerScores(leaderboard.AmazonId, GC_ScoreTimeSpan.ALL_TIME);
				SA_AmazonGameCircleManager.Instance.LoadLocalPlayerScores(leaderboard.AmazonId, GC_ScoreTimeSpan.WEEK);
				SA_AmazonGameCircleManager.Instance.LoadLocalPlayerScores(leaderboard.AmazonId, GC_ScoreTimeSpan.TODAY);
			}
		}
	}

	private void OnAmazonLocalPlayerScoreLoaded (AMN_LocalPlayerScoreLoadedResult result)
	{
		Debug.Log(string.Format("[OnAmazonLocalPlayerScoreLoaded] {0}|{1}|{2}|{3}", result.LeaderboardId, result.TimeSpan.ToString(), result.Rank, result.Score));
		_CurrentEventsCount++;
		if (_CurrentEventsCount == _DataEventsCount) {
			SA_AmazonGameCircleManager.Instance.OnLocalPlayerScoreLoaded -= OnAmazonLocalPlayerScoreLoaded;
			OnAllLoaded();
		}
	}

	private void OnAmazonGameCircleRequestAchievementsReceived (AMN_RequestAchievementsResult result)
	{
		SA_AmazonGameCircleManager.Instance.OnRequestAchievementsReceived -= OnAmazonGameCircleRequestAchievementsReceived;

		_CurrentEventsCount++;
		if (_CurrentEventsCount == _DataEventsCount) {
			OnAllLoaded();
		}
	}

	void OnAmazonRequestAchievementsReceived(AMN_RequestAchievementsResult result) {
		UM_Result res = new UM_Result(result);
		OnAchievementsDataLoaded(res);
	}

	void OnAmazonRequestLeaderboardsReceived(AMN_RequestLeaderboardsResult result) {
		UM_Result res =  new UM_Result(result);
		OnLeaderboardsDataLoaded(res);
	}

	void OnAmazonSubmitLeaderboardReceived(AMN_SubmitLeaderboardResult result) {
		UM_Leaderboard leaderboard = UltimateMobileSettings.Instance.GetLeaderboardByAmazonId(result.LeaderboardID);
		if(leaderboard != null) {
			leaderboard.Setup(SA_AmazonGameCircleManager.Instance.GetLeaderboard(result.LeaderboardID));

			UM_LeaderboardResult res = new UM_LeaderboardResult(leaderboard, result);
			ActionScoreSubmitted(res);
		}
	}

	private void OnAmazonScoresLoaded (AMN_ScoresLoadedResult result)
	{
		UM_Leaderboard leaderboard = UltimateMobileSettings.Instance.GetLeaderboardByAmazonId(result.LeaderboardId);
		if (leaderboard != null) {
			leaderboard.Setup(SA_AmazonGameCircleManager.Instance.GetLeaderboard(result.LeaderboardId));

			UM_LeaderboardResult res = new UM_LeaderboardResult(leaderboard, result);
			ActionScoresListLoaded(res);
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------


	private void SetConnectionState(UM_ConnectionState NewState) {
		if(_ConnectionSate != NewState) {
			_ConnectionSate = NewState;
			OnConnectionStateChnaged(_ConnectionSate);
		}

	}

	private void SetFriendList(List<string> friendsIds) {
		_FriendsList.Clear();
		foreach(string id in friendsIds) {
			if(!id.Equals(Player.PlayerId)) {
				_FriendsList.Add(id);
			}
		}
	}
	
	

}
