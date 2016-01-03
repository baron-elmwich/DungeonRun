using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Villager : MonoBehaviour {

	public Canvas canvas;
	public Text text;
//	private Vector2 spriteDim;
//	private BoxCollider2D bc2d;
	
	// Use this for initialization
	void Start () 
	{
//		canvas.enabled = false;
		// Get sprite dimensions to set the size of the collider
//		spriteDim = GetComponent<SpriteRenderer>().sprite.bounds.size;
//		bc2d = GetComponent<BoxCollider2D>();
//		bc2d.size = spriteDim;

		text = GetComponent<Text>();
		Debug.Log(text.text);
	}

	void Update () 
	{

	}

	void OnMouseDown () 
	{
		canvas.enabled = true;
	}

	void OnMouseUp () 
	{
		canvas.enabled = false;
	}
}
