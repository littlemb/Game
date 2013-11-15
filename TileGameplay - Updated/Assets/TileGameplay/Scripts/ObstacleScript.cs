using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
	
	//tiles taken up; ie. 1x1, 2x2, 2x1
	public int length;				// x-direction; positive to the right from the origin
	public int width;				// y-direction; positive in the up direction from the origin
	
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
		if(length == 1 && width == 1)
		{
			transform.position = GameObject.Find(tileName).transform.position; //move the object to its start position
			levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().isOccupied = true;
			levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().isOccupiedByObject = true;
			levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().SetObject(gameObject);
		}
		else
		{
			Vector3 newPosition = GameObject.Find(tileName).transform.position;
			
			newPosition.x += (1/length);
			newPosition.z += (1/width);
			
			transform.position = newPosition;
			for(int i=0; i<length; i++)
			{
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+(i*tileBoardLength)].GetComponent<GameTile>().isOccupied = true;
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+(i*tileBoardLength)].GetComponent<GameTile>().isOccupiedByObject = true;
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+(i*tileBoardLength)].GetComponent<GameTile>().SetObject(gameObject);
			}
			for(int i=0; i<width; i++)
			{
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+i].GetComponent<GameTile>().isOccupied = true;
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+i].GetComponent<GameTile>().isOccupiedByObject = true;
				levelManager.GetComponent<BoardManager>().tiles[tileIndex+i].GetComponent<GameTile>().SetObject(gameObject);
			}
//			levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().isOccupied = true;
//			levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().isOccupiedByObject = true;
//			levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().SetObject(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
