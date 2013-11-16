using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
	
	/*
	 * The script is written to draw the bottom left corner of the object in the specified tile
	 * 	Then it just moves up and to the right when increasing the length and width
	 */
	
	//tiles taken up; ie. 1x1x4, 2x2x1, 2x1x1
	public int length;				// x-direction; to the right
	public int width;				// z-direction; up the board
	public int height;				// y-direction; straight out from the board
	
	public int tileIndex;
	private int tileBoardLength;
	
	private GameObject levelManager;

	// Use this for initialization
	void Start ()
	{
		levelManager = GameObject.Find("LevelManager");
		tileBoardLength = levelManager.GetComponent<BoardManager>().tileBoardLength;
		
		string tileName;
		tileName = "Tile" + tileIndex.ToString();
		
		Vector3 newPosition = GameObject.Find(tileName).transform.position;
		Vector3 newScale = GameObject.Find(tileName).transform.localScale;
		
		//object size will be set here
		newScale.x = (length*2.0f)-1;
		newScale.y = height;
		newScale.z = (width*2.0f)-1;
		
		//object position set here
		newPosition.x = newPosition.x + length - 1.0f;
		newPosition.z = newPosition.z + width - 1.0f;
		
		transform.localScale = newScale;
		transform.position = newPosition;
		
		for(int j=0; j<length; j++)
		{
			for(int i=0; i<width; i++)
			{
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+(i*tileBoardLength)+j].GetComponent<GameTile>().isOccupied = true;
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+(i*tileBoardLength)+j].GetComponent<GameTile>().isOccupiedByObject = true;
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+(i*tileBoardLength)+j].GetComponent<GameTile>().SetObject(gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//this could be made to be destroyed
		//shoot a rocket at it
		//however, this would require a free-aiming rocket rather than just on enemies
	}
}
