using UnityEngine;
using System;
using System.Collections;

public class UM_RTM_Participant {

	private string _Id = string.Empty;
	private string _Name = string.Empty;

	private UM_RTM_ParticipantStatus _Status = UM_RTM_ParticipantStatus.STATUS_UNRESPONSIVE;

	private GP_Participant _gpParticipant = null;
	private GK_Player _gkPlayer = null;

	public UM_RTM_Participant(GP_Participant participant) {
		_Id = participant.id;
		_Name = participant.DisplayName;
		_gpParticipant = participant;
		_Status = (UM_RTM_ParticipantStatus)(int)participant.Status;
	}

	public UM_RTM_Participant(GK_Player participant) {
		_Id = participant.Id;
		_Id = participant.DisplayName;
		_gkPlayer = participant;
	}

	public string Id {
		get {
			return _Id;
		}
	}

	public string Name {
		get {
			return _Name;
		}
	}

	public UM_RTM_ParticipantStatus Status {
		get {
			return _Status;
		}
	}

	public Texture2D HighResImage {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				return _gpParticipant.BigPhoto;
			case RuntimePlatform.IPhonePlayer:
				return _gkPlayer.BigPhoto;
			default: return null;
			}
		}
	}

	public Texture2D SmallIcon {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				return _gpParticipant.SmallPhoto;
			case RuntimePlatform.IPhonePlayer:
				return _gkPlayer.SmallPhoto;
			default: return null;
			}
		}
	}
}
