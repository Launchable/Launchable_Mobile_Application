using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UM_Storage
{

	public static void Save (string key, string data)
	{
#if UNITY_ANDROID
		AndroidNative.SaveToCacheStorage (key, System.Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (data)));
#else
		SA.IOSNative.Storage.AppCache.Save(key, data);
#endif
	}

	public static void Save (string key, Texture2D texture)
	{
#if UNITY_ANDROID
		AndroidNative.SaveToCacheStorage (key, System.Convert.ToBase64String (texture.EncodeToPNG ()));
#else
		SA.IOSNative.Storage.AppCache.Save(key, texture);
#endif
	}

	public static void Save (string key, byte[] data)
	{
#if UNITY_ANDROID
		AndroidNative.SaveToCacheStorage (key, System.Convert.ToBase64String (data));
#else
		SA.IOSNative.Storage.AppCache.Save(key, data);
#endif
	}

	public static string GetString (string key)
	{
#if UNITY_ANDROID
		return AN_Storage.GetString (key);
#else
		return SA.IOSNative.Storage.AppCache.GetString(key);
#endif
	}

	public static Texture2D GetTexture (string key)
	{
#if UNITY_ANDROID
		return AN_Storage.GetTexture (key);
#else
		return SA.IOSNative.Storage.AppCache.GetTexture(key);
#endif
	}

	public static byte[] GetData (string key)
	{
#if UNITY_ANDROID
		return AN_Storage.GetData (key);
#else
		return SA.IOSNative.Storage.AppCache.GetData(key);
#endif
	}

}
