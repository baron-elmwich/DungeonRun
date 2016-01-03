using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseHero {

	private int maxHealth = 10;
	private int maxMagic = 10;
	private int maxAction = 10;
	public int currentHealth;
	public int currentMagic;
	public int currentAction;

	private int strength = 4;
	private int defense = 4;
	private int speed = 4;

	//private Button fightButton; 

	public int Health
	{
		get {return currentHealth; }
		set {currentHealth = value; }
	}

	public int Magic
	{
		get {return currentMagic; }
		set {currentMagic = value; }
	}
	
	public int Action
	{
		get {return currentAction; }
		set {currentAction = value; }
	}

	// public int Magic { get; set; }
	
	public int Strength
	{
		get {return strength; }
		set {strength = value; }
	}
	
	public int Defense
	{
		get {return defense; }
		set {defense = value; }
	}

	public int Speed
	{
		get {return speed; }
		set {speed = value; }
	}
	
	public void ChooseAction ()
	{
		// Enable choice gui 
		//button.enabled = true;
		// Choose which action to perform via buttons 
		//WaitForSeconds(2);
		// Return choice to the ChoiceQueue. Can I return a function?
		// Disable choice gui
		//button.enabled = false;
	}
}
