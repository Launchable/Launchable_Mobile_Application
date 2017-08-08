////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SA.Analytics.Google {

	public class RequestCache  {

		private const string DATA_SPLITTER = "|";
		private const string RQUEST_DATA_SPLITTER = "%rps%";

		private const string GA_DATA_CACHE_KEY = "GoogleAnalyticsRequestCache";

		public static void SaveRequest(string cache) {

			if(!GA_Settings.Instance.IsRequetsCachingEnabled) { return; }

			CachedRequest r = new CachedRequest(cache, DateTime.Now.Ticks);

			List<CachedRequest> current = CurrenCachedRequests;
			current.Add(r);
			CacheRequests(current);

		}

		public static void SendCachedRequests() {
			CacheQueue.Instance.Run ();
		}


		public static void Clear() {
			PlayerPrefs.DeleteKey(GA_DATA_CACHE_KEY);
		}

		public static string SavedData {
			get {
				if(PlayerPrefs.HasKey(GA_DATA_CACHE_KEY)) {
					return PlayerPrefs.GetString(GA_DATA_CACHE_KEY);
				} else {
					return string.Empty;
				}
			}

			set {
				PlayerPrefs.SetString(GA_DATA_CACHE_KEY, value);
			}
		}

		public static void CacheRequests(List<CachedRequest> requests) {
			List<List<string>> cache =  new List<List<string>>();

			foreach(CachedRequest r  in requests) { 
				List<string> data =  new List<string>();
				data.Add(r.RequestBody);
				data.Add(r.TimeCreated.ToString());

				cache.Add(data);
			}

			SavedData =  SA.Common.Data.Json.Serialize(cache);
		}

		public static List<CachedRequest> CurrenCachedRequests {
			get {
				if(SavedData == string.Empty) {
					return new List<CachedRequest>();
				} else {
					try {
						List<CachedRequest> current =  new List<CachedRequest>();
						List<object> requests  =   SA.Common.Data.Json.Deserialize(SavedData) as List<object>;
						foreach(object request in requests) {

							
							List<object> dataList = request as List<object>;
							CachedRequest r =  new CachedRequest();
							int index = 1;
							foreach(object d in dataList) {
								string val = d as String;
								switch(index) {
								case 1:
									r.RequestBody = val;
									break;
								case 2:
									r.TimeCreated = Convert.ToInt64(val);
									break;
								}

								index++;
							}

							current.Add(r);
						}

						return current;

					} catch(Exception ex) {
						Clear();
						Debug.LogError(ex.Message);
						return new List<CachedRequest>();
					}
				}
			}
		}


	}

}
