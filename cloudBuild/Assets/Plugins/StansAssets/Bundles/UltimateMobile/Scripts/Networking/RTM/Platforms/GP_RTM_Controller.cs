using UnityEngine;
using System;
using System.Collections.Generic;

public class GP_RTM_Controller : iRTM_Matchmaker {

	public event Action<UM_RTM_Invite> InvitationReceived = delegate {};
	public event Action<UM_RTM_Invite> InvitationAccepted = delegate {};
	public event Action<string> InvitationDeclined = delegate {};

	public event Action<UM_RTM_RoomCreatedResult> RoomCreated = delegate {};
	public event Action RoomUpdated = delegate {};

	public event Action<string, byte[]> MatchDataReceived = delegate {};

	private List<UM_RTM_Invite> _Invitations = new List<UM_RTM_Invite>();

	private UM_RTM_Room _CurrentRoom = new UM_RTM_Room();

	public GP_RTM_Controller(){
		GooglePlayRTM.ActionInvitationReceived += HandleActionInvitationReceived;
		GooglePlayRTM.ActionInvitationRemoved += HandleActionInvitationRemoved;
		GooglePlayRTM.ActionInvitationAccepted += HandleActionInvitationAccepted;

		GooglePlayRTM.ActionRoomCreated += HandleActionRoomCreated;
		GooglePlayRTM.ActionDataRecieved += HandleActionMatchDataReceived;
		GooglePlayRTM.ActionRoomUpdated += HandleActionRoomUpdated;

		GooglePlayConnection.ActionPlayerConnected += HandleActionPlayerConnected;
	}

	public void OpenInvitationUI(int minPlayers, int maxPlayers) {
		GooglePlayRTM.Instance.OpenInvitationBoxUI(minPlayers, maxPlayers);
	}

	public void AcceptInvite(UM_RTM_Invite invite) {
		GooglePlayRTM.Instance.AcceptInvitation(invite.Id);
	}

	public void DeclineInvite(UM_RTM_Invite invite) {
		GooglePlayRTM.Instance.DeclineInvitation(invite.Id);
	}
	
	public void FindMatch (int minPlayers, int maxPlayers) {
		GooglePlayRTM.Instance.FindMatch(minPlayers, maxPlayers);
	}

	public void SendDataToAll(byte[] data, UM_RTM_PackageType type) {
		GooglePlayRTM.Instance.SendDataToAll(data, type.GetGPPackageType());
	}

	public void SendDataToPlayer(byte[] data, UM_RTM_PackageType type, params string[] receivers) {
		GooglePlayRTM.Instance.SendDataToPlayers(data, type.GetGPPackageType(), receivers);
	}

	public void LeaveMatch() {
		GooglePlayRTM.Instance.LeaveRoom();
	}

	private void HandleActionRoomUpdated (GP_RTM_Room room) {
		_CurrentRoom = new UM_RTM_Room(room);
		RoomUpdated();
	}

	private void HandleActionMatchDataReceived(GP_RTM_Network_Package package) {
		MatchDataReceived(package.participantId, package.buffer);
	}

	private void HandleActionRoomCreated(GP_GamesStatusCodes status) {
		UM_RTM_RoomCreatedResult result = new UM_RTM_RoomCreatedResult(status);

		_CurrentRoom = new UM_RTM_Room(GooglePlayRTM.Instance.currentRoom);
		RoomCreated(result);
	}

	private void HandleActionPlayerConnected ()
	{
		GooglePlayInvitationManager.Instance.RegisterInvitationListener();
	}

	private void HandleActionInvitationReceived(GP_Invite invite) {

		UM_RTM_Invite inv = new UM_RTM_Invite(invite);
		_Invitations.Add(inv);

		InvitationReceived(inv);
	}

	private void HandleActionInvitationRemoved(string id) {
		RemoveInvitation(id);
		InvitationDeclined(id);
	}

	private void HandleActionInvitationAccepted(GP_Invite invite) {
		if (invite.InvitationType != GP_InvitationType.INVITATION_TYPE_REAL_TIME) {
			return;
		}

		UM_RTM_Invite inv = null;
		if (!TryGetInvitation(invite.Id, out inv)) {
			inv = new UM_RTM_Invite(invite);
			_Invitations.Add(inv);
		}
		InvitationAccepted(inv);
	}

	private bool TryGetInvitation(string id, out UM_RTM_Invite invite) {
		invite = null;
		foreach (UM_RTM_Invite inv in _Invitations) {
			if (inv.Id.Equals(id)) {
				invite = inv;
				return true;
			}
		}
		return false;
	}

	private void RemoveInvitation(string id) {
		foreach (UM_RTM_Invite invite in _Invitations) {
			if (invite.Id.Equals(id)) {
				_Invitations.Remove(invite);
				return;
			}
		}
	}
	
	public List<UM_RTM_Invite> Invitations {
		get {
			return _Invitations;
		}
	}

	public UM_RTM_Room CurrentRoom {
		get {
			return _CurrentRoom;
		}
	}
}
