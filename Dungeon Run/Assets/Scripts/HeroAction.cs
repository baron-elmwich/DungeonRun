﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroAction : MonoBehaviour {

	public int startingAction = 0;
	public int maxAction = 100;
	public int currentAction;
	public Slider ActionSlider;

	public int speed = 5;
	public int defaultFillScaler = 75;
	public bool actionBarFull = false;
	public bool actionHandled = false;
	private bool isPaused = false;
	
	// Use this for initialization
	void Awake () {
		currentAction = startingAction;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentAction < maxAction) 
		{
			if (!isPaused)
			{
				currentAction += (int) ( speed * defaultFillScaler * Time.deltaTime );
				ActionSlider.value = currentAction;
			}
		}

		actionBarFull = currentAction >= maxAction;
	}

	public bool isActionBarFull () {
		// I may not need this function
		return false;
	}

	public void Reset () {
		currentAction = 0;
		actionBarFull = false;
	}

	public void Pause () {
		isPaused = true;
	}
	
	public void UnPause () {
		isPaused = false;
	}

}
