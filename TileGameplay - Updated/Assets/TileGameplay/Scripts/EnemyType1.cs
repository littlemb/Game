using UnityEngine;
using System.Collections;

public class EnemyType1 : MonoBehaviour {

    //notes
    //make sure ALL characters and enemies are on the "Ignore Raycast" layer - when we raycast we will be raycasting for tiles (with "Ignore Raycast" the raycast will pass through the characters and hit the tiles)
    //all character MUST have unique names
    

    
    //initial tile - set inside the editor for every character and enemy
    public int startPosition;
    //position in array (equivalent to startPosition initially)
    public int tilesIndex;
    
    //level manager - found inside the Start() function
    private GameObject levelManager;
    
    //tile board length
    private int tileBoardLength; //gets set inside the Start() function
            
    //for movement algorithms
    public int distance = 3;
    
    //for GUI stuffs
    private bool enemySelected;
    
    //for combat
    public float startingHealth = 100;
    public int range = 5;
    public float armor = 1;
    public float damage = 15;
    public float health;
    
    //for AI stuff
    public bool hasMoved = false;
    GameObject[] players;
	private int playerCount;

	//for moving animation
	public bool moving = false;
	public string tempTileName;
	public float distanceToTile;

    
    // Use this for initialization
    void Start ()
    {
            levelManager = GameObject.Find("LevelManager");
            //if an initialPosition has not been set, set it to Tile1
            
            string tileName;
            tileName = "Tile" + startPosition.ToString();
            transform.position = GameObject.Find(tileName).transform.position; //move the character to its start position
            levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().isOccupied = true;
            levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().isOccupiedByEnemy = true;
            levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().SetCharacter(gameObject);
            tilesIndex = startPosition;

            tileBoardLength = levelManager.GetComponent<BoardManager>().tileBoardLength; //make sure we have the exact same value from BoardManager.cs
            health = startingHealth; //make sure current health is equal to starting health
            players = GameObject.FindGameObjectsWithTag("Player");

    }
    
    // Update is called once per frame
    void Update ()
    {
		if(health <= 0)
		{
		        levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = false;
		        levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByEnemy = false;
		        levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(null);
		        Destroy(gameObject);
		}
		players = GameObject.FindGameObjectsWithTag("Player");
		playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
		
		transform.LookAt(ClosestPlayer().transform.position);
		
		if(moving)
		{
			MoveAnimation();
		}
    }
	
	GameObject ClosestPlayer()
	{
		float minDistance = Vector3.Distance(transform.position, players[0].transform.position);
		GameObject closestPlayer = players[0];
		for(int i=1; i<playerCount; i++)
		{
			if(minDistance > Vector3.Distance(transform.position, players[i].transform.position))
			{
				minDistance = Vector3.Distance(transform.position, players[i].transform.position);
				closestPlayer = players[i];
			}
		}
		return closestPlayer;
	}
    
    // New Method - Tom
    // GUI stuffs
    void OnGUI () {
            
            //if statement activates gui if an enemy is selected
            if (enemySelected)
            {
                    GUI.Box(new Rect(10,10,130,130), "Enemy Info");
                    
                    //label to display health
                    GUI.Label (new Rect (20, 40, 80, 20), "Health: " + health);
                    GUI.Label (new Rect (20, 60, 80, 20), "Armor: " + armor);
                    GUI.Label (new Rect (20, 80, 80, 25), "Damage: " + damage);
                    GUI.Label (new Rect (20, 100, 120, 25), "Movement Range: " + distance);
                    GUI.Label (new Rect (20, 120, 120, 25), "Attack Range: " + range);
                    
            }
    }
    
