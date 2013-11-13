using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {
	
	//occupy bools - determines if a tile is occupied, if so, what is it occupied by?
	public bool isOccupied; //is the tile occupied by a player, enemy, or object? 
	public bool isOccupiedByPlayer; //the tile is occupied, is it occupied by a player? (if true, isOccupied is true)
	public bool isOccupiedByEnemy; //the tile is occupied, is it occupied by an enemy? (if true, isOccupied is true)
	
	//the character associated with this tile
	//if this character dies or leaves this tile, set character to null
	public GameObject characterOnTile;
	
	public bool canMoveHere; //can a character move here?
	public bool canShootHere;
	public bool canHealHere;
	public bool canAreaDamage;
	
	//material variables
	public Material baseMat; //normal material
	public Material movementMat; //blue
	public Material invalidMat; //red
	public Material cursorMat; //green
	public Material healMat; //pink
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	public void SetMoveHere(bool param)
	{
		canMoveHere = param;
	}
	
	//new method - Chris
	public void SetShootHere(bool param)
	{
		canShootHere = param;
	}
	
	public void SetAreaDamage(bool param)
	{
		canAreaDamage = param;
	}
	
	public bool GetAreaDamage()
	{
		return canAreaDamage;
	}
	
	public bool GetShootHere()
	{
		return canShootHere;
	}
	
	public void SetHealHere(bool param)
	{
		canHealHere = param;
	}
	
	public bool GetHealHere()
	{
		return canHealHere;
	}
	
	public void ChangeToMoveMaterial()
	{
		renderer.material = movementMat;
		//canMoveHere = true;
	}
	
	public void ChangeToCursorMaterial()
	{
		renderer.material = cursorMat;
	}
	
	public void ChangeToDefaultMaterial()
	{
		renderer.material = baseMat;
	}
	
	//new method - Chris
	public void ChangeToEnemyMaterial()
	{
		renderer.material = invalidMat;
	}
	
	public void ChangeToHealMaterial()
	{
		renderer.material = healMat;
	}
	
	
	public GameObject GetCharacter()
	{
		return characterOnTile;
	}
	
	public void SetCharacter(GameObject character)
	{
		characterOnTile = character;	
	}
	
	public bool GetOccupiedByPlayer()
	{
		return isOccupiedByPlayer;
	}
}
