using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SA.Analytics.Google {
		
	public class CacheQueue : SA.Common.Pattern.Singleton<CacheQueue> {


		private bool IsWorking = false;
		private CachedRequest _CurrentRequest = null;
		private List<CachedRequest> _CurrentQueue = null;

		public void Run() {



			if (IsWorking) { return; }


			IsWorking = true;
			_CurrentQueue = RequestCache.CurrenCachedRequests;
			if (_CurrentQueue.Count == 0) {
				Stop ();
			} else {
				_CurrentRequest = _CurrentQueue [0];
				StartCoroutine(Send (_CurrentRequest));
			}
		}




		private void Stop() {
			IsWorking = false;
			_CurrentRequest = null;
			_CurrentQueue = null;
		}

		private void Continue() {

			_CurrentQueue.Remove (_CurrentRequest);

			if(_CurrentQueue.Count == 0) {
				RequestCache.Clear ();
				Stop ();
			} else {
				RequestCache.CacheRequests (_CurrentQueue);
				_CurrentRequest = _CurrentQueue [0];
				StartCoroutine(Send (_CurrentRequest));
			}
				
		}



		private IEnumerator Send (CachedRequest request) {
	
			string HitRequest = request.RequestBody;
			if(GA_Settings.Instance.IsQueueTimeEnabled) {
				HitRequest += "&qt" + request.Delay;
			}


			WWW www = Manager.SendSkipCache(HitRequest);

			// Wait for request complete
			yield return www;


			if(www.error != null) {
				Stop ();
			} else {
				yield return new WaitForSeconds(2f);
				Continue ();
			}

			yield return null;
		}

	}

}