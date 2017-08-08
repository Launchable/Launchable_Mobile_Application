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

public class AMN_PlayerData : AMN_Singleton<AMN_PlayerData>  {
	
	private const string ENTITLEMENTS = "ENTITLEMENTS";
	
	public const string DATA_SPLITTER = "|";
	
	void Awake() {
		DontDestroyOnLoad (gameObject);
	}
	
	public static void AddNewSKU (string SKU) {
		string entitlements;
		
		if (PlayerPrefs.HasKey (ENTITLEMENTS)) {
			entitlements = PlayerPrefs.GetString(ENTITLEMENTS);
			
			entitlements += SKU + DATA_SPLITTER;
		}
		else {
			entitlements = SKU + DATA_SPLITTER;
		}
		
		
		PlayerPrefs.SetString(ENTITLEMENTS, entitlements);
	}
	
	public static List<string> GetAvailableSKUs () {
		List<string> list = new List<string> ();
		
		if (PlayerPrefs.HasKey (ENTITLEMENTS)) {
			string entitlements = PlayerPrefs.GetString(ENTITLEMENTS);
			
			string[] storeData = entitlements.Split(DATA_SPLITTER[0]);
			
			for(int i = 0; i < storeData.Length; i++) {
				list.Add(storeData[i]);
			}
		}
		
		return list;
	}
}
