using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analyticsController : MonoBehaviour {
	
	string launch_url = "http://launchableURLhere.com/index.php/LaunchableApp/Launches";
	string currentTarget;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void addTargetFound(string targetName){
		currentTarget = targetName;
		StartCoroutine("targetLaunched");
	}
	
	IEnumerator targetLaunched(){
		print("Instance of Target " + currentTarget + " added to analytics");
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
}
