/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;

        #endregion // PRIVATE_MEMBER_VARIABLES
        metadataParse mParser;
        DynamicDataSetLoader targetControl;
        analyticsController analyticsControl;

        private bool firstrun = true;
        private bool ispaused;

        private float startTime = 0.0f;
        private float endTime = 5.0f;
        private float timer;
        private bool check;

        public Text debugText;

        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mParser = transform.Find("targetObject(Clone)").GetComponent<metadataParse>();
            targetControl = GameObject.FindObjectOfType<DynamicDataSetLoader>();
            analyticsControl = GameObject.FindObjectOfType<analyticsController>();

            debugText = GameObject.Find("Debug").GetComponent<Text>();

            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

        }
        /*
		void Update()
		{
			if (Input.GetMouseButtonDown (0)) {
				mParser.videoPlayer.Pause ();
				mParser.audioSource.Pause ();
				mParser.playPauseIcon.gameObject.SetActive (true);
			}
//			else if(Input.GetMouseButtonUp(0)) {
//				if (timer != endTime) {
//					timer += Time.deltaTime;
//				} else if (timer == endTime) {
//					mParser.playPauseIcon.gameObject.SetActive (false);
//					timer = 0.0f;
//				}
//			}
		}*/


        #endregion // UNTIY_MONOBEHAVIOUR_METHODS


        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();

            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            GameObject.Find("targetManager").GetComponent<TargetTracker>().AddTarget(this.gameObject);

            ispaused = mParser.isPaused;
            firstrun = mParser.firstRun;

            check = GameObject.FindObjectOfType<metadataParse>().pauseVideo;


            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);


            // Enable Canvas
            foreach (Canvas component in canvasComponents)
            {
                component.enabled = true;
            }
            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }


            if (check == true)
            {
                mParser.videoPlayer.Pause();
                mParser.audioSource.Pause();
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            string trackableName = mTrackableBehaviour.TrackableName;
            analyticsControl.launchTimeTrack(true);
            if (targetControl.currentTrackable != trackableName)
            {
                targetControl.currentTrackable = trackableName;
                mParser.resetCard();
                debugText.text += "After reset card @ time: " + Time.time + "\n";
                mParser.loadMetadata(trackableName);
            }
            // if still the same target play the video
            else
            {
                //				mParser.videoPlayer.Play ();
                //				mParser.audioSource.Play ();
                mParser.playIcon.gameObject.SetActive(true);
                mParser.pauseIcon.gameObject.SetActive(false);
                mParser.pauseVideo = true;

            }

            mParser.runContactAnimation();

        }

        public void DropTargetTracking()
        {
            OnTrackingLost();
        }

        private void OnTrackingLost()
        {
            GameObject.Find("targetManager").GetComponent<TargetTracker>().RemoveTarget(this.gameObject);


            firstrun = mParser.firstRun;
            if (!firstrun)
            {
                Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
                Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
                Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);

                // Disable Canvas
                foreach (Canvas component in canvasComponents)
                {
                    component.enabled = false;
                }

                // Disable rendering:
                foreach (Renderer component in rendererComponents)
                {
                    component.enabled = false;
                }

                // Disable colliders:
                foreach (Collider component in colliderComponents)
                {
                    component.enabled = false;
                }
                analyticsControl.launchTimeTrack(false);

                Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

                ispaused = mParser.isPaused;
                firstrun = mParser.firstRun;

                // when target lost pause the video
                if (!firstrun)
                {   
                    mParser.videoPlayer.Pause();
                    mParser.audioSource.Pause();
                }

                mParser.videoPlayer.Pause();
                mParser.audioSource.Pause();

                //			mParser.playIcon.gameObject.SetActive (false);
                //			mParser.pauseIcon.gameObject.SetActive (false);

                mParser.resetContactButtons();
            }
            else
            {
                //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " first run @ time: " + Time.time);
            }
        }
        #endregion // PRIVATE_METHODS

    }

}