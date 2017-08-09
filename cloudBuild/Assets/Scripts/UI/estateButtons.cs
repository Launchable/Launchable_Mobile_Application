using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class estateButtons : MonoBehaviour {
	public DynamicDataSetLoader datasetLoader;
	
	public void phoneContact(){
		 Application.OpenURL ("tel://" + datasetLoader.phoneContact);
	}
	
	public void webContact(){
		Application.OpenURL (datasetLoader.webContact);  
	}
	
	public void emailContact(){
		Application.OpenURL("mailto:" + datasetLoader.emailContact); 
	}
}
