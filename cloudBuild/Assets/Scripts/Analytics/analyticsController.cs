using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analyticsController : MonoBehaviour {
	
	string launch_url = "http://launchableURLhere.com/index.php/LaunchableApp/Launches";
	string currentTarget;
	float startTime = 0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void addTargetFound(string targetName){
		currentTarget = targetName;
		string url = launch_url+"/card"+currentTarget;
		StartCoroutine(uploadAnalytics(url));
	}
	
	IEnumerator uploadAnalytics(string url){
		print("<color=green>Analytics added: " + url + "</color>");
		//WWW download = new WWW( url);

		// Wait until the download is done
		yield return null;//download;
/*
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			// show the highscores
			Debug.Log(download.text);
		}*/
	}
	
	public void launchTimeTrack(bool toggle){
		float trackedTime = 0;
		
		if(toggle){
			startTime = Time.time;
		}
		else{
			if(startTime > 0.1f){
				trackedTime = Time.time - startTime;
				string url = launch_url+"/track"+currentTarget+trackedTime.ToString();
				StartCoroutine(uploadAnalytics(url));
			}
		}
	}
	
	public void screenshotTaken(){
		string url = launch_url+"/screenshot"+currentTarget;
		StartCoroutine(uploadAnalytics(url));
	}
	
	public void screenshotShare(string platform){
		string url;
		if(platform == "twitter"){
			url = launch_url+"/screenshot"+platform + currentTarget;
		}
		else url = launch_url+"/screenshot"+platform + currentTarget;
			
		StartCoroutine(uploadAnalytics(url));
	}
}
