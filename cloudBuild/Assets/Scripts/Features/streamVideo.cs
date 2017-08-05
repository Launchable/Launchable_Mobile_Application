using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class streamVideo : MonoBehaviour {

	public RawImage image;
	public VideoClip videoToPlay;
	private VideoPlayer videoPlayer;
	private VideoSource videoSource;
	private AudioSource audioSource;

	public GameObject playIcon;

	public bool isPaused = false;
	private bool firstRun = true;


	IEnumerator playVideo()
	{
		playIcon.gameObject.SetActive (false);
		firstRun = false;

		//Add VideoPlayer to the GameObject
		videoPlayer = gameObject.AddComponent<VideoPlayer> ();

		//Add AudioSource
		audioSource = gameObject.AddComponent<AudioSource> ();

		//Disable Play on Awake for both Video and Audio
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		audioSource.Pause ();

		//We want to play from video clip not from url
		videoPlayer.source = VideoSource.VideoClip;

		// Vide clip from Url
		videoPlayer.source = VideoSource.Url;
		videoPlayer.url = "https://s3.amazonaws.com/launchable-videos/HkNrn0lEZ/HkNrn0lEZ.mp4";


		//Set Audio Output to AudioSource
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		videoPlayer.EnableAudioTrack (0, true);
		videoPlayer.SetTargetAudioSource (0, audioSource);

		//Set video To Play then prepare Audio to prevent Buffering
		videoPlayer.clip = videoToPlay;
		videoPlayer.Prepare ();

		//Wait until video is prepared
		WaitForSeconds waitTime = new WaitForSeconds (1);
		while (!videoPlayer.isPrepared) {
			Debug.Log ("Preparing Video");
			//Prepare/Wait for 5 sceonds only
			yield return waitTime;
			//Break out of the while loop after 5 seconds wait
			break;
		}

		Debug.Log ("Done Preparing Video");

		//Assign the Texture from Video to RawImage to be displayed
		image.texture = videoPlayer.texture;

		//Play Video
		videoPlayer.Play();

		//Play Sound
		audioSource.Play ();

		Debug.Log ("Playing Video");
		while (videoPlayer.isPlaying) {
			Debug.LogWarning ("Video Time: " + Mathf.FloorToInt ((float)videoPlayer.time));
			yield return null;
		}

		Debug.Log ("Done Playing Video");

	}

	public void playPause() {
		if (!firstRun && !isPaused) {
			videoPlayer.Pause ();
			audioSource.Pause ();
			playIcon.SetActive (true);
			isPaused = true;
		} else if (!firstRun && isPaused) {
			videoPlayer.Play ();
			audioSource.Play ();
			playIcon.SetActive (false);
			isPaused = false;
		} else {
			StartCoroutine (playVideo ());
		}
	}


}
