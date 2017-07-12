using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class estateButtons : MonoBehaviour {
	public metadataParse metadata;
	
	public void phoneContact(){
		 Application.OpenURL ("tel://" + metadata.phoneContact);
	}
	
	public void webContact(){
		print(metadata.webContact);
		Application.OpenURL (metadata.webContact);  
	}
	
	public void emailContact(){
		print(metadata.emailContact);
		Application.OpenURL("mailto:" + metadata.emailContact); 
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
