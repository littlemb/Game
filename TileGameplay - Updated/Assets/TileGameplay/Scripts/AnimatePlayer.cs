using UnityEngine;
using System.Collections;

public class AnimatePlayer : MonoBehaviour {

	private GameObject camera;
	
	//ZoomOnPlayer
	private GameObject playerObject;
	private Transform playerPivot;
	
	private bool zoomedIn = false;
	private bool zooming = false;
	private int zoomCount = 0;
	private float zoomSpeed = 45.0f;
	//giving the over the shoulder look
	private int zoomRotCount = 0;
	private float zoomRotSpeed = 45.0f;
	
	private bool zoomingOut = false;
	//will reverse the zoomCount and decrement back to zero to return to original position
	
	private bool rotating = false;
	private int rotateCount = 0;
	private float rotateSpeed = 90.0f;
	
	private int position = 0;					// position 0-3;	rotate back to position 0 before zooming back out
												//					zoom out decrementing the count when zooming in

	// Use this for initialization
	void Start ()
	{
		camera = GameObject.Find("Main Camera");
	}
	
	// 0.02 fixed timestep
	void FixedUpdate ()
	{
		if(Input.GetKeyDown("z") )
			//|| (camera.GetComponent<CameraControl>().activeCharacter.GetComponent<CharacterType1>().combat && !zoomedIn)
			//|| 	(!camera.GetComponent<CameraControl>().activeCharacter.GetComponent<CharacterType1>().combat && zoomedIn)
		{
			playerObject = camera.GetComponent<CameraControl>().activeCharacter;
			playerPivot = playerObject.transform;
			if(zooming)
			{
				zooming = false;
			}
			else
				zooming = true;
		}
		
		if(Input.GetKeyDown("x"))
		{
			if(zoomedIn)
			{
				if(rotating)
					rotating = false;
				else
				{
					rotating = true;
					position = (position+1)%4;
				}
			}
		}
		
		if(zooming)
		{
			if(!zoomedIn)
			{
				if(zoomCount < 250)	//5 seconds
				{
					MoveTowardsPlayer(playerObject);
					zoomCount++;
				}
				//else
				//{
				//	zoomingIn = false;
				//	zoomedIn = true;
				//	zoomCount = 0;
				//}
				
				if(zoomRotCount < 10)
				{
					ZoomRotIn(playerObject);
					zoomRotCount++;
				}
			}
			else
			{
				if(zoomCount > 0)
				{
					//if position 0, then proceed
					//else, rotate until position = 0
					//make sure zoom doesn't begin until final rotation is completed (although this might work)
					MoveAwayPlayer(playerObject);
					zoomCount--;
				}
				else
				{
					zooming = false;
					zoomedIn = false;
					zoomCount = 0;
					zoomRotCount = 0;
				}
				
				if(zoomRotCount < 10)
				{
					ZoomRotOut(playerObject);
					zoomRotCount++;
				}
			}
		}
		
		if(rotating)
		{
			//add stuff here
			//record position
			//return to position 0 when zooming back out
			
			if(rotateCount < 50)
			{
				RotateAroundPivot(playerObject);
				rotateCount++;
			}
			else
			{
				rotating = false;
				rotateCount = 0;
			}
		}
	}
	
	
	public void RotateAroundPivot(GameObject target)
	{
		transform.RotateAround(target.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
	}
	
	// move in an over-the-shoulder view of player, not right behind
	//create a transform that is relative to the activeCharacter but sit to the right and above it
	
	//should turn of move tiles while zoomed in
	public void MoveTowardsPlayer(GameObject target)
	{
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, zoomSpeed*Time.deltaTime);
		if(Vector3.Distance(transform.position, target.transform.position) < 5.0)
		{
			//transform.position = target.transform.position;
			zooming = false;
			zoomedIn = true;
			zoomRotCount = 0;
			//zoomCount = 0;
		} 
	}
	
	public void MoveAwayPlayer(GameObject target)
	{
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, -zoomSpeed*Time.deltaTime);

	}
	
	public void ZoomRotIn(GameObject target)
	{
		transform.RotateAround(target.transform.position, Vector3.right, -zoomRotSpeed * Time.deltaTime);
	}
				
	public void ZoomRotOut(GameObject target)
	{
		transform.RotateAround(target.transform.position, Vector3.right, zoomRotSpeed * Time.deltaTime);
	}
}