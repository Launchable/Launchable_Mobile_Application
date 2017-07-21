using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analyticsController : MonoBehaviour {
	
	string launch_url = "http://launchableURLhere.com/index.php/LaunchableApp/Launches";
	string currentTarget;
	float startTime;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void addTargetFound(string targetName){
		currentTarget = targetName;
		StartCoroutine("targetLaunched");
	}
	
	IEnumerator targetLaunched(){
		print("<color=green>Instance of Target " + currentTarget + " added to analytics</color>");
		//WWW download = new WWW( launch_url+"/card"+currentTarget);

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
			trackedTime = Time.time - startTime;
			print("<color=green>Tracking Time for " + currentTarget + "is " + trackedTime + " seconds</color>");
		}
	}
	
}
