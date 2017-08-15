using UnityEngine;
using System.Collections;

public class UM_CameraAPIExample : BaseIOSFeaturePreview {

	public Texture2D hello_texture;
	public Texture2D darawTexgture = null;

	


	void OnGUI() {
		UpdateToStartPos();



		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Camera And Gallery", style);
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Save Screenshot To Camera Roll")) {
			UM_Camera.Instance.OnImageSaved += OnImageSaved;
			UM_Camera.Instance.SaveScreenshotToGallery();
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Save Texture To Camera Roll")) {
			UM_Camera.Instance.OnImageSaved += OnImageSaved;
			UM_Camera.Instance.SaveImageToGalalry(hello_texture);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Album")) {
			UM_Camera.Instance.OnImagePicked += OnImage;
			UM_Camera.Instance.GetImageFromGallery();
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Camera")) {
			UM_Camera.Instance.OnImagePicked += OnImage;
			UM_Camera.Instance.GetImageFromCamera();
		}

		StartX = XStartPos;
		StartY+= YButtonStep;
		StartY+= YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "PickedImage", style);
		StartY+= YLableStep;

		if(darawTexgture != null) {
			GUI.DrawTexture(new Rect(StartX, StartY, buttonWidth, buttonWidth), darawTexgture);
		}
	

	}


	void OnImageSaved (UM_ImageSaveResult result) {
		UM_Camera.Instance.OnImageSaved -= OnImageSaved;
		if(result.IsSucceeded) {
			//no image path for IOS
			MNPopup popup = new MNPopup ("Image Saved", result.imagePath);
			popup.AddAction ("Ok", () => {});
			popup.Show ();
		} else {
			MNPopup popup = new MNPopup ("Failed", "Image Save Failed");
			popup.AddAction ("Ok", () => {});
			popup.Show ();
		}

	}

	

	private void OnImage (UM_ImagePickResult result) {
		UM_Camera.Instance.OnImageSaved -= OnImageSaved;
		if(result.IsSucceeded) {
			darawTexgture = result.image;
		}

		UM_Camera.Instance.OnImagePicked -= OnImage;
	}
}
