using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachingController : MonoBehaviour
{

	#region Public Variables

//	[Header ("UI")]
//	public GameObject _WindowCoaching;
	public GameObject _PanelCoaching1;
	public GameObject _PanelCoaching2;
	public GameObject _PanelCoaching3;

	#endregion

	#region Unity Methods

	void Start ()
	{
//		bool hasAlreadySeen = PlayerPrefs.GetInt ("HasSeenCoaching", 0) == 1;
//		if (hasAlreadySeen) {
//			HideCoaching ();
//			return;
//		}
		SetupUI ();
	}

	#endregion

	#region Public Methods

	public void HideCoaching ()
	{
		PlayerPrefs.SetInt ("HasSeenCoaching", 1);
//		_WindowCoaching.SetActive (false);
	}

	public void ShowCoaching2 ()
	{
		_PanelCoaching1.SetActive (false);
		_PanelCoaching2.SetActive (true);
		_PanelCoaching3.SetActive (false);
	}

	public void ShowCoaching3 ()
	{
		_PanelCoaching1.SetActive (false);
		_PanelCoaching2.SetActive (false);
		_PanelCoaching3.SetActive (true);
	}

	#endregion

	#region Private Methods

	private void SetupUI ()
	{
//		_WindowCoaching.SetActive (true);
		_PanelCoaching1.SetActive (true);
		_PanelCoaching2.SetActive (true);
		_PanelCoaching3.SetActive (true);
	}

	#endregion

}