    // New Method - Tom
    // Called instead of show your moves when selecting enemy, shows enemy health
    void ShowData()
    {
            enemySelected = true;
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
            //MovementType1Inactive(distance, tilesIndex);
            enemySelected = false;
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
    

    //new method - Chris
    //turns off connections of character to previous tile and sets them for the new tile
    void MoveCharacter(string newTileName)
    {
	    //turn off old position first so there is still reference to it
	    levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = false;
	    levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByEnemy = false;
	    levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(null);
	    
	    //transform.position = GameObject.Find(newTileName).transform.position; //move the character to its new position
	
	    tilesIndex = int.Parse(newTileName.Substring(4));
	    levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = true;
	    levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByEnemy = true;
	    levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(gameObject);
		
		moving = true;
		tempTileName = "Tile" + tilesIndex.ToString();
    }

	void MoveAnimation()
	{
		distanceToTile = Vector3.Distance(transform.position, GameObject.Find(tempTileName).transform.position);
		transform.position = Vector3.MoveTowards(transform.position, GameObject.Find(tempTileName).transform.position, 5.0f*Time.deltaTime);
		if(distanceToTile < 0.01f)
		{
			transform.position = GameObject.Find(tempTileName).transform.position;
			moving = false;
		}
	}
    
    void FindPlayersInRange(int units, int index)
    {
            
            int newIndex;
            int newUnits;
            
            if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied ||
                    levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
            {
                    if(levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer)
                    {
                            levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetShootHere(true);
                    }
                    newUnits = units-1;
                    newIndex = index+tileBoardLength;
                    FindPlayersInRange(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }

            if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied ||
                    levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
            {
                    if(levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer)
                    {
                            levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetShootHere(true);
                    }
                    newUnits = units-1;
                    newIndex = index-tileBoardLength;
                    FindPlayersInRange(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }

            if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied ||
                    levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
            {
                    if(levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer)
                    {
                            levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetShootHere(true);
                    }
                    newUnits = units-1;
                    newIndex = index+1;
                    FindPlayersInRange(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }
            
            if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied ||
                    levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
            {
                    if(levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer)
                    {
                            levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetShootHere(true);
                    }
                    newUnits = units-1;
                    newIndex = index-1;
                    FindPlayersInRange(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }
            
            /*
            Chris's original work, left here for posterity.
            for(int i=0; i<enemies.Length; i++)
            {
                    levelManager.GetComponent<BoardManager>().tiles[enemies[i].GetComponent<EnemyType1>().tilesIndex].GetComponent<GameTile>().ChangeToEnemyMaterial();
                    levelManager.GetComponent<BoardManager>().tiles[enemies[i].GetComponent<EnemyType1>().tilesIndex].GetComponent<GameTile>().SetShootHere(true);
            }
            */
    }
    
    void DontFindPlayersInRange()
    {
            for(int i=0; i<players.Length; i++)
            {
                    levelManager.GetComponent<BoardManager>().tiles[players[i].GetComponent<CharacterType1>().tilesIndex].GetComponent<GameTile>().SetShootHere(false);
            }                
    }
    
    
    // new method - Tom
    // returns health
    public float GetHealth()
    {
            return health;
    }
    // new method - Tom
    // sets health
    public void SetHealth(float newHealth)
    {
            health = newHealth;
    }
    // new method - Tom
    // returns damage
    public float GetDamage()
    {
            return damage;
    }
    public void SetDamage(float newDamage)
    {
            damage = newDamage;
    }
    
    public float GetArmor()
    {
            return armor;
    }
    public void SetArmor(float newArmor)
    {
            armor = newArmor;
    }

    void FindClosestMoveSpot(int units, int index)
    {
            
            int newIndex;
            int newUnits;
            
            if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied) && units>0)
            {
                    levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetMoveHere(true);
                    newUnits = units-1;
                    newIndex = index+tileBoardLength;
                    FindClosestMoveSpot(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }

            if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied) && units>0)
            {
                    levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetMoveHere(true);
                    newUnits = units-1;
                    newIndex = index-tileBoardLength;
                    FindClosestMoveSpot(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }

            if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied) && units>0)
            {
                    levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetMoveHere(true);
                    newUnits = units-1;
                    newIndex = index+1;
                    FindClosestMoveSpot(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }
            
            if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied) && units>0)
            {
                    levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetMoveHere(true);
                    newUnits = units-1;
                    newIndex = index-1;
                    FindClosestMoveSpot(newUnits, newIndex);
            }
            else
            {
                    //break;//the tile is occupied therefore the player cannot advance through this tile
            }
            
    }
    
