using UnityEngine;
using System.Collections.Generic;

public class UM_RTM_Room {

	private List<UM_RTM_Participant> _Participants = new List<UM_RTM_Participant>();

	public UM_RTM_Room () {

	}

	public UM_RTM_Room (GP_RTM_Room room) {
		foreach (GP_Participant p in room.participants) {
			UM_RTM_Participant participant = new UM_RTM_Participant(p);
			_Participants.Add(participant);
		}
	}

	public UM_RTM_Room (GK_RTM_Match match) {
		foreach (GK_Player player in match.Players) {
			UM_RTM_Participant participant = new UM_RTM_Participant(player);
			_Participants.Add(participant);
		}
	}

	public UM_RTM_Participant GetParticipantById(string id) {
		foreach (UM_RTM_Participant participant in _Participants) {
			if (participant.Id.Equals(id)) {
				return participant;
			}
		}
		return null;
	}

	public List<UM_RTM_Participant> Participants {
		get {
			return _Participants;
		}
	}
}
