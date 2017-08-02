using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using monoflow;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;
using System;

public class metadataParse : MonoBehaviour {
	
	//in the dashboard, they'll have the option to put in contact info. 
	public string emailContact = "none";
	public string webContact = "none";
	public string phoneContact = "none";

	//gameObjects for the contact card.
	Text estateTitle;
	Text estateBody;
	Image estatePicture;
	
	//debugging
	float startTime = 0f;
	public Text debugText;

	
	string streamPath; //path to save medatada in phone 
	string targetName; 
	bool estateCard = false; //check if we should have contact card functionality
	
	analyticsController analyticsControl;
	
	// unity video player objects
	private RawImage videoTex;
	private GameObject videoGameObject;
	private VideoPlayer videoPlayer;
	private VideoSource videoSource;
	private AudioSource audioSource;
	
	void Start(){
		//local path for phone
		streamPath = Application.persistentDataPath + "/";
	#if UNITY_IOS
		streamPath += "Library/Application Support/";
	#endif
		analyticsControl = GameObject.FindObjectOfType<analyticsController> ();
	}
	
	//gets target name from defaultTrackableEventHandler and finds the metadata file
	public void loadMetadata(string metadataFile){
		//downloaded metadata path
		targetName = metadataFile;
		string metadataPath = streamPath + metadataFile+".txt";
		StreamReader file =  new StreamReader(metadataPath);
		parseData(file);
	}
	
	//goes line by line in metadata and
	void parseData (StreamReader metadataFile) {
		//find cloned objects
		estateTitle = transform.Find("Canvas/estateCard/title").GetComponent<Text>();
		estateBody = transform.Find("Canvas/estateCard/body").GetComponent<Text>();
		estatePicture = transform.Find("Canvas/picture").GetComponent<Image>();

		
		//find the rawimage object for video
		videoGameObject = transform.Find ("videoCanvas/videoTexture").gameObject;
		videoTex = videoGameObject.GetComponent<RawImage> ();

		//go through metadata file line per line
		string line;
		while((line = metadataFile.ReadLine()) != null){
			executeLineCommand(line);
		}
		
		//estateCard shows a contact card with info in it. Currently deprecated but can be used later 
		if(estateCard){	
			transform.Find("Canvas").GetComponent<CanvasGroup>().alpha =1.0f;
			transform.Find("Canvas").localScale = new Vector3(.00858262f,.00858262f,.00858262f);

		}else{
			transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0.0f;
		}
		
		metadataFile.Close();
	}

	//read and execute line
	void executeLineCommand(string line){
		//an example of a split line would look like this {'videoURL', 'www.video.com/firstVideo.mp4'}
		string[] splitMetadata = line.Split(' ');
		
		if(splitMetadata[1].StartsWith("false")) return;
		
		switch (splitMetadata[0])
		{
		  case "name":
			analyticsControl.addTargetFound(targetName + " - " + splitMetadata[1]);
			break;
          case "videoUrl":
			StartCoroutine(splitMetadata [1]);
            break;
		  case "videoTranslateX":
		    //translate video according to metadata
			videoGameObject.transform.position = new Vector3(Convert.ToSingle(splitMetadata[1]),0.0f,0.0f);
			break;
		  case "videoTranslateY":
			//translate video according to metadata
			videoGameObject.transform.position = new Vector3(0.0f,0.0f,Convert.ToSingle(splitMetadata[1]));
			break;
		  case "videoResize":
			//scale video according to metadata
			videoGameObject.transform.localScale = new Vector3(Convert.ToSingle(splitMetadata[1]),Convert.ToSingle(splitMetadata[1]),Convert.ToSingle(splitMetadata[1]));
			break;
          case "3durl":
            load3dAsset(splitMetadata[1]);
            break;
          case "estateCard":
            estateCard = false; //to activate contact card functionality this should be changed to true.
            break;
		default:
			break;
		}
		
		//update contact card if enabled
		if(estateCard){
			switch(splitMetadata[0])
			{
			  case "estateCardTitle":
				estateTitle.text = line.Replace(splitMetadata[0],"");
				break;
			  case "estateCardDescription":
				estateBody.text = line.Replace(splitMetadata[0],"");
				break;
			  case "estateImageUrl":
				loadImage(splitMetadata[1]);
				break;
			  case "estatePhone":
				phoneContact = splitMetadata[1];
				break;
			  case "estateEmail":
				emailContact = splitMetadata[1];
				break;
			  case "estateWebsite":
				webContact = splitMetadata[1];
				break;
			default:
				break;
			}
		}

	}
	
	// start streaming the video from url link
	IEnumerator playVideo(string url)
	{
		//start debugging time
		launchTimeTrack(true, "");

		//Add VideoPlayer to the GameObject
		videoPlayer = gameObject.AddComponent<VideoPlayer>();

		//Add AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();

		//Disable Play on Awake for both Video and Audio
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		audioSource.Pause();

		//We want to play from video clip not from url
		videoPlayer.source = VideoSource.VideoClip;
		// Video clip from Url
		videoPlayer.source = VideoSource.Url;


		videoPlayer.url = url;

		//Set Audio Output to AudioSource
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		videoPlayer.EnableAudioTrack(0, true);
		videoPlayer.SetTargetAudioSource(0, audioSource);
		launchTimeTrack(false, "Setting up video components takes ");
		launchTimeTrack(true, "");
		//Set video To Play then prepare Audio to prevent Buffering
//		videoPlayer.clip = videoToPlay;
		videoPlayer.Prepare();

		//Wait until video is prepared
		WaitForSeconds waitTime = new WaitForSeconds(1);

		while (!videoPlayer.isPrepared)
		{
			Debug.Log("Preparing Video");
			//Prepare/Wait for 5 sceonds only
			yield return waitTime;
			//Break out of the while loop after 5 seconds wait
			break;
		}
		launchTimeTrack(false, "Preparing video takes ");
		launchTimeTrack(true, "");
		Debug.Log("Done Preparing Video");

		//Assign the Texture from Video to RawImage to be displayed
		videoTex.texture = videoPlayer.texture;
	
		//Play Video
		videoPlayer.Play();
		//Play Sound
		audioSource.Play();
		launchTimeTrack(false, "Starting video takes ");

		Debug.Log("Playing Video");
		while (videoPlayer.isPlaying)
		{
			//Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
			yield return null;
		}

		Debug.Log("Done Playing Video");
	}
	
	void load3dAsset(string url){
		StartCoroutine(GetAssetBundle(url));
	}
	
	void loadImage(string url){
		StartCoroutine(GetImage(url));
	}
	
	//Download Asset bundle. For this to work with any asset we have to get the asset name from metadata file or filename
    IEnumerator GetAssetBundle(string url) {
        
		UnityWebRequest www = UnityWebRequest.GetAssetBundle(url);
		yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
			AssetBundle bundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
			Instantiate(bundle.LoadAsset("Maze_1"),transform);//hardcoded for now
			
		}
    }
	
	//downloads image and puts it on contact card sprite
    IEnumerator GetImage(string url) {
		WWW www = new WWW(url);
		yield return www;
		estatePicture.sprite =  Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }	
	
	public void resetCard(){
		transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0.0f;
	}
	
	//time tracking. toggle true means start time, false is stop and print
	public void launchTimeTrack(bool toggle, string tracking){	
		float trackedTime;	
		if(toggle){
			startTime = Time.time;
		}
		else{
			trackedTime = Time.time - startTime;
			debugText.text += tracking + trackedTime.ToString() + " seconds \n";
		}

	}
}
