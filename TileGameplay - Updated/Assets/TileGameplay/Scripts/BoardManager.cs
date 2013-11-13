using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {
	
	public int tileBoardWidth; //set inside editor
	public int tileBoardLength; //set inside editor (e.g. a tileBoardLength of 10 means there will be a board of size 100 (10 x 10)
	public GameObject[] tiles;
	
	// Use this for initialization
	void Awake () 
	{
		//set the size of the array
		tiles = new GameObject[(tileBoardLength*tileBoardLength)+1];
		//the "+1" is used in order to have the tile numbers equal to their position in the array (e.g. Tile93 is at index 93 in the array)
		//this also eliminates the need to use "-1" every time a tile is referenced
		//tiles[0] should have nothing in it
		
		//put each tile into the tiles array by using their name and numbers
		for(int i = 1; i <= tileBoardWidth*tileBoardLength; i++)
		{
			//Tiles MUST be named "Tile" + their number (very important)
			//once the name has been set, this loop will use GameObject.Find(tileName) to locate the tile and place it into the array
			string tileName;
			tileName = "Tile" + i.ToString();
			//print ("Adding: " + tileName);
			tiles[i] = GameObject.Find(tileName);
		}
		
	}
}
