using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int startinghealth = 20;
	public int currenthealth;
	public int attackdamage = 5;
	// public AudioClip deathClip;
	
	//AudioSource playerAudio;
	bool isDead;
	// bool damaged;

	// Use this for initialization
	void Awake () {
		//playerAudio = GetComponent <AudioSource> ();
		currenthealth = startinghealth;
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Figure better system for stats
	public int Attack () {
		return attackdamage;
	}

	
	public void TakeDamage (int amount)
	{
		// damaged = true;
		currenthealth -= amount;
		Debug.Log("Current enemy health: " + currenthealth);
		
		//playerAudio.play();
		
		if (currenthealth <= 0 && !isDead) 
		{
			Death();
		}
		
	}
	
	void Death () {
		isDead = true;
		Debug.Log("You defeated the enemy!");
		//playerAudio.clip = deathClip;
		//playerAudio.Play();
		
	}

	public bool Died () {
		return isDead;
	}
}
