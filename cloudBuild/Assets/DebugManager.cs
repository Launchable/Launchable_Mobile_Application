using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {

    bool mouseDown = false;
    float holdTime = 0.0f;
    float timeInc = 0.1f;

    public bool isConsoleOpen;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
            StartCoroutine(HoldTimer());
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }

    }

    IEnumerator HoldTimer()
    {
        yield return new WaitForSeconds(timeInc);
        holdTime += timeInc;
        Debug.Log("Holding @ " + holdTime);
        if (mouseDown)
        {
            StartCoroutine(HoldTimer());
        }
        else
        {
            CheckHold();
        }
    }

    void CheckHold()
    {
        if(holdTime > 1f)
        {
            if(isConsoleOpen)
            {
                CloseConsole();
            }
            else
            {
                OpenConsole();  
            }
        }
        holdTime = 0.0f;
    }

    public void CloseConsole()
    {
        isConsoleOpen = false;
        anim.SetBool("IsActive", isConsoleOpen);
    }

    public void OpenConsole()
    {
        isConsoleOpen = true;
        anim.SetBool("IsActive", isConsoleOpen);
    }
}
