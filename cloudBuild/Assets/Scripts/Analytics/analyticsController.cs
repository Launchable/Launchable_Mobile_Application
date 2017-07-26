using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analyticsController : MonoBehaviour {
	
	string launch_url = "http://13.59.240.48/methods/test";
	string currentTarget;
	float startTime = 0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void addTargetFound(string targetName){
		currentTarget = targetName;
	}
	
	IEnumerator uploadAnalytics(WWWForm uploadData){
		WWW download = new WWW( launch_url, uploadData);

		// Wait until the download is done
		yield return download;

		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			// show the highscores
			Debug.Log(download.text);
		}
	}
	
	public void launchTimeTrack(bool toggle){
		float trackedTime = 0;
		
		if(toggle){
			startTime = Time.time;
		}
		else{
			if(startTime > 0.1f){
				trackedTime = Time.time - startTime;
				WWWForm form = new WWWForm();
				form.AddField("target","" + currentTarget);
				form.AddField("duration",trackedTime.ToString());
				StartCoroutine(uploadAnalytics(form));
			}
		}
	}
	
	public void screenshotTaken(){
		WWWForm form = new WWWForm();
		form.AddField("target","" + currentTarget);
		form.AddField("screenshotTaken","true");
		StartCoroutine(uploadAnalytics(form));
	}
	
	public void screenshotShare(string platform){
		WWWForm form = new WWWForm();
		form.AddField("target","" + currentTarget);
		if(platform == "twitter"){
			form.AddField("share","twitter");
		}
		else {
			form.AddField("share","facebook");
		}
		
		StartCoroutine(uploadAnalytics(form));
	}
}
