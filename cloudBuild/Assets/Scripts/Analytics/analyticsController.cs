using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analyticsController : MonoBehaviour {
	
	string launch_url = "http://launchableURLhere.com/index.php/LaunchableApp/Launches";
	string currentTarget;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void addAnalytic(string targetName){
		currentTarget = targetName;
		StartCoroutine("targetLaunched");
	}
	
	IEnumerator targetLaunched(){
		WWW download = new WWW( launch_url+"/card"+currentTarget);

		// Wait until the download is done
		yield return download;

		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			// show the highscores
			Debug.Log(download.text);
		}
	}
}
