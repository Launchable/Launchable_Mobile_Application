using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flashLightController : MonoBehaviour
{

	#region Public Variables
	public Image _TorchToggleImage;
	public Sprite _TorchOnSprite;
	public Sprite _TorchOffSprite;
	#endregion

	#region Private Variables
	private bool mIsTorchSupported = false;
	private bool mTorchState = false;
	#endregion

	#region Unity Methods
	void Awake ()
	{
		Vuforia.VuforiaARController.Instance.RegisterVuforiaInitializedCallback (delegate() {
			mIsTorchSupported = Vuforia.CameraDevice.Instance.SetFlashTorchMode (false);
			mTorchState = false;
		});
	}
	#endregion


	#region Public Methods
	public void ToggleTorch ()
	{
		mTorchState = !mTorchState;
		Vuforia.CameraDevice.Instance.SetFlashTorchMode (mTorchState);
		_TorchToggleImage.sprite = (mTorchState == true) ? _TorchOnSprite : _TorchOffSprite;

	}
	#endregion
}