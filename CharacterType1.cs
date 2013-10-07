using UnityEngine;
using System.Collections;

public class CharacterType1 : MonoBehaviour {
	
	//notes
	//make sure ALL characters and enemies are on the "Ignore Raycast" layer - when we raycast we will be raycasting for tiles (with "Ignore Raycast" the raycast will pass through the characters and hit the tiles)
	//all character MUST have unique names
	
	//initial tile - set inside the editor for every character and enemy
	public int startPosition;
	//position in array (equivalent to startPosition initially)
	private int tilesIndex;
	
	//level manager - found inside the Start() function
	private GameObject levelManager;
	
	//tile board length
	private int tileBoardLength; //gets set inside the Start() function
		
	//for movement algorithms
	public int distance = 3;
	
	// Use this for initialization
	void Start () 
	{
		levelManager = GameObject.Find("LevelManager");
		//if an initialPosition has not been set, set it to Tile1
		
		string tileName;
		tileName = "Tile" + startPosition.ToString();
		transform.position = GameObject.Find(tileName).transform.position; //move the character to its start position
		levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().isOccupied = true;
		levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().isOccupiedByPlayer = true;
		levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().SetCharacter(gameObject);
		tilesIndex = startPosition;

		tileBoardLength = levelManager.GetComponent<BoardManager>().tileBoardLength; //make sure we have the exact same value from BoardManager.cs
	}
	
	// Update is called once per frame
	void Update () 
	{
		/**Testing movement types**/
		/**
		if(Input.GetKeyDown("h"))
		{
			MovementType1(3);
		}
		if(Input.GetKeyDown("j"))
		{
			MovementType2();	
		}
		**/
	}
	
	/**called from CameraControl.MouseSelectCharacter() whenever the player selects this character (highlights the valid movement tiles)**/
	void ShowYourMoves()
	{
		//add logic here to change the distance value or the movement type
		//things such as health or items can change the distance variable
		//print ("Showing : " + gameObject.name + "'s moves.");
		MovementType1(distance, tilesIndex);
	}
	
	/**unhighlight the valid movement tiles**/
	void DontShowYourMoves()
	{
		//add logic here to change the distance value or the movement type
		//print ("No longer showing : " + gameObject.name + "'s moves.");
		MovementType1Inactive(distance, tilesIndex);
	}
	
	/**called from CameraControl when this player is the current active player**/
	void MovementType1(int units, int index)
	{
		//'+' style movement in units (up, down, left, right)
		//character cannot walk through an occupied tile
		//if a tile is occupied, the for loop breaks and the character must choose a different path
		
		int newIndex;
		int newUnits;
		
		//up movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetMoveHere(true);
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			MovementType1(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		//down movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetMoveHere(true);
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			MovementType1(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		//right movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetMoveHere(true);
			newUnits = units-1;
			newIndex = index+1;
			MovementType1(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		//left movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetMoveHere(true);
			newUnits = units-1;
			newIndex = index-1;
			MovementType1(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
	}
	
	
	
	
	
	
	
	/**the opposite of MovementType1 - this is called whenever this character is no longer active (called from CameraControl)**/
	void MovementType1Inactive(int units, int index)
	{
		//'+' style movement in units (up, down, left, right)
		//character cannot walk through an occupied tile
		//if a tile is occupied, the for loop breaks and the character must choose a different path
		
		int newIndex;
		int newUnits;
		
		//up movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().ChangeToDefaultMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetMoveHere(false);
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			MovementType1Inactive(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		//down movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().ChangeToDefaultMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetMoveHere(false);
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			MovementType1Inactive(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		//right movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().ChangeToDefaultMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetMoveHere(false);
			newUnits = units-1;
			newIndex = index+1;
			MovementType1Inactive(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		//left movement
		if(!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().ChangeToDefaultMaterial();
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetMoveHere(false);
			newUnits = units-1;
			newIndex = index-1;
			MovementType1Inactive(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
	}
	
	
	
	
	
	
	
	void MovementType2()
	{
		//'o' style movement - the character can move one unit outside of their current spot (up, down, left, right, and diagonals)
		//up 1
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength].GetComponent<GameTile>().SetMoveHere(true);
		}
		//down 1
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength].GetComponent<GameTile>().SetMoveHere(true);
		}
		//right 1
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex+1].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+1].GetComponent<GameTile>().SetMoveHere(true);
		}
		//left 1
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex-1].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+1].GetComponent<GameTile>().SetMoveHere(true);
		}
		//up right diagonal
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength+1].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength+1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength+1].GetComponent<GameTile>().SetMoveHere(true);
		}
		//up left diagonal
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength-1].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength-1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex+tileBoardLength-1].GetComponent<GameTile>().SetMoveHere(true);
		}
		//down right diagonal
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength+1].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength+1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength+1].GetComponent<GameTile>().SetMoveHere(true);
		}
		//down left diagonal
		if(!levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength-1].GetComponent<GameTile>().isOccupied)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength-1].GetComponent<GameTile>().ChangeToMoveMaterial();
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex-tileBoardLength-1].GetComponent<GameTile>().SetMoveHere(true);
		}
	}
}
