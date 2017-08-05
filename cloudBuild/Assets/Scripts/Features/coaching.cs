using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefs2
{
	public static void SetBool(string key, bool state)
	{
		PlayerPrefs.SetInt(key, state ? 1 : 0);
	}

	public static bool GetBool(string key)
	{
		return (PlayerPrefs.GetInt(key) == 1);
	}
		
}
