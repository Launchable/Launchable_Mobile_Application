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
	public GameObject phoneButton;
	public GameObject emailButton;
	
	//debugging
	float startTime = 0f;
	public Text debugText;
	
	string streamPath; //path to save target info locally 
	string targetName; 

	analyticsController analyticsControl;
	
	// unity video player objects
	private RawImage videoTex;
	private GameObject videoGameObject;
	public VideoPlayer videoPlayer;
	private VideoSource videoSource;
	public AudioSource audioSource;

//	public GameObject playIcon;
	private bool isPaused = false;
	public bool firstRun = true;
	private string videoLink;

	void Start(){
		//local path for phone
		streamPath = Application.persistentDataPath + "/";
	#if UNITY_IOS
		streamPath += "Library/Application Support/";
	#endif
		analyticsControl = GameObject.FindObjectOfType<analyticsController> ();
		//Add VideoPlayer to the GameObject
		videoPlayer = gameObject.AddComponent<VideoPlayer>();

		//Add AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();
		
		//find the rawimage object for video
		videoGameObject = transform.Find ("videoCanvas/videoTexture").gameObject;
		videoTex = videoGameObject.GetComponent<RawImage> ();
	}
	
	//gets target name from defaultTrackableEventHandler and finds the metadata file
	public void loadMetadata(string metadataFile){
		targetName = metadataFile;
		string metadataPath = streamPath + metadataFile+".txt";
		StreamReader file =  new StreamReader(metadataPath);
		parseData(file);

	}
	
	//goes line by line in metadata and
	void parseData (StreamReader metadataFile) {

		
		//go through metadata file line per line
		string line;
		while((line = metadataFile.ReadLine()) != null)
		{
			executeLineCommand(line);
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
			videoGameObject.transform.localScale = new Vector3(1f,1f,1f);
			loadVideo(splitMetadata[1]);
			break;
		case "videoTranslateX":
			//translate video according to metadata
			videoGameObject.transform.position = new Vector3(Convert.ToSingle(splitMetadata[1]),0.0f,0.0f);			break;
			break;
		case "videoTranslateY":
			//translate video according to metadata
			videoGameObject.transform.position = new Vector3(0.0f,0.0f,Convert.ToSingle(splitMetadata[1]));			break;
			break;
		case "videoResize":
			//scale video according to metadata
			videoGameObject.transform.localScale = new Vector3(Convert.ToSingle(splitMetadata[1]),Convert.ToSingle(splitMetadata[1]),Convert.ToSingle(splitMetadata[1]));			break;
			break;
		case "3durl":
			load3dAsset(splitMetadata[1]);
			break;
		case "estatePhone":
			updatePhone(splitMetadata[1]);
			break;
		case "estateEmail":
			updateEmail(splitMetadata[1]);
			break;
		default:
			break;
		}

	}
		
	void loadVideo(string url){
		StartCoroutine(playVideo(url));
	}
	
	void updatePhone(string phoneNumber){
		phoneContact = phoneNumber;
		phoneButton.GetComponent<Animator> ().Play ("phoneAnimation", -1, 0f);

	}
	
	void updateEmail(string email){
		emailContact = email;
		emailButton.GetComponent<Animator> ().Play ("emailAnimation", -1, 0f);
	}
	
	// start streaming the video from url link
	IEnumerator playVideo(string url){

		//playIcon.gameObject.SetActive (true);
		firstRun = false;

		//Disable Play on Awake for both Video and Audio
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		audioSource.Pause();

		// Video clip from Url
		videoPlayer.source = VideoSource.Url;
		videoPlayer.url = url;

		//Set Audio Output to AudioSource
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		videoPlayer.EnableAudioTrack(0, true);
		videoPlayer.SetTargetAudioSource(0, audioSource);

		//Set video To Play then prepare Audio to prevent Buffering
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


		Debug.Log("Done Preparing Video");

		//Assign the Texture from Video to RawImage to be displayed
		videoTex.texture = videoPlayer.texture;
	
		//Play Video
		videoPlayer.Play();
		//Play Sound
		audioSource.Play();
	}

//	public void playPause() {
//		if (!firstRun && !isPaused) {
//			videoPlayer.Pause ();
//			audioSource.Pause ();
//			playIcon.SetActive (true);
//			isPaused = true;
//		} else if (!firstRun && isPaused) {
//			videoPlayer.Play ();
//			audioSource.Play ();
//			playIcon.SetActive (false);
//			isPaused = false;
//		} else {
//			StartCoroutine (playVideo ());
//		}
//	}
		
	
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
			Instantiate(bundle.LoadAsset("Maze_1"),transform);//hardcoded asset name for now
			
		}
    }
	
    IEnumerator GetImage(string url) {
		WWW www = new WWW(url);
		yield return www;
		//estatePicture.sprite =  Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }	
	
	//time tracking. toggle true means start time, false is stop and print
	public void resetCard(){
		videoGameObject.transform.localScale = new Vector3(0f,0f,0f);
	}
	
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
