using System.Collections;
using System.Collections.Generic;
using monoflow;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;

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
	//path to save target info locally 
	string streamPath; 
	string targetName; 

	analyticsController analyticsControl;
	
	// unity video player objects
    [SerializeField]
	private RawImage videoTex;
    [SerializeField]
    float fadeDuration = 0.25f;
    private RawImage videoTex_preview;
    private GameObject videoGameObject;
    private GameObject videoGameObject_preview;
    private string videoLink;
    private string videoFolderLink;
    // access video player anywhere
    public VideoPlayer videoPlayer;
	public AudioSource audioSource;
    // access preview video player anywhere
    public VideoPlayer videoPlayer_preview;
    public AudioSource audioSource_preview;
    
	// buttons for video player
    public Image playIcon;
	public Image pauseIcon;


	public bool isPaused = false;
	public bool firstRun = true;

	public bool pauseVideo = false;


	void Start(){
		//local path for phone
		streamPath = Application.persistentDataPath + "/";
	#if UNITY_IOS
		streamPath += "Library/Application Support/";
	#endif
		analyticsControl = GameObject.FindObjectOfType<analyticsController> ();
		
		//find the rawimage object for video
		videoGameObject = transform.Find ("videoCanvas/videoTexture").gameObject;
		videoTex = videoGameObject.GetComponent<RawImage> ();
        videoTex.canvasRenderer.SetAlpha(0.0f);
        videoGameObject_preview = transform.Find("videoCanvas_preview/preview").gameObject;
        videoTex_preview = videoGameObject_preview.GetComponent<RawImage>();

        pauseIcon.gameObject.SetActive(true);
    }
	
	//gets target name from defaultTrackableEventHandler and finds the metadata file
	public void loadMetadata(string metadataFile){
        debugText.text += "Waiting for freeze...start loadMetaData @ time: " + Time.time + "\n";
        targetName = metadataFile;
		string metadataPath = streamPath + metadataFile+".txt";
        debugText.text += "Waiting for freeze...before stream @ time: " + Time.time + "\n";
        StreamReader file =  new StreamReader(metadataPath);
        debugText.text += "Waiting for freeze...after stream @ time: " + Time.time + "\n";
        parseData(file);

	}
	
	//goes line by line in metadata and
	void parseData (StreamReader metadataFile) {

        debugText.text += "Waiting for freeze...start parsing data @ time: " + Time.time + "\n";
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
			videoGameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			// store video link and start streaming the video in the unity videoplayer
			videoLink = splitMetadata [1];
            debugText.text += "Waiting for freeze...before coroutine @ time: " + Time.time + "\n";
            StartCoroutine(playVideo ());
			break;
        case "videoPreviewUrl":
            videoGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            videoGameObject_preview.transform.localScale = new Vector3(1f, 1f, 1f);
            // store video link and start streaming the video in the unity videoplayer
            videoFolderLink = splitMetadata[1];
            StartCoroutine(playPreviewVideo());
            break;
        case "videoTranslateX":
			//translate video according to metadata
			videoGameObject.transform.position = new Vector3(Convert.ToSingle(splitMetadata[1]),0.0f,0.0f);
            videoGameObject_preview.transform.position = new Vector3(Convert.ToSingle(splitMetadata[1]),0.0f,0.0f);
			break;
		case "videoTranslateY":
			//translate video according to metadata
			videoGameObject.transform.position = new Vector3(0.0f,0.0f,Convert.ToSingle(splitMetadata[1]));
            videoGameObject_preview.transform.position = new Vector3(0.0f, 0.0f, Convert.ToSingle(splitMetadata[1]));
            break;
		case "videoResize":
			//scale video according to metadata
			videoGameObject.transform.localScale = new Vector3(Convert.ToSingle(splitMetadata[1]),Convert.ToSingle(splitMetadata[1]),Convert.ToSingle(splitMetadata[1]));
                videoGameObject_preview.transform.localScale = new Vector3(Convert.ToSingle(splitMetadata[1]), Convert.ToSingle(splitMetadata[1]), Convert.ToSingle(splitMetadata[1]));
            break;
		case "3durl":
            if(splitMetadata[1] != "none")
            { 
			load3dAsset(splitMetadata[1]);
            }
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
		
	
	void updatePhone(string phoneNumber){
		phoneContact = phoneNumber;
	}
	
	void updateEmail(string email){
		emailContact = email;
	}

    public void runContactAnimation()
    {
        if (emailContact != "null")
        {
            emailButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            emailButton.GetComponent<Animator>().Play("emailInOut", -1, 0f);
        }
        if (phoneContact != "null")
        {
            phoneButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            phoneButton.GetComponent<Animator>().Play("phoneInOut", -1, 0f);
        }

    }
    public void resetContactButtons(){
		emailButton.transform.localScale = new Vector3(0.0f,1.0f,1.0f);
		phoneButton.transform.localScale = new Vector3(0.0f,1.0f,1.0f);

	}

	// start streaming the video from url link
	IEnumerator playVideo()
	{

		firstRun = false;

		//Add VideoPlayer to the GameObject
		videoPlayer = gameObject.AddComponent<VideoPlayer>();

		//Add AudioSource
		audioSource = gameObject.AddComponent<AudioSource>();

		//Disable Play on Awake for both Video and Audio
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		audioSource.Pause();

        // Video clip from Url
        videoPlayer.source = VideoSource.Url;
		videoPlayer.url = videoLink;

		//Set Audio Output to AudioSource
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		videoPlayer.EnableAudioTrack(0, true);
		videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.Prepare();
        //return null;
        
        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);

		while (!videoPlayer.isPrepared)
		{
			Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
			//Break out of the while loop after 5 seconds wait
			//break;
		}

        debugText.text += "Waiting for freeze...4 @ time: " + Time.time + "\n";

        Debug.Log("Done Preparing Video");
        Debug.Log("Color test");


        //Assign the Texture from Video to RawImage to be displayed
        videoTex.texture = videoPlayer.texture;

        debugText.text += "Waiting for freeze...5 @ time: " + Time.time + "\n";


        //Play Video
        videoPlayer.Play();
        videoTex.CrossFadeAlpha(1, fadeDuration, false);

        debugText.text += "Waiting for freeze...6 @ time: " + Time.time + "\n";


        //Play Sounds
        audioSource.Play();
        Debug.Log("Playing the video");
        
    }

    // start streaming the video from url link
    IEnumerator playPreviewVideo()
    {
//        pauseIcon.gameObject.SetActive(false);
//        playIcon.gameObject.SetActive(false);

        //firstRun = false;

        //Add VideoPlayers to the GameObject
        videoPlayer_preview = transform.Find("videoCanvas_preview/preview").gameObject.AddComponent<VideoPlayer>();
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource_preview = transform.Find("videoCanvas_preview/preview").gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer_preview.playOnAwake = false;
        videoPlayer.playOnAwake = false;
        audioSource_preview.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();
        audioSource_preview.Pause();

        // Video clips from Url
        videoPlayer_preview.source = VideoSource.Url;
        videoPlayer_preview.url = videoFolderLink + "10.mp4";
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoFolderLink + "full.mp4";

        //Set Audio Output to AudioSource
        videoPlayer_preview.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer_preview.EnableAudioTrack(0, true);
        //videoPlayer.EnableAudioTrack(0, true);
        videoPlayer_preview.SetTargetAudioSource(0, audioSource_preview);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer_preview.Prepare();
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += SwitchToFull;

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);

        while (!videoPlayer_preview.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            //break;
        }

        videoGameObject.transform.position = Vector3.zero;
        videoGameObject.transform.localScale = Vector3.zero;

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        videoTex_preview.texture = videoPlayer_preview.texture;

        //Play Video
        videoPlayer_preview.Play();

        //Play Sounds
        audioSource_preview.Play();
        Debug.Log("Playing the video @ " + Time.time);

    }

    private void SwitchToFull(VideoPlayer source)
    {
        videoGameObject.transform.position = videoGameObject_preview.transform.position;
        videoGameObject.transform.localScale = videoGameObject_preview.transform.localScale;
        videoGameObject_preview.transform.localScale = Vector3.zero;
        //throw new NotImplementedException();
        Debug.Log("Video Switched @ " + Time.time);
        videoPlayer.time = videoPlayer_preview.time;

        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Play();
        videoTex.texture = videoPlayer.texture;
        
        
        audioSource.Play();
        //Stop Preview
        videoPlayer_preview.Stop();
        audioSource_preview.Stop();
    }

	public void videoState()
	{

        tapToFocus.OnClicked += videoStateHandler;

		
	}

    public void videoStateHandler()
    {
        pauseVideo = !pauseVideo;
        if (pauseVideo == true)
        {
            videoPlayer.Pause();
            audioSource.Pause();
            //			playPauseIcon.sprite = pauseIcon;
            playIcon.gameObject.SetActive(true);
            pauseIcon.gameObject.SetActive(false);
        }
        else
        {
            videoPlayer.Play();
            audioSource.Play();
            playIcon.gameObject.SetActive(false);
            pauseIcon.gameObject.SetActive(true);
        }
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
