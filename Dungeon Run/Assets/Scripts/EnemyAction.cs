using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyAction : MonoBehaviour {

	public int startingAction = 0;
	public int maxAction = 100;
	public int currentAction;
	
	public int speed = 4;
	public int defaultFillScaler = 50;
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
