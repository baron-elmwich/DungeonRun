using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	public GameObject stageImage;
	public Camera cam;
	public Transform[] enemySpawnPoints;
	public Transform[] heroSpawnPoints;

	//private int area = 1;
	//private int level = 1;
	private int stageNum;
	private float time;
	
	public GameObject enemy;
	public GameObject hero;

	public List<GameObject> Characters;
	public List<GameObject> Heroes;
	public List<GameObject> Enemies;
	private int deadEnemyCount;
	private int deadHeroCount;

	private HeroAction heroAction;
	private EnemyAction enemyAction;
	//private HeroHealth heroHealth;
	//private EnemyHealth enemyHealth;

	public GameObject fightButton;

	public Queue<string> activeHeroQueue;
	public Queue<string> activeEnemyQueue;
	private string activeHeroTag;
	private string activeEnemyTag;
	// Separate queue for enemy actions so they can attack while you are still making choices
	// Single queue for choices so attacks don't happen simultaneously
	// public Queue<???> choiceQueue;

	// Use this for initialization
	void Awake () {
		ChooseStage();
		SpawnParty();
		ChooseEnemies();
		SpawnEnemies();

		// May need to reorganize this part
		activeHeroTag = "";
		activeEnemyTag = "";
		heroAction = hero.GetComponent<HeroAction> ();
		//enemyAction = enemy.GetComponent<EnemyAction> ();
		//heroHealth = hero.GetComponent<HeroHealth> ();
		//enemyHealth = enemy.GetComponent<EnemyHealth> ();
		fightButton.SetActive(false);

		// Begin battle, move to Update
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		// Check for full action bars on heroes
		for (int i=0; i<Heroes.Count; i++) {
			heroAction = Heroes[i].GetComponent<HeroAction> ();
			//if (characterAction.actionBarFull && !activeHeroQueue.Contains(Characters[i].gameObject.tag)) {
			if (heroAction.actionBarFull && !activeHeroTag.Equals(Heroes[i].gameObject.tag)) {
				activeHeroQueue.Enqueue(Characters[i].gameObject.tag);
				//Debug.Log("Engueued " + Characters[i].gameObject.tag);
				heroAction.actionHandled = false; 
				fightButton.SetActive(true);
			} 
		}
		
		// Check for full action bars on enemies
		for (int i=0; i<Enemies.Count; i++) {
			enemyAction = Enemies[i].GetComponent<EnemyAction> ();
			//if (characterAction.actionBarFull && !activeHeroQueue.Contains(Enemies[i].gameObject.tag)) {
			if (enemyAction.actionBarFull && !activeEnemyTag.Equals(Enemies[i].gameObject.tag)) {
				activeEnemyQueue.Enqueue(Enemies[i].gameObject.tag);
				Debug.Log("Enemy - Engueued " + Enemies[i].gameObject.tag);
				enemyAction.actionHandled = false; 
			} 
		}

		// Check action queues for active heroes
		while ( activeHeroQueue.Count > 0) {
			// Dequeue first entry from action queue
			activeHeroTag = activeHeroQueue.Dequeue();
			//Debug.Log("Active Hero: " + activeHeroTag);

			// Characters[i].ChooseAction();
			// Make UI choices presumably
			// Add choice to choice queue (this IS Characters.Count long)

			// Again NOTE: Need to redefine what characterAction means in this loop
			// characterAction.actionHandled = true;
			// Cue to reset the action bar
			//characterAction.Reset();
		}

		// Check action queues for active enemies
		while ( activeEnemyQueue.Count > 0) {
			// Dequeue first entry from action queue
			activeEnemyTag = activeEnemyQueue.Dequeue();
			Debug.Log(activeEnemyTag + " dequeued.");

			hero.GetComponent<HeroHealth>().TakeDamage(5);
			
			int activeEnemyIndex = ReturnCharacterIndex(activeEnemyTag);
			Characters[activeEnemyIndex].GetComponent<EnemyAction>().Reset();
			activeEnemyTag = ""; 
		}

		// Check choice queue 
		for (int i=0; i<Characters.Count; i++) {
			// Delay and execute the choices, delay before next choice is executed?
		}

		// Check for dead heroes

		// Check for dead enemies
		for (int i=0; i<Enemies.Count; i++) {
			if (Enemies[i].GetComponent<EnemyHealth>().Died()){
				Enemies[i].SetActive(false);
				deadEnemyCount++;
			}
		}

		if (deadEnemyCount==Enemies.Count) {
			Debug.Log("You have defeated all enemies!");
		}

	}

	void ChooseStage () 
	{
		// Add switch for area and level in future

		// 1 - given probs determined by area and level, randomly select a stage number
		stageNum = Random.Range(1,4); // Only 3 test images to choose from right now

		// 2 - attach texture corresponding to stage number to stageImage quad
		string stageName = "test_forest" + stageNum;
		// Debug.Log("Stage Name: " + stageName);
		Material material = Resources.Load(stageName, typeof(Material)) as Material;
		MeshRenderer mr = stageImage.GetComponent<MeshRenderer>();
		mr.material = material;
		mr.material.shader = Shader.Find("Unlit/Texture");

		// 3 - stretch background to camera size
		// Ignore this step for now, I made the test bg the wrong size is why they didn't fit the camera
	}

	void ChooseEnemies ()
	{
		/*
		int numEnemies = 1;
		for (int i=0; i<numEnemies; i++) {
			//GameObject tmpEnemy = Resources.Load("Prefabs/TestEnemy", typeof(GameObject)) as GameObject;
			GameObject tmpEnemy = (GameObject) Instantiate(Resources.Load("Prefabs/TestEnemy"));
			Debug.Log("GameObject " + tmpEnemy.gameObject.name + " created.");

			// Add the enemies to the characters list
			Enemies.Add(tmpEnemy);
			Characters.Add(tmpEnemy);
			Debug.Log("Enemies: " + Enemies.Count + " Characters: " + Characters.Count);
		} */
	}

	void SpawnEnemies ()
	{	
		int numEnemies = 1;
		for (int i=0; i<numEnemies; i++) {
			//GameObject tmpEnemy = Resources.Load("Prefabs/TestEnemy", typeof(GameObject)) as GameObject;
			GameObject tmpEnemy = (GameObject) Instantiate(Resources.Load("Prefabs/TestEnemy"), 
													enemySpawnPoints[0].position, enemySpawnPoints[0].rotation);
			Debug.Log("GameObject " + tmpEnemy.gameObject.name + " created.");

			// Add the enemies to the characters list
			Enemies.Add(tmpEnemy);
			Characters.Add(tmpEnemy);
		}

		// Instantiate each enemy in the Enemy list
		for (int i=0; i<Enemies.Count; i++) {
			Sprite testSprite = Resources.Load("Sprites/knight2", typeof(Sprite)) as Sprite;
			SpriteRenderer sr = Enemies[i].GetComponent<SpriteRenderer>();
			sr.sprite = testSprite;
			Enemies[i].SetActive(true);

			//Instantiate (Enemies[i], enemySpawnPoints[0].position, enemySpawnPoints[0].rotation);
			//Debug.Log("Instantiate called.");
			Debug.Log("Starting health enemy" + i + ": " + Enemies[i].GetComponent<EnemyHealth>().startinghealth);
		}

		activeEnemyQueue = new Queue<string>();
		deadEnemyCount = 0;
	}

	void SpawnParty ()
	{
		// For heroes in the party, move them to the battlefield
		hero.GetComponent<Transform>().position = heroSpawnPoints[0].position;

		// Add the heroes to the characters list
		Characters.Add(hero);
		Heroes.Add(hero);

		activeHeroQueue = new Queue<string>();
		deadHeroCount = 0;
	}

	public void Fight()
	{
		Debug.Log("You chose FIGHT!");
		Enemies[0].GetComponent<EnemyHealth>().TakeDamage(7);

		int activeHeroIndex = ReturnCharacterIndex(activeHeroTag);
		Characters[activeHeroIndex].GetComponent<HeroAction>().Reset();
		fightButton.SetActive(false);
		activeHeroTag = "";
	}

	int ReturnCharacterIndex (string tag)
	{
		// Return the index number of an object in Characters for a given tag
		int idx = -1;
		for (int i=0; i<Characters.Count; i++)
		{
			if (Characters[i].gameObject.tag == tag) {
				idx = i;
			}
		}
		return idx;
	}
}



