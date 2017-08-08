/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine.Video;
using UnityEngine;

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

        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
			mParser = transform.Find("targetObject(Clone)").GetComponent<metadataParse> ();
			targetControl = GameObject.FindObjectOfType<DynamicDataSetLoader> ();
			analyticsControl = GameObject.FindObjectOfType<analyticsController> ();

            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
				
        }
			

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
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Canvas [] canvasComponents = GetComponentsInChildren<Canvas>(true);

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

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			string trackableName = mTrackableBehaviour.TrackableName;
			analyticsControl.launchTimeTrack(true);

			if (targetControl.currentTrackable != trackableName) {
				targetControl.currentTrackable = trackableName;
				mParser.resetCard ();
				mParser.loadMetadata (trackableName);
			} 
			// if still the same target play the video
			else {
				mParser.videoPlayer.Play ();
				mParser.audioSource.Play ();
			}

        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Canvas [] canvasComponents = GetComponentsInChildren<Canvas>(true);

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

			// when target lost pause the video
			mParser.videoPlayer.Pause ();
			mParser.audioSource.Pause ();
		
		}
        #endregion // PRIVATE_METHODS

    }
}
