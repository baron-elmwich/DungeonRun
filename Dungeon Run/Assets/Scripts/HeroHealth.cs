using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroHealth : MonoBehaviour {

	public int startinghealth = 30;
	public int currenthealth;
	public int attackdamage = 7;
	public Slider HealthSlider;
	public Image DamageImage;
	// public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

	//AudioSource playerAudio;
	bool isDead;
	bool damaged;

	// Use this for initialization
	void Awake () {
		//playerAudio = GetComponent <AudioSource> ();
		currenthealth = startinghealth;
	}
	
	// Update is called once per frame
	void Update () {
		// flash a color if player takes damage
		if (damaged) 
		{
			DamageImage.color = flashColor;
		} else 
		{
			DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	}

	// Figure better system for stats
	public int Attack () {
		return attackdamage;
	}

	public void TakeDamage (int amount)
	{
		damaged = true;
		currenthealth -= amount;
		HealthSlider.value = currenthealth;
		Debug.Log("Current hero health: " + currenthealth);

		//playerAudio.play();

		if (currenthealth <= 0 && !isDead) 
		{
			Death();
		}

	}

	void Death () {
		isDead = true;

		//playerAudio.clip = deathClip;
		//playerAudio.Play();

	}
}
