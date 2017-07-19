using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class tapDude : MonoBehaviour {

	private CameraDevice.FocusMode mFocusMode = CameraDevice.FocusMode.FOCUS_MODE_NORMAL;

	private const string AUTOFOCUS_ON = "Autofocus On";
	private const string AUTOFOCUS_OFF = "Autofocus Off";
	private string mAutoFocusText = "";

	float touchDuration;
	private Touch touch;

	private float newx;
	private float newy;

//	public GameObject animation;

	private bool check = false;


	//An array of Objects that stores the results of the Resources.LoadAll() method  
	private Object[] objects;  
	//Each returned object is converted to a Texture and stored in this array  
	private Texture[] textures;  
	//With this Material object, a reference to the game object Material can be stored  
	private Material goMaterial;  
	//An integer to advance frames  
	private int frameCounter = 0;     



//	private bool mouseClicked = false;

			
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



		//Load all textures found on the Sequence folder, that is placed inside the resources folder  
		this.objects = Resources.LoadAll("Sequence", typeof(Texture));  

		//Initialize the array of textures with the same size as the objects array  
		this.textures = new Texture[objects.Length];  

		//Cast each Object to Texture and store the result inside the Textures array  
		for(int i=0; i < objects.Length;i++)  
		{  
			this.textures[i] = (Texture)this.objects[i];  

			print ("this is i value: " + i);
		}  
			
	}
		
	void Update() {



		if (Input.GetMouseButton (0)) {
//				mouseClicked = true;
			StartCoroutine(Play(3f));  
			//Set the material's texture to the current value of the frameCounter variable  
//			animation.GetComponent<RawImage>().texture = textures[frameCounter]; 
//
////			Debug.Log (frameCounter);
//
//			animation.transform.localScale = new Vector3 (1f, 1f, 1f);
//			animation.transform.position = Input.mousePosition;

		}
			
//		StartCoroutine (singleOrDouble ());

//		if (Input.GetMouseButtonUp (0)) {
//
//
//			animation.transform.localScale = new Vector3 (0, 0, 0);
//		}



		if (Input.touchCount > 0) { 
			//if there is any touch
			touchDuration += Time.deltaTime;
			touch = Input.GetTouch (0);

			/*
			if(touch.phase == TouchPhase.Began)
			{
				print ("coming here right now");
				RaycastHit hit;

				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

				print ("this is ray: " + ray);

				if (Physics.Raycast (ray, out hit)) 
					if (hit.collider != null) 
				{
					animation.GetComponent<RawImage>().transform.position = Vector3.Lerp (transform.position, hit.point, Time.time);
					print ("hello!!");
				}
					
				print ("object should be moving now");
			}
			*/

			if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f) { 
				//making sure it only check the touch once && it was a short touch/tap and not a dragging.
//				StartCoroutine ("singleOrDouble");

				HandleSingleTap ();

			}

		} else {
			touchDuration = 0.0f;
		}

//		if (check == true) {
//			animation.transform.localScale = new Vector3 (0, 0, 0);
//			check = false;
//		}


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
		
//	private IEnumerator singleOrDouble(){
////		yield return new WaitForSeconds(2f);
//		yield return new WaitForEndOfFrame ();
////		yield return new WaitForEndOfFrame ();
//
//		if(touch.tapCount == 1)
//			Debug.Log ("Single");
//		else if(touch.tapCount == 2){
//			//this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
//			StopCoroutine("singleOrDouble");
//			Debug.Log ("Double");
//		}
//	}
		
		
//	private void OnVuforiaStarted()
//	{
//		CameraDevice.Instance.SetFocusMode(
//			CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
//	}
//
//	private void OnPaused(bool paused)
//	{
//		if (!paused) // resumed
//		{
//			// Set again autofocus mode when app is resumed
//			CameraDevice.Instance.SetFocusMode(
//				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
//		}
//	}





//	void Awake()  
//	{  
//		//Get a reference to the Material of the game object this script is attached to  
//		//		this.goMaterial = this.renderer.material;  
//		//		this.goMaterial = this.rend.material;
//	}  

//	void Start ()  
//	{  
//
//	}  
//
//	void Update ()  
//	{  
//		//Call the 'PlayLoop' method as a coroutine with a 0.04 delay  
//	}  
//
//	//The following methods return a IEnumerator so they can be yielded:  
//	//A method to play the animation in a loop  
//	IEnumerator PlayLoop(float delay)  
//	{  
//		//Wait for the time defined at the delay parameter  
//		yield return new WaitForSeconds(delay);    
//
//		//Advance one frame  
//		frameCounter = (++frameCounter)%textures.Length;  
//
//		//Stop this coroutine  
//		StopCoroutine("PlayLoop");  
//	}    
//
	//A method to play the animation just once  
	IEnumerator Play(float delay)  
	{  
		//Wait for the time defined at the delay parameter  
		yield return new WaitForSeconds(delay);    

		//If the frame counter isn't at the last frame  
		if(frameCounter < textures.Length-1)  
		{  
			//Advance one frame  
			++frameCounter;  
		}  

		//Stop this coroutine  
//		StopCoroutine("PlayLoop");

		check = true;

	}



}
