using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class videoController : MonoBehaviour {

	public VideoPlayer video;
	public Slider slider;

	// properties of the video player
	bool isDone;

	public bool IsPlaying{
		get{ return video.isPlaying; }
	}

	public bool IsLooping{
		get{ return video.isLooping; }
	}

	// you can't play video until this is true
	public bool IsPrepared{
		get{ return video.isPrepared;}
	}

	public bool IsDone{
		get{ return isDone; }
	}

	// current time in the video in second
	public double Time{
		get{ return video.time; }
	}

	// the number of frame/frame per second = second 
	public ulong Duration{
		get{ return (ulong)(video.frameCount / video.frameRate);}
	}

	// the time of our video from 0 to any
	// return no less than 0 and no bigger than our druation of the video
	// this is going use for the slider
	public double NTime{
		get{ return Time / Duration; }
	}
		
	void OnEnable(){
		// what error is the player encounter
		video.errorReceived += errorReceived;
		// everytime the video fram is ready
		video.frameReady += frameReady;
		// is the video reach to the end point
		video.loopPointReached += loopPointReached;
		// let us know if we ready to call the function for video such play, pause...
		video.prepareCompleted += prepareCompleted;
		// move on a jump bar (not so sure what this does)
		video.seekCompleted += seekCompleted;
		// if the video have start it
		video.started += started;

	}

	void OnDisable(){
		// what error is the player encounter
		video.errorReceived -= errorReceived;
		// everytime the video fram is ready
		video.frameReady -= frameReady;
		// is the video reach to the end point
		video.loopPointReached -= loopPointReached;
		// let us know if we ready to call the function for video such play, pause...
		video.prepareCompleted -= prepareCompleted;
		// move on a jump bar (not so sure what this does)
		video.seekCompleted -= seekCompleted;
		// if the video have start it
		video.started -= started;

	}

	// functions for debuging video player
	void errorReceived(VideoPlayer v, string msg){
		Debug.Log ("video player eero: " + msg);
	}

	// how many frame in the video
	void frameReady(VideoPlayer v, long frame){

	}

	void loopPointReached(VideoPlayer v){
		Debug.Log ("video player loop point reached");
		isDone = true;
	}

	// new video is prepared
	void prepareCompleted(VideoPlayer v){
		Debug.Log ("video player finished prepareing");
		isDone = false;
	}

	void seekCompleted(VideoPlayer v){
		Debug.Log ("video player finished seeking");
		isDone = false;
	}

	void started(VideoPlayer v){
		Debug.Log ("video player finished started");
	}

	void Update()
	{
		if (!IsPrepared)
			return;
		slider.value = (float)NTime;
	}

	// the "name" is the actually name of the video not the path
	public void LoadVideo(string name){
		// this is the path of the video located: asset/Videos/name.mp4
		string tmp = Application.dataPath + "/Resources/videos/" + name; /*.mp4, .avi, .mov*/
		if (video.url == tmp)
			return;
		video.url = tmp;
		video.Prepare ();

		Debug.Log ("can set direct audio volume: " + video.canSetDirectAudioVolume);
		Debug.Log ("can set playback speed: " + video.canSetPlaybackSpeed);
		Debug.Log ("can set skip on drop: " + video.canSetSkipOnDrop);
		Debug.Log ("can set time: " + video.canSetTime);
		Debug.Log ("can step: " + video.canStep);

	}

	public void PlayVideo(){
		if (!IsPrepared)
			return;
		video.Play ();
	}

	public void PauseVideo(){
		if (!IsPlaying)
			return;
		video.Pause ();
	}

	public void RestartVideo(){
		if (!IsPrepared)
			return;
		PauseVideo ();
		Seek (0);
	}

	public void LoopVideo(bool toggle){
		if (!IsPrepared)
			return;
		video.isLooping = toggle;
	}

	public void Seek(float nTime){
		if (!video.canSetTime)
			return;
		if (!IsPrepared)
			return;
		nTime = Mathf.Clamp (nTime, 0, 1);
		video.time = nTime * Duration;
	}

	public void IncrementPlaybackSpeed(){
		if (!video.canSetPlaybackSpeed)
			return;
		// playback spped bounds between 0 - 10
		video.playbackSpeed += 1;
		video.playbackSpeed = Mathf.Clamp (video.playbackSpeed, 0, 10);
	}

	public void DecrementPlaybackSpeed(){
		if (!video.canSetPlaybackSpeed)
			return;
		// playback spped bounds between 0 - 10
		video.playbackSpeed -= 1;
		video.playbackSpeed = Mathf.Clamp (video.playbackSpeed, 0, 10);
	}
		
}
