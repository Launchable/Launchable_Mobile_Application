﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetTracker : MonoBehaviour {

        
    public List<GameObject> targets;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.K))
        {
            ClearTargetTracking();
        }
	}

    public void AddTarget (GameObject targ)
    {
        Debug.Log("Added new target: " + targ);
        targets.Add(targ);
    }

    public void RemoveTarget (GameObject targ)
    {
        targets.Remove(targ);
    }

    public void ClearTargetTracking()
    {
        foreach (GameObject targ in targets)
        {
            targ.GetComponent<DefaultTrackableEventHandler>().DropTargetTracking();
        }
    }
}