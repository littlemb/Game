using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	//notes
	
	//active character holder -> the character we are currently moving (the character last clicked)
	public GameObject activeCharacter;
	
	//camera
	public Camera camera; //set inside editor
	
	//for raycasting and changing the color of tiles
	public GameObject prevHit;
	
	//bools to control player input
	private bool characterSelected; //has the player selected a character to move yet? the player cannot move a character until this value is true
	
	// Use this for initialization
	void Start () 
	{
		prevHit = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//player must select a character to move first
		//from there the player can either move the active character (if they have any valid moves) or select another character
		
		//select the character to move
		if(Input.GetMouseButtonDown(0))
		{
			MouseSelectCharacter();
		}	
		
		if(characterSelected)
		{
			MouseSelectTile();
			
			//select the active character's destination tile
			if(Input.GetMouseButtonDown(0))
			{
				MouseMoveCharacter();
			}
		}
	}
	
	//select the character we want to move
	//we don't raycast to hit a character, we raycast to hit a tile occupied by a character
	void MouseSelectCharacter()
	{
		RaycastHit clickCharTile;
		Ray rayCharTile = camera.ScreenPointToRay(Input.mousePosition);
		
		//looking for a tile that is occupied by a player character
		if(Physics.Raycast(rayCharTile, out clickCharTile))
		{
			if(clickCharTile.collider != null)
			{
				if(clickCharTile.collider.tag == "Tile" && clickCharTile.collider.gameObject.GetComponent<GameTile>().isOccupiedByPlayer)
				{
					//activeCharacter = clickCharTile.collider.gameObject.GetComponent<GameTile>().GetCharacter();
					//print ("Selected : " + activeCharacter.name);
									
					//check to see if we've selected a new character
					if(activeCharacter != null)
					{
						//activeCharacter has already been set - compare the two names
						if(activeCharacter.name == clickCharTile.collider.gameObject.GetComponent<GameTile>().GetCharacter().name)
						{
							//selected the same character, do nothing
							print ("Selected the same character.");
						}
						else
						{	
							//selected a new character
							print ("Selected a new character.");
							
							//before setting the new character, we must stop showing the moves of the old active character
							activeCharacter.SendMessage("DontShowYourMoves");
							
							//now we can set activeCharacter to the new character
							activeCharacter = clickCharTile.collider.gameObject.GetComponent<GameTile>().GetCharacter();
							
							NewCharacterSelected();
						}
					}
					else{
						//activeCharacter has not been set - set it to the character on the clicked tile
						activeCharacter = clickCharTile.collider.gameObject.GetComponent<GameTile>().GetCharacter();
						
						NewCharacterSelected();
					}
					activeCharacter.SendMessage("ShowYourMoves"); //use send message instead of GetComponent in order to use scripts with different names but identical function names
				}
			}
		}
	}
	
	//call this function whenever a new character is selected
	//sets prevHit back to null in order to start hovering on the new character's tiles
	void NewCharacterSelected()
	{
		characterSelected = true;
		prevHit = null;
	}
	
	//once we have an active character to move, we need to select their destination tile
	void MouseSelectTile()
	{
		RaycastHit hitTile;
		Ray rayTile = camera.ScreenPointToRay(Input.mousePosition);
		Debug.DrawLine(rayTile.origin, rayTile.direction * 10);
		if(Physics.Raycast(rayTile, out hitTile))
		{
			if(hitTile.collider != null && hitTile.collider.tag == "Tile" && hitTile.collider.gameObject.GetComponent<GameTile>().canMoveHere)
			{
				if(hitTile.collider.tag == "Tile" && hitTile.collider != null)
				{
					hitTile.collider.gameObject.GetComponent<GameTile>().ChangeToCursorMaterial();
				}
				
				if(prevHit != null)
				{
					if(hitTile.collider != null && prevHit.name != hitTile.collider.gameObject.name)
					{
						prevHit.GetComponent<GameTile>().ChangeToMoveMaterial();	
					}
				}
				
				if(hitTile.collider == null)
				{
					prevHit.GetComponent<GameTile>().ChangeToMoveMaterial();
					prevHit = null;
				}
				else
				{
					prevHit = hitTile.collider.gameObject;
				}	
			}
			else{
				if(prevHit != null)
				{
					prevHit.GetComponent<GameTile>().ChangeToMoveMaterial();
				}
			}
		}
	}
	
	//once we have an active character, we need to move them
	void MouseMoveCharacter()
	{
		RaycastHit clickTile;
		Ray rayTile = camera.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(rayTile, out clickTile))
		{
			if(clickTile.collider != null && clickTile.collider.tag == "Tile" && clickTile.collider.gameObject.GetComponent<GameTile>().canMoveHere)
			{
				print ("Clicked tile: " + clickTile.collider.gameObject.name);
				
				//turns off activeCharacters original moves
				activeCharacter.SendMessage("DontShowYourMoves");
				//turns off tile clicked to move to
				prevHit = null;
				//calls method from CharacterType1.cs and passes the clicked tile
				activeCharacter.SendMessage("MoveCharacter", clickTile.collider.gameObject.name);
				//no character active
				activeCharacter = null;
			}
		}
	}

}
