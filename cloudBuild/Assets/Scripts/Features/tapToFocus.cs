using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class tapToFocus : MonoBehaviour {

	private CameraDevice.FocusMode mFocusMode = CameraDevice.FocusMode.FOCUS_MODE_NORMAL;

	private const string AUTOFOCUS_ON = "Autofocus On";
	private const string AUTOFOCUS_OFF = "Autofocus Off";
	private string mAutoFocusText = "";

	float touchDuration;
	private Touch touch;
			
	void OnEnable()
	{
		VuforiaBehaviour vuforiaBehaviour = (VuforiaBehaviour)FindObjectOfType(typeof(VuforiaBehaviour));

		if (vuforiaBehaviour)
		{
			
		}
		var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

		if (!isAutoFocus)
		{
			CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
		}
			
	}
		
	void Update() {
		if (Input.touchCount > 0) { 
			//if there is any touch
			touchDuration += Time.deltaTime;
			touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f) { 
				StartCoroutine ("singleOrDouble");
				HandleSingleTap ();
			}

		} else {
			touchDuration = 0.0f;
		}

	}
		
	private void HandleSingleTap()
	{
		// trigger focus once
		if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO))
		{
			mFocusMode = CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO;
			mAutoFocusText = AUTOFOCUS_ON;
		}

	}
		
	private IEnumerator singleOrDouble(){
		yield return new WaitForEndOfFrame ();

		if(touch.tapCount == 1)
			Debug.Log ("Single");
		else if(touch.tapCount == 2){
			//this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
			StopCoroutine("singleOrDouble");
			Debug.Log ("Double");
		}
	}
		
}
