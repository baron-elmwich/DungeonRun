using UnityEngine;
using System.Collections;

public class VillageManager : MonoBehaviour {

	public GameObject villager;
	public Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
	
		Invoke ("Spawn", 0f);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Clicked");
	
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if(hitInfo)
			{
				Debug.Log( hitInfo.transform.name );
				Debug.Log("You clicked a villager.");
				// Here you can check hitInfo to see which collider has been hit, and act appropriately.
			}
		}
	}

//	void CastRay() {
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		RaycastHit2D hit;
//		if (Physics2D.Raycast(ray, out hit, 100)) {
//			Debug.DrawLine(ray.origin, hit.point);
//			Debug.Log("Hit object: " + hit.collider.gameObject.name);
//		}
//	}

	void Spawn ()
	{
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		
		Instantiate (villager, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

		Debug.Log("Spawned " + villager.gameObject.name);
	}
}
