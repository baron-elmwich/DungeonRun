using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	public float dragSpeed = -1;
	public int minX = -10; 
	public int maxX = 10; 
	public int minZ = -10; 
	public int maxZ = 10; 
	
	public int bottomMargin = 0; // if you have some icons at the bottom (like an RPG game) this will help preventing the drag action at the bottom
	
	public float orthZoomStep = 0.3f; 
	public int orthZoomMaxSize = 5; // FUTURE: set these based on background size?
	public int orthZoomMinSize = 4; 
	
	private bool orthographicView = true;
	private Vector3 dragOrigin;
	
	// Update is called once per frame
	void Update () {
		moveCamera();
		zoomCamera();
		//readjustCamera();
	}
	
	void moveCamera()
	{
		if (Input.GetMouseButtonDown(0))
		{    
			dragOrigin = Input.mousePosition;
			return;
		}
		
		if (!Input.GetMouseButton(0)) return;
		
		if(dragOrigin.y <= bottomMargin) return;
		
		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
		
		if(move.x > 0)
		{
			if(!isWithinRightBorder())
				move.x =0;
		}
		else
		{
			if(!isWithinLeftBorder())
				move.x=0;
		}
		
		if(move.z > 0)
		{
			if(!isWithinTopBorder())
				move.z=0;
		}
		else
		{
			if(!isWithinBottomBorder())
				move.z=0;
		}
		
		
		transform.Translate(move, Space.World);
	}
	
	void zoomCamera()
	{
		if(!isWithinBorders())
			return;
		
		// zoom out
		if (Input.GetAxis("Mouse ScrollWheel") <0)
		{
			if(orthographicView)
			{
				if (Camera.main.orthographicSize <=orthZoomMaxSize)
					Camera.main.orthographicSize += orthZoomStep;
			}
			else
			{
				if (Camera.main.fieldOfView<=150)
					Camera.main.fieldOfView +=5;
			}
		}
		// zoom in
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if(orthographicView)
			{
				if (Camera.main.orthographicSize >= orthZoomMinSize)
					Camera.main.orthographicSize -= orthZoomStep;            
			}
			else
			{
				if (Camera.main.fieldOfView>2)
					Camera.main.fieldOfView -=5;
			}
		}
	}

	void readjustCamera()
	{
		// If camera is dragged or zoomed past the background's edge, viewport should move back
	}
	
	bool isWithinBorders()
	{
		return ( isWithinLeftBorder() && isWithinBottomBorder() && isWithinRightBorder() && isWithinTopBorder() );
	}
	
	bool isWithinLeftBorder()
	{
		Vector3 currentTopLeftGlobal = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
		if(currentTopLeftGlobal.x > minX)
			return true;
		else
			return false;
		
	}
	
	bool isWithinRightBorder()
	{
		Vector3 currentBottomRightGlobal = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0));
		if(currentBottomRightGlobal.x < maxX)
			return true;
		else
			return false;
	}
	
	bool isWithinTopBorder()
	{
		Vector3 currentTopLeftGlobal = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height,0));
		if(currentTopLeftGlobal.z < maxZ)
			return true;
		else
			return false;
	}
	
	bool isWithinBottomBorder()
	{
		Vector3 currentBottomRightGlobal = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0));
		if(currentBottomRightGlobal.z > minZ)
			return true;
		else
			return false;
	}
	
}
