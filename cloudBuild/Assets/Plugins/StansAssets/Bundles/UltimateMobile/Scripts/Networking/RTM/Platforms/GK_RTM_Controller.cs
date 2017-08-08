using UnityEngine;
using System;
using System.Collections.Generic;

public class GK_RTM_Controller : iRTM_Matchmaker {

	#pragma warning disable 0067
	public event Action<UM_RTM_Invite> InvitationReceived = delegate {};
	public event Action<UM_RTM_Invite> InvitationAccepted = delegate {};
	public event Action<string> InvitationDeclined = delegate {};

	public event Action<UM_RTM_RoomCreatedResult> RoomCreated = delegate {};
	public event Action RoomUpdated = delegate {};

	public event Action<string, byte[]> MatchDataReceived = delegate {};

	private List<UM_RTM_Invite> _Invitations = new List<UM_RTM_Invite>();

	private UM_RTM_Room _CurrentRoom = new UM_RTM_Room();

	public GK_RTM_Controller() {
		GameCenterInvitations.ActionPlayerAcceptedInvitation += HandleActionPlayerAcceptedInvitation;

		GameCenter_RTM.ActionMatchStarted += HandleActionRoomCreated;
		GameCenter_RTM.ActionDataReceived += HandleActionMatchDataReceived;
	}

	private void HandleActionRoomCreated (GK_RTM_MatchStartedResult result) {
		if (result.IsSucceeded) {
			_CurrentRoom = new UM_RTM_Room(result.Match);
		}

		UM_RTM_RoomCreatedResult res = new UM_RTM_RoomCreatedResult(result);
		RoomCreated(res);
	}

	private void HandleActionMatchDataReceived (GK_Player sender, byte[] data)
	{
		MatchDataReceived(sender.Id, data);
	}

	private void HandleActionPlayerAcceptedInvitation (GK_MatchType type, GK_Invite invite) {
		if (type != GK_MatchType.RealTime) {
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

	public void OpenInvitationUI(int minPlayers, int maxPlayers) {
		GameCenter_RTM.Instance.FindMatchWithNativeUI(minPlayers, maxPlayers);
	}

	public void AcceptInvite(UM_RTM_Invite invite) {
		//TODO: GameCenter_RTM.Instance.StartMatchWithInvite(invite.Id, false);
	}

	public void DeclineInvite(UM_RTM_Invite invite) {
	}
	
	public void FindMatch (int minPlayers, int maxPlayers) {
		GameCenter_RTM.Instance.FindMatch(minPlayers, maxPlayers);
	}

	public void SendDataToAll(byte[] data, UM_RTM_PackageType type) {
		GameCenter_RTM.Instance.SendData(data, type.GetGKPackageType());
	}
	
	public void SendDataToPlayer(byte[] data, UM_RTM_PackageType type, params string[] receivers) {
		//GameCenter_RTM.Instance.SendData(data, type.GetGKPackageType(), receivers);
	}
	
	public void LeaveMatch() {
		GameCenter_RTM.Instance.Disconnect();
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
