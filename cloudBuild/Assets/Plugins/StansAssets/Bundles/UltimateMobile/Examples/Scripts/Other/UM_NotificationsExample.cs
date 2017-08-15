using UnityEngine;
using System.Collections;

public class UM_NotificationsExample : BaseIOSFeaturePreview {


	private int LastNotificationId = 0;


	void OnGUI() {
		UpdateToStartPos();



		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Local Notifications API", style);
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Show Notification Popup ")) {
			UM_NotificationController.Instance.ShowNotificationPoup("Hello", "Notification popup test");
		}



		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Schedule Local Notification")) {
			LastNotificationId = UM_NotificationController.Instance.ScheduleLocalNotification("Hello Locacl", "Local Notification Example", 5);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Cansel Last Notification")) {
			UM_NotificationController.Instance.CancelLocalNotification(LastNotificationId);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Cansel All Last Notifications")) {
			UM_NotificationController.Instance.CancelAllLocalNotifications();
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		StartY+=YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Push Notifications API", style);
		StartY+= YLableStep;


		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Retrieve Device PushId ")) {
			UM_NotificationController.OnPushIdLoadResult += OnPushIdLoaded;
			UM_NotificationController.Instance.RetrieveDevicePushId();
		}

	}



	private void OnPushIdLoaded (UM_PushRegistrationResult res) {
		if(res.IsSucceeded) {
			MNPopup popup = new MNPopup ("Succeeded", "Device Id: " + res.deviceId);
			popup.AddAction ("Ok", () => {});
			popup.Show ();
		} else {
			MNPopup popup = new MNPopup ("Failed", "No device id");
			popup.AddAction ("Ok", () => {});
			popup.Show ();
		}
	}
}
