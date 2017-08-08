using UnityEngine;
using System.Collections;

public class RTM_GameExampleController : MonoBehaviour {

	public GameObject avatar;
	public GameObject hi;
	public SA_Label playerLabel;
	public SA_Label gameState;
	public SA_Label parisipants;
	
	public DefaultPreviewButton connectButton;
	
	
	public DefaultPreviewButton helloButton;
	public DefaultPreviewButton leaveRoomButton;
	public DefaultPreviewButton showRoomButton;
	
	
	public DefaultPreviewButton[] ConnectionDependedntButtons;
	public SA_PartisipantUI[] patricipants;
	public SA_FriendUI[] friends;
	
	
	private Texture defaulttexture;
	
	
	void Start() {
		
		playerLabel.text = "Player Disconnected";
		defaulttexture = avatar.GetComponent<Renderer>().material.mainTexture;

		RTM.Matchmaker.InvitationReceived += HandleInvitationReceived;
		RTM.Matchmaker.InvitationAccepted += HandleInvitationAccepted;

		RTM.Matchmaker.RoomCreated += HandleRoomCreated;

		RTM.Matchmaker.MatchDataReceived += HandleMatchDataReceived;
		
		//listen for GooglePlayConnection events
		//GooglePlayInvitationManager.ActionInvitationReceived += OnInvite;
		//GooglePlayInvitationManager.ActionInvitationAccepted += ActionInvitationAccepted;
		//GooglePlayRTM.ActionRoomCreated += OnRoomCreated;
		
		//GooglePlayConnection.ActionPlayerConnected +=  OnPlayerConnected;
		//GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;

		UM_GameServiceManager.OnPlayerConnected += OnPlayerConnected;
		UM_GameServiceManager.OnPlayerDisconnected += OnPlayerDisconnected;
		
		if(UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			//checking if player already connected
			OnPlayerConnected ();
		} 
		
		//networking event
		//GooglePlayRTM.ActionDataRecieved += OnGCDataReceived;
		
		
	}

	void HandleMatchDataReceived (string senderId, byte[] data)
	{
		string str = string.Empty;
#if !UNITY_WP8 && !UNITY_WSA
		System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
		str = enc.GetString(data);
#endif
		string name = senderId;
		
		UM_RTM_Participant p =  RTM.Matchmaker.CurrentRoom.GetParticipantById(senderId);
		if(p != null) {
			name = p.Name;
		}
		
		AndroidMessage.Create("Data Eeceived", "player " + name + " \n " + "data: " + str);
	}

	void HandleRoomCreated (UM_RTM_RoomCreatedResult result)
	{
		SA_StatusBar.text = "Room Create Result:  " + result.IsSuccess.ToString();
	}

	void HandleInvitationAccepted (UM_RTM_Invite invite)
	{
		Debug.Log("ActionInvitationAccepted called");		
		
		Debug.Log("Starting The Game");
		//make sure you have prepared your scene to start the game before you accepting the invite. Room join even will be triggered
		//RTM.Matchmaker.AcceptInvite(invite);
	}

	void HandleInvitationReceived (UM_RTM_Invite invite)
	{		
		inviteId = invite.Id;
		
		AndroidDialog dialog =  AndroidDialog.Create("Invite", "You have new invite from: " + invite.SenderId, "Manage Manually", "Open Google Inbox");
		dialog.ActionComplete += OnInvDialogComplete;
	}
	
	private void ConncetButtonPress() {
		Debug.Log("UM_GameServiceManager State  -> " + UM_GameServiceManager.Instance.ConnectionSate.ToString());
		if (UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			SA_StatusBar.text = "Disconnecting from Play Service...";
			//GooglePlayConnection.instance.Disconnect ();
			UM_GameServiceManager.Instance.Disconnect();
		} else {
			SA_StatusBar.text = "Connecting to Play Service...";
			//GooglePlayConnection.instance.Connect ();
			UM_GameServiceManager.Instance.Connect();
		}
	}
	
	
	private void ShowWatingRoom() {
		//GooglePlayRTM.instance.ShowWaitingRoomIntent();
	}
	
	
	/*
	private static int ROLE_FARMER = 0x1; // 001 in binary
	private static int ROLE_ARCHER = 0x2; // 010 in binary
	private static int ROLE_WIZARD = 0x4; // 100 in binary


	private static int TRACK_1 = 1; // 100 in binary
	private static int TRACK_2 = 2; // 100 in binary
*/
	
