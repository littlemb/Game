using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
	
	public int length;				//if the object takes up more than one tile, otherwise equals 1
	public int width;				//if the object takes up more than one tile, otherwise equals 1
	public int tileIndex;
	private int tileBoardLength;
	
	private GameObject levelManager;

	// Use this for initialization
	void Start ()
	{
		levelManager = GameObject.Find("LevelManager");
		
		string tileName;
		tileName = "Tile" + tileIndex.ToString();
		transform.position = GameObject.Find(tileName).transform.position; //move the character to its start position
		levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().isOccupied = true;
		levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().isOccupiedByObject = true;
		levelManager.GetComponent<BoardManager>().tiles[tileIndex].GetComponent<GameTile>().SetObject(gameObject);
		
		//***************add stuff if larger than 1x1
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
