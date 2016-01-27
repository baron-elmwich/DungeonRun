using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	public static BattleManager instance = null;
	public float battleStartDelay = 2f; 

	// Scene Objects
	public GameObject stageImage;
	public Camera cam;
	public Transform[] enemySpawnPoints;
	public Transform[] heroSpawnPoints;
	public GameObject winPanel; //, losePanel in future
	public GameObject fightButton;
	private Text introText;
	private GameObject introImage;

	// Script Objects
	//private int area = 1;
	private int steps = 1;
	private int stageNum;
	private int enemyNum; // Temporary num for choosing random enemies
	private int formation; 
	private float time;
	private bool battleOver = false;
	private bool doingSetup = true;

	public GameObject hero;
	public GameObject enemy;
	private GameObject tmpEnemy; // For spawning enemies
	private GameObject targetEnemy; 
	// private GameObject targetHero; // For future enemy targetting

	public List<GameObject> Characters;
	public List<GameObject> Heroes;
	public List<GameObject> Enemies;
	private int deadEnemyCount;
	private int deadHeroCount;

	private HeroAction heroAction;
	private EnemyAction enemyAction;

	public Queue<string> activeHeroQueue;
	public Queue<string> activeEnemyQueue;
	private string activeHeroTag;
	private string activeEnemyTag;
	// Separate queue for enemy actions so they can attack while you are still making choices
	// Single queue for choices so attacks don't happen simultaneously
	// public Queue<???> choiceQueue;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject); // Save the BattleManager

		// Begin setup
		ChooseStage();
		SpawnParty();
		string chosenEnemies = ChooseEnemies();
		SpawnEnemies(chosenEnemies);
		PauseAllActionBars(); // 

		// May need to reorganize this part
		activeHeroTag = "";
		activeEnemyTag = "";
		heroAction = hero.GetComponent<HeroAction> ();
		fightButton.SetActive(false);

		InitGame();
		doingSetup = false;
		// Begin battle, move to Update
	}

	//
	private void OnLevelWasLoaded(int index) 
	{
		steps++;
		InitGame();
	}

	void InitGame ()
	{
		Debug.Log("InitGame was called.");
		doingSetup = true;

		introImage = GameObject.Find("IntroImage");
		introText = GameObject.Find("IntroText").GetComponent<Text>();
		introText.text = introText.text + "\n Steps: " + steps;
		introImage.SetActive(true);

		Invoke("HideIntroImage", battleStartDelay);
	}

	private void HideIntroImage() {
		introImage.SetActive(false);
		UnPauseAllActionBars();
	}
	
	// Update is called once per frame
	void Update () {
		if (!battleOver) {
			
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

			// If fightButton is active, player can choose target of attack
			if (fightButton.activeSelf) {
				// For now, highlight the targetted enemy as yellow
				targetEnemy.GetComponent<SpriteRenderer>().color = Color.yellow;
				if (Input.GetMouseButtonDown(0))
				{
					//Debug.Log("Mouse is down");
					Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
					RaycastHit2D hit=Physics2D.Raycast(rayPos, Vector2.zero, 0f);
					if (hit) {
						Debug.Log("Target changed to " + hit.transform.gameObject.name);
						targetEnemy.GetComponent<SpriteRenderer>().color = Color.white;
						targetEnemy = hit.transform.gameObject;
					}
				} 
			}

			/*
			// Check for full action bars on enemies
			for (int i=0; i<Enemies.Count; i++) {
				enemyAction = Enemies[i].GetComponent<EnemyAction> ();
				//if (characterAction.actionBarFull && !activeHeroQueue.Contains(Enemies[i].gameObject.tag)) {
				if (enemyAction.actionBarFull && !activeEnemyTag.Equals(Enemies[i].gameObject.tag)) {
					activeEnemyQueue.Enqueue(Enemies[i].gameObject.tag);
					Debug.Log("Enemy - Engueued " + Enemies[i].gameObject.tag);
					enemyAction.actionHandled = false; 
				} 
			} */

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
			/*
			// Check action queues for active enemies
			while ( activeEnemyQueue.Count > 0) {
				// Dequeue first entry from action queue
				activeEnemyTag = activeEnemyQueue.Dequeue();
				Debug.Log(activeEnemyTag + " dequeued.");
				int activeEnemyIndex = ReturnCharacterIndex(activeEnemyTag);

				// Active enemy deals damage to player

				hero.GetComponent<HeroHealth>().TakeDamage(5);

				Characters[activeEnemyIndex].GetComponent<EnemyAction>().Reset();
				activeEnemyTag = ""; 
			} */

			// Check choice queue 
			for (int i=0; i<Characters.Count; i++) {
				// Delay and execute the choices, delay before next choice is executed?
			}

			// Check for dead heroes

			// Check for dead enemies
			for (int i=0; i<Enemies.Count; i++) {
				if (Enemies[i].GetComponent<EnemyHealth>().Died()){
					RemoveDeadEnemy(Enemies[i]);
					deadEnemyCount++;
				}
			}

			//if (deadEnemyCount==Enemies.Count) {
			if (Enemies.Count == 0) {
				Debug.Log("You have defeated all enemies!");
				battleOver = true;
				winPanel.SetActive(true);
				//Restart();
			}
			
		} // battleOver
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

	string ChooseEnemies ()
	{
		//formation = Random.Range(1,4);
		formation = 2;
		// In testing, since formations come in 1, 2, or 4 enemies, set the enemyNum accordingly
		if (formation == 3) {
			enemyNum = 4;
		} else {
			enemyNum = formation;
		}

		return("Prefabs/Knight1");
	}

	void SpawnEnemies (string enemyStr)
	{	
		// Instantiate each enemy in the Enemy list
		int numEnemies = enemyNum;
		//GameObject tmpEnemy = new GameObject();
		for (int i=0; i<numEnemies; i++) {
			if (formation == 1) 
			{
				tmpEnemy = (GameObject) Instantiate(Resources.Load(enemyStr), 
					enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
			} else if (formation == 2){
				tmpEnemy = (GameObject) Instantiate(Resources.Load(enemyStr), 
					enemySpawnPoints[i+1].position, enemySpawnPoints[i+1].rotation);
			} else {
				tmpEnemy = (GameObject) Instantiate(Resources.Load(enemyStr), 
					enemySpawnPoints[i+3].position, enemySpawnPoints[i+3].rotation);
			}
			Debug.Log("GameObject " + tmpEnemy.gameObject.name + " created.");

			// Add the enemies to the characters list
			Enemies.Add(tmpEnemy);
			Characters.Add(tmpEnemy);
		}

		activeEnemyQueue = new Queue<string>();
		deadEnemyCount = 0;
		targetEnemy = Enemies[0]; // Default target is the first enemy
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

	// Wrapper for button to call method
	public void Fight() {
		StartCoroutine(FightRoutine(2));
	}

	IEnumerator FightRoutine(int delay)
	{
		fightButton.SetActive(false);
		Debug.Log("You chose FIGHT!");
		yield return new WaitForSeconds(delay);
		StartCoroutine(Move(hero.transform, 1, 2f));

		if (targetEnemy.name == "Hero") {
			// You can target a party member, but it switches back to an enemy afterwards
			targetEnemy.GetComponent<HeroHealth>().TakeDamage(11);
			targetEnemy.GetComponent<SpriteRenderer>().color = Color.white;
			targetEnemy = Enemies[0];
		} else {
			targetEnemy.GetComponent<EnemyHealth>().TakeDamage(11);
		}

		int activeHeroIndex = ReturnCharacterIndex(activeHeroTag);
		Characters[activeHeroIndex].GetComponent<HeroAction>().Reset();
		activeHeroTag = "";
	}

	// When attacking, move the attacker a short distance forward and back, direction = 1 or -1 for left/right
	IEnumerator Move(Transform t, int direction, float time) 
	{
		Vector3 offset = new Vector3(1f, 0f, 0f);

		//t.position = Vector3.Lerp(t.position, (t.position + offset * direction), Time.deltaTime * 2);
		t.position = Vector3.Lerp(t.position, (t.position + offset * direction), time);
		yield return new WaitForSeconds(0.1f);
		t.position = Vector3.Lerp(t.position, (t.position - offset * direction), time);
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

	void RemoveDeadEnemy (GameObject anEnemy) {
		Enemies.Remove(anEnemy);
		Characters.Remove(anEnemy);
		Destroy(anEnemy);

		// Reset the target enemy to the first position
		if (Enemies.Count>0) {
			targetEnemy = Enemies[0];
		} else {
			targetEnemy = null;
		}
	}

	public void PauseAllActionBars ()
	{
		for (int i=0; i<Enemies.Count; i++) {
			Enemies[i].GetComponent<EnemyAction>().Pause();
		}
		for (int i=0; i<Heroes.Count; i++) {
			Heroes[i].GetComponent<HeroAction>().Pause();
		}
	}

	public void UnPauseAllActionBars ()
	{
		for (int i=0; i<Enemies.Count; i++) {
			Enemies[i].GetComponent<EnemyAction>().UnPause();
		}
		for (int i=0; i<Heroes.Count; i++) {
			Heroes[i].GetComponent<HeroAction>().UnPause();
		}
	}

	public void Restart ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame ()
	{
		Debug.Log("Quit button clicked");
		//Application.Quit();
	}
}