	private void findMatch() {
		/*
		GooglePlayRTM.instance.SetExclusiveBitMask (ROLE_WIZARD);
		GooglePlayRTM.instance.SetVariant (TRACK_1);
*/
		int minPlayers = 1;
		int maxPlayers = 2;
		
		//GooglePlayRTM.instance.FindMatch(minPlayers, maxPlayers);
		RTM.Matchmaker.FindMatch(minPlayers, maxPlayers);
	}
	
	private void InviteFriends() {
		//int minPlayers = 1;
//		int maxPlayers = 2;
		//GooglePlayRTM.instance.OpenInvitationBoxUI(minPlayers, maxPlayers);
	}
	
	
	private void SendHello() {
		string msg = "hello world";
		System.Text.UTF8Encoding  encoding = new System.Text.UTF8Encoding();
		byte[] data = encoding.GetBytes(msg);
		
		RTM.Matchmaker.SendDataToAll (data, UM_RTM_PackageType.Reliable);
		
	}
	
	private void LeaveRoom() {
		//GooglePlayRTM.instance.LeaveRoom();
		RTM.Matchmaker.LeaveMatch();
	}
	
	
	
	private void DrawParticipants() {
		
		//UpdateGameState("Room State: " + GooglePlayRTM.instance.currentRoom.status.ToString());
		parisipants.text = "Total Room Participants: " + RTM.Matchmaker.CurrentRoom.Participants.Count; //GooglePlayRTM.instance.currentRoom.participants.Count;
		
		
		
		foreach(SA_PartisipantUI p in patricipants) {
			p.gameObject.SetActive(false);
		}
		
		int i = 0;

		foreach (UM_RTM_Participant p in RTM.Matchmaker.CurrentRoom.Participants) {
			patricipants[i].gameObject.SetActive(true);
			patricipants[i].SetParticipant(p);
			i++;
		}

		/*foreach(GP_Participant p in GooglePlayRTM.instance.currentRoom.participants) {
			patricipants[i].gameObject.SetActive(true);
			patricipants[i].SetParticipant(p);
			i++;
		}*/
		
		
	}
	
	private void UpdateGameState(string msg) {
		gameState.text = msg;
	}
	