    void DontFindClosestMoveSpot()
    {
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach(GameObject tile in tiles)
            {
                    if(tile.GetComponent<GameTile>().canMoveHere == true)
                    {
                            tile.GetComponent<GameTile>().SetMoveHere(false);
                    }
            }                
    }
    
    
    void EnemyAI()
    {
            /*
             * AI Process of Thought
             *
             * 1) Shoot player if in range
             * 2) Move towards enemy
             * 3)
             */
            
            GameObject closePlayer = new GameObject();
            float closest = 10000000;
            foreach (GameObject player in players)
            {
                    Vector3 pos = player.transform.position;
                    float dist = Vector3.Distance(pos, transform.position);
                    if(dist < closest)
                    {
                            closest = dist;
                            closePlayer = player;
                    }
            }                
            FindPlayersInRange(range, tilesIndex);
            GameObject[] tiles;
            tiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach (GameObject tile in tiles)
            {
                    if(tile.GetComponent<GameTile>().GetShootHere())
                    {
                            if(tile.GetComponent<GameTile>().GetCharacter() == closePlayer)
                            {
                                    float playerHealth = closePlayer.GetComponent<CharacterType1>().GetHealth();
                                    float playerArmor = closePlayer.GetComponent<CharacterType1>().GetArmor();
                                    float damageToApply = damage*(1/playerArmor);
                                    playerHealth = playerHealth - damageToApply;
                                    closePlayer.GetComponent<CharacterType1>().SetHealth(playerHealth);
                                    hasMoved = true;
                            }
                    }
            }
            DontFindPlayersInRange();
            if(!hasMoved)
            {
                    FindClosestMoveSpot(distance, tilesIndex);
                    closest = 10000000;
                    GameObject closeTile = new GameObject();
                    foreach (GameObject tile in tiles)
                    {
                            if(tile.GetComponent<GameTile>().canMoveHere == true)
                            {
                                    Vector3 tilePos = tile.transform.position;
                                    Vector3 playerPos = closePlayer.transform.position;
                                    float dist = Vector3.Distance(tilePos, playerPos);
                                    if(dist < closest)
                                    {
                                            closest = dist;
                                            closeTile = tile;
                                    }
                            }
                    }
                    print (gameObject.name + " " + closeTile.name);
                    MoveCharacter(closeTile.name);
                    DontFindClosestMoveSpot();
            }
            hasMoved = false;
            /*
            string newTileName;
            bool emptyTile = false;
            int tempTileIndex = tilesIndex;
            //turn off old position first so there is still reference to it
            levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = false;
            levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByEnemy = false;
            levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(null);
            
            int range = Random.Range(1,2);
            int i = Random.Range(0,3);
            while(emptyTile == false)
            {
                    if(i == 0)
                    {
                            tempTileIndex = tempTileIndex + range;
                    }
                    else if(i == 1)
                    {
                            tempTileIndex = tempTileIndex - range;
                    }
                    else if(i == 2)
                    {
                            tempTileIndex = tempTileIndex + range*tileBoardLength;
                    }
                    else
                    {
                            tempTileIndex = tempTileIndex - range*tileBoardLength;
                    }
                    if(levelManager.GetComponent<BoardManager>().tiles[tempTileIndex].GetComponent<GameTile>().isOccupied == false)
                    {
                            emptyTile = true;
                            tilesIndex = tempTileIndex;
                    }
            }
                    
            newTileName = "Tile" + tilesIndex;
            transform.position = GameObject.Find(newTileName).transform.position; //move the character to its new position
            levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = true;
            levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByEnemy = true;
            levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(gameObject);
            */
    }
        
}