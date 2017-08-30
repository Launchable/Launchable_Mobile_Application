using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class estateButtons : MonoBehaviour {
    public TargetTracker tracker;
	
	public void phoneContact(){
        Debug.Log("PHONE CONTACT IS " + tracker.GetTargetPhone());
        Application.OpenURL ("tel://" + tracker.GetTargetPhone());
	}
	
	public void webContact(){
		//Application.OpenURL (tracker.webContact);  
	}
	
	public void emailContact(){
        Debug.Log("EMAIL CONTACT IS " + tracker.GetTargetEmail());
		Application.OpenURL("mailto:" + tracker.GetTargetEmail()); 
	}
}
