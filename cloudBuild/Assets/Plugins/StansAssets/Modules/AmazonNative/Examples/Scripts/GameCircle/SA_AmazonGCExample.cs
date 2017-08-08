using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SA_AmazonGCExample : MonoBehaviour  {
		
	public DefaultPreviewButton[] buttons;

	public SA_Label playerLabel;
	public SA_Label playerID;
	public SA_Label alias;
	
	private AMN_WWWTextureLoader loader;
	private Texture2D image;
	public GameObject avatar;

	private List<GC_Achievement> achievements = null;
	private List<GC_Leaderboard> leaderboards = null;
	private bool isInitialized = false;

	private long leaderboard_progress = 100;
	private float achieve_progress = 150;

	private string achieve_id = "first_achievement";
	private string leaderboard_id = "first_leaderboard";

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Start() {		
		loader = AMN_WWWTextureLoader.Create();
		DisableButtons ();

		SA_AmazonGameCircleManager.Instance.OnInitializeResult += OnInitializeResult;
		SA_AmazonGameCircleManager.Instance.OnRequestPlayerDataReceived += OnRequestPlayerDataReceived;
		SA_AmazonGameCircleManager.Instance.OnRequestAchievementsReceived += OnRequestAchievementsReceived;
		SA_AmazonGameCircleManager.Instance.OnUpdateAchievementReceived += OnUpdateAchievementReceived;
		SA_AmazonGameCircleManager.Instance.OnRequestLeaderboardsReceived += OnRequestLeaderboardsReceived;
		SA_AmazonGameCircleManager.Instance.OnSubmitLeaderboardReceived += OnSubmitLeaderboardReceived;
	}
		
	void OnGUI() {
		if (isInitialized) {
			EnableButtons();
		}						
	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------

	void OnInitializeResult(AMN_InitializeResult result) {
		if (result.isSuccess) {			
			isInitialized = true;

			playerLabel.text = "Player Connected";
			SA_StatusBar.text = "Amazon connected";
		} else {			
			playerLabel.text = "Player Disconnected";	
			SA_StatusBar.text = "Amazon disconnected with error " + result.Error;
		}
	}

	void OnRequestPlayerDataReceived(AMN_RequestPlayerDataResult result) {
		if (result.isSuccess) {			
			GC_Player player = result.Player;
			playerID.text = "PlayerID: " + " " + player.PlayerId;
			alias.text = "Alias: " + " " + player.Name;
			LoadAvatar (player.AvatarUrl);
		} else {			
			playerID.text = "PlayerID: none";
			alias.text = "Alias: none";
		}
	}

	private void OnProfileImageLoaded(Texture2D texture) {		
		loader.OnLoad -= OnProfileImageLoaded;

		if(texture != null) {
			image = texture;
		}
		avatar.GetComponent<Renderer>().material.mainTexture = image;
	}

	void OnRequestAchievementsReceived(AMN_RequestAchievementsResult result) {
		if (result.isSuccess) {
			achievements = result.AchievementList;

			Debug.Log("Printing Achievements list, total items: " + achievements.Count);
			foreach(GC_Achievement ach in achievements) {
				Debug.Log(ach.Identifier);
				Debug.Log(ach.Description);
				Debug.Log(ach.PointValue);
				Debug.Log(ach.DateUnlocked);

			}


			SA_StatusBar.text = "OnRequest Achievements success!";
		} else {
			SA_StatusBar.text = "OnRequest Achievements Failed with error " + result.Error;
		}
	}

	void OnRequestLeaderboardsReceived(AMN_RequestLeaderboardsResult result) {
		if (result.isSuccess) {
			leaderboards = result.LeaderboardsList;


			Debug.Log("Printing Leaderboards list, total items: " + leaderboards.Count);
			foreach(GC_Leaderboard lb in leaderboards) {
				Debug.Log(lb.Identifier);
				Debug.Log(lb.Title);
				Debug.Log(lb.Description);
				Debug.Log(lb.ImageUrl);
			}

			SA_StatusBar.text = "OnRequest Leaderboards success!";
		} else {
			SA_StatusBar.text = "OnRequest Leaderboards Failed with error " + result.Error;
		}
	}

	void OnUpdateAchievementReceived(AMN_UpdateAchievementResult result) {
		if (result.isSuccess) {
			SA_StatusBar.text = "OnUpdate Achievement Completed for id " + result.AchievementID;
		} else {
			SA_StatusBar.text =  "OnUpdate Achievement Failed for id " + result.AchievementID + result.Error;
		}
	}

	void OnSubmitLeaderboardReceived(AMN_SubmitLeaderboardResult result) {
		if (result.isSuccess) {
			SA_StatusBar.text = "OnSubmit Leaderboard Completed for id " + result.LeaderboardID;
		} else {
			SA_StatusBar.text = "OnSubmit Leaderboard Failed for id " + result.LeaderboardID + result.Error;
		}
	}

	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------

	//--------------------------------------
	// PRIVATE API CALL METHODS
	//--------------------------------------


	private void InitializeAmazon() {
		if (SA_AmazonGameCircleManager.Instance.IsInitialized) {
			SA_StatusBar.text = "Disconnecting from Amazon Service";
			SA_AmazonGameCircleManager.Instance.Disconnect ();
		} else {
			SA_StatusBar.text = "Connecting to Amazon";
			SA_AmazonGameCircleManager.Instance.Connect ();
		}
	}
	
	private void DisableButtons() {
		foreach(DefaultPreviewButton button in buttons) {
			button.DisabledButton();
		}
	}
	
	private void EnableButtons() {
		foreach(DefaultPreviewButton button in buttons) {
			button.EnabledButton();
		}
	}

	private void ShowGCOverlay() {
		SA_AmazonGameCircleManager.Instance.ShowGCOverlay ();
	}
	
	private void ShowSignInPage() {
		SA_AmazonGameCircleManager.Instance.ShowSignInPage ();
	}
	
	private void RetrieveLocalPlayer() {
		SA_AmazonGameCircleManager.Instance.RetrieveLocalPlayer ();
	}
	
	private void ShowAchievementsOverlay() {
		SA_AmazonGameCircleManager.Instance.ShowAchievementsOverlay ();
	}	

	private void RequestAchievements() {
		SA_AmazonGameCircleManager.Instance.RequestAchievements ();
	}	
	
	private void LoadAvatar (string url) {
		loader.OnLoad += OnProfileImageLoaded;
		loader.LoadTexture (url);
	}

	private void UpdateAchievementProgress() {
		SA_AmazonGameCircleManager.Instance.UpdateAchievementProgress (achieve_id, achieve_progress);
	}	

	private void ShowLeaderboardsOverlay() {
		SA_AmazonGameCircleManager.Instance.ShowLeaderboardsOverlay ();
	}

	private void RequestLeaderboards() {
		SA_AmazonGameCircleManager.Instance.RequestLeaderboards ();
	}

	private void SubmitLeaderBoardProgress() {
		SA_AmazonGameCircleManager.Instance.SubmitLeaderBoardProgress (leaderboard_id, leaderboard_progress++);
	}
}
