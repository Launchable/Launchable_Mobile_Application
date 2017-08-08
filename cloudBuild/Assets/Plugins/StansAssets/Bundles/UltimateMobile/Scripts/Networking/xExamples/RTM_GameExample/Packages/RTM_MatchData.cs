using UnityEngine;
using System.Collections;

public class RTM_MatchData : MNT_NetworkPackage {

	public int _DataValue = 1;

	public RTM_MatchData() : base(1) {

	}

	public RTM_MatchData(byte[] data) : base(data) {
		_DataValue = ReadValue<int>();
	}

	public void SetData(int data) {
		_DataValue = data;
	}

	public void Save() {
		WriteValue<int>(_DataValue);
	}

}
