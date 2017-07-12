using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using monoflow;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class metadataParse : MonoBehaviour {
	
	public string emailContact = "none";
	public string webContact = "none";
	public string phoneContact = "none";
	Text estateTitle;
	Text estateBody;
	Image estatePicture;
	MPMP MPMPPlayback;
	string streamPath; //path to save target info locally 
	string targetName; 
	string estatesCover = "estates_1";
	bool estateCard = false; //check if we should have contact card functionality
	
	void Start(){
		streamPath = Application.persistentDataPath + "/";
	#if UNITY_IOS
		streamPath += "Library/Application Support/";
	#endif
	}
	
	//gets target name from defaultTrackableEventHandler and finds the metadata file
	public void loadMetadata(string metadataFile){
		if(metadataFile == estatesCover){
			
			return;
		}
		string metadataPath = streamPath + metadataFile+".txt";
		targetName = metadataFile;
		StreamReader file =  new StreamReader(metadataPath);
		parseData(file);
	}
	
	void parseData (StreamReader metadataFile) {
		//find cloned objects
		estateTitle = transform.Find("Canvas/estateCard/title").GetComponent<Text>();
		estateBody = transform.Find("Canvas/estateCard/body").GetComponent<Text>();
		estatePicture = transform.Find("Canvas/picture").GetComponent<Image>();
		string line;
		while((line = metadataFile.ReadLine()) != null)
		{
				executeLineCommand(line);
		}
		print("estate card: " + estateCard);
		if(estateCard){	
			transform.Find("Canvas").GetComponent<CanvasGroup>().alpha =1.0f;
			//transform.Find("Quad").transform.position = new Vector3(0.35f,0.0f,0.74f);
			print("move video up");		
			transform.Find("Canvas").localScale = new Vector3(.00858262f,.00858262f,.00858262f);
		}else{
			transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0.0f;
			transform.Find("Quad").transform.position = new Vector3(0.0f,0.0f,0.0f);
			print("estate card disappear");
		}
		
		metadataFile.Close();
	}
	
	IEnumerator waitToPlay(){
		yield return new WaitForSeconds(1f);
		MPMPPlayback.Play();
	}
	
	void executeLineCommand(string line){
		string[] splitMetadata = line.Split(' ');
		if(splitMetadata[1].StartsWith("false")) return;
		
		switch (splitMetadata[0])
		{
          case "videoUrl":
            loadVideo(splitMetadata[1]);
            break;
          case "3durl":
            load3dAsset(splitMetadata[1]);
            break;
          case "estateCard":
            estateCard = false; //to activate contact card functionality this should be changed to true.
            break;
		default:
			break;
		}
		
		if(estateCard){
			
			switch(splitMetadata[0])
			{
			  case "estateCardTitle":
				estateTitle.text = line.Replace(splitMetadata[0],"");
				break;
			  case "estateCardDescription":
				estateBody.text = line.Replace(splitMetadata[0],"");
				break;
			  case "estateImageUrl":
				loadImage(splitMetadata[1]);
				break;
			  case "estatePhone":
				phoneContact = splitMetadata[1];
				break;
			  case "estateEmail":
				emailContact = splitMetadata[1];
				break;
			  case "estateWebsite":
				webContact = splitMetadata[1];
				break;
			default:
				break;
			}
		}

	}
	
	void loadVideo(string url){
			//the quad mesh render turns off for some reason so I'm turning it back on as a temporary fix until we figure out why it does that
			MeshRenderer quadMeshRender = transform.Find("Quad").GetComponent<MeshRenderer>();
			quadMeshRender.enabled = true;
			
			//grab mpmp object and run the video from url
			MPMPPlayback = transform.Find("MPMP.instance").GetComponent<MPMP>();
			MPMPPlayback.Load(url);
			StartCoroutine(waitToPlay());
	}
	
	void load3dAsset(string url){
		StartCoroutine(GetAssetBundle(url));
	}
	
	void loadImage(string url){
		StartCoroutine(GetImage(url));
	}
	
    IEnumerator GetAssetBundle(string url) {
        
		UnityWebRequest www = UnityWebRequest.GetAssetBundle(url);
		yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
			AssetBundle bundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
			Instantiate(bundle.LoadAsset("Maze_1"),transform);
			
		}
		
		//not tested
		//byte[] bytes = www.bytes;
		//string filename = test;
		//File.WriteAllBytes(Application.persistentDataPath+"/" + filename, bytes);
		/*reference
		http://answers.unity3d.com/questions/19522/how-to-download-an-asset-bundle-to-local-hard-driv.html
		http://answers.unity3d.com/questions/591912/how-to-download-asset-bundles-from-website-to-appl.html
		*/
    }
	
    IEnumerator GetImage(string url) {
		WWW www = new WWW(url);
		yield return www;
		estatePicture.sprite =  Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }	
	
	public void resetCard(){
		print("reseting");
		transform.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0.0f;
	}
}