	void FixedUpdate() {
		DrawParticipants();
		
		/*if(GooglePlayRTM.instance.currentRoom.status!= GP_RTM_RoomStatus.ROOM_VARIANT_DEFAULT && GooglePlayRTM.instance.currentRoom.status!= GP_RTM_RoomStatus.ROOM_STATUS_ACTIVE) {
			showRoomButton.EnabledButton();
		} else {
			showRoomButton.DisabledButton();
		}
		
		
		
		if(GooglePlayRTM.instance.currentRoom.status == GP_RTM_RoomStatus.ROOM_VARIANT_DEFAULT) {
			leaveRoomButton.DisabledButton();
		} else {
			leaveRoomButton.EnabledButton();
		}
		
		if(GooglePlayRTM.instance.currentRoom.status == GP_RTM_RoomStatus.ROOM_STATUS_ACTIVE) {
			helloButton.EnabledButton();
			hi.SetActive(true);
		} else {
			helloButton.DisabledButton();
			hi.SetActive(false);
		}*/
		
		if(UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			if (UM_GameServiceManager.Instance.Player.SmallPhoto != null) {
				avatar.GetComponent<Renderer>().material.mainTexture = UM_GameServiceManager.Instance.Player.SmallPhoto;
			} 
		}  else {
			avatar.GetComponent<Renderer>().material.mainTexture = defaulttexture;
		}
		
		
		string title = "Connect";
		if(UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			title = "Disconnect";
			
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.EnabledButton();
			}
			
			
		} else {
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.DisabledButton();
				
			}
			if(UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.DISCONNECTED || UM_GameServiceManager.Instance.ConnectionSate == UM_ConnectionState.UNDEFINED) {
				
				title = "Connect";
			} else {
				title = "Connecting..";
			}
		}
		
		connectButton.text = title;
	}
	
	private void OnPlayerDisconnected() {
		SA_StatusBar.text = "Player Disconnected";
		playerLabel.text = "Player Disconnected";
	}
	
	private void OnPlayerConnected() {
		SA_StatusBar.text = "Player Connected";
		playerLabel.text = UM_GameServiceManager.Instance.Player.Name;	
		
		//GooglePlayManager.ActionFriendsListLoaded +=  OnFriendListLoaded;
		//GooglePlayManager.Instance.LoadFriends();
	}
	
	void OnFriendListLoaded (GooglePlayResult result) {
		/*Debug.Log("OnFriendListLoaded: " + result.Message);
		GooglePlayManager.ActionFriendsListLoaded -=  OnFriendListLoaded;
		
		if(result.IsSucceeded) {
			Debug.Log("Friends Load Success");
			
			int i = 0;
			foreach(string fId in GooglePlayManager.instance.friendsList) {
				if(i < 3) {
					friends[i].SetFriendId(fId);
				}
				i++;
			}
		}*/
	}
	
	private string inviteId;
	private void OnInvite(GP_Invite invitation) {
		
		if (invitation.InvitationType != GP_InvitationType.INVITATION_TYPE_REAL_TIME) {
			return;
		}
		
		inviteId = invitation.Id;
		
		AndroidDialog dialog =  AndroidDialog.Create("Invite", "You have new invite from: " + invitation.Participant.DisplayName, "Manage Manually", "Open Google Inbox");
		dialog.ActionComplete += OnInvDialogComplete;
	}

	private void OnInvDialogComplete(AndroidDialogResult result) {
		
		
		
		//parsing result
		switch(result) {
		case AndroidDialogResult.YES:
			AndroidDialog dialog =  AndroidDialog.Create("Manage Invite", "Would you like to accept this invite?", "Accept", "Decline");
			dialog.ActionComplete += OnInvManageDialogComplete;
			break;
		case AndroidDialogResult.NO:
			GooglePlayRTM.Instance.OpenInvitationInBoxUI();
			break;

		}
	}
	
	private void OnInvManageDialogComplete(AndroidDialogResult result) {
		switch(result) {
		case AndroidDialogResult.YES:
			GooglePlayRTM.Instance.AcceptInvitation(inviteId);
			break;
		case AndroidDialogResult.NO:
			GooglePlayRTM.Instance.DeclineInvitation(inviteId);
			break;
		}
	}
	
	/*void ActionInvitationAccepted (GP_Invite invitation) {
		
		Debug.Log("ActionInvitationAccepted called");
		
		if (invitation.InvitationType != GP_InvitationType.INVITATION_TYPE_REAL_TIME) {
			return;
		}
		
		
		Debug.Log("Starting The Game");
		//make sure you have prepared your scene to start the game before you accepting the invite. Room join even will be triggered
		GooglePlayRTM.instance.AcceptInvitation(invitation.Id);
	}
	
	private void OnRoomCreated(GP_GamesStatusCodes code) {
		SA_StatusBar.text = "Room Create Result:  " + code.ToString();
	}* /	
	
	/*private void OnGCDataReceived(GP_RTM_Network_Package package) {
		#if (UNITY_ANDROID && !UNITY_EDITOR ) || SA_DEBUG_MODE
		
		
		System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
		string str = enc.GetString(package.buffer);
		
		
		string name = package.participantId;
		
		
		GP_Participant p =  GooglePlayRTM.instance.currentRoom.GetParticipantById(package.participantId);
		if(p != null) {
			GooglePlayerTemplate player = GooglePlayManager.instance.GetPlayerById(p.playerId);
			if(player != null) {
				name = player.name;
			}
		}
		
		AndroidMessage.Create("Data Eeceived", "player " + name + " \n " + "data: " + str);
		
		#endif
		
	}*/
}
