using UnityEngine;
using System.Collections;

public class CharacterType1 : MonoBehaviour {
	
	//notes
	//make sure ALL characters and enemies are on the "Ignore Raycast" layer - when we raycast we will be raycasting for tiles (with "Ignore Raycast" the raycast will pass through the characters and hit the tiles)
	//all character MUST have unique names
	
	

	
	//initial tile - set inside the editor for every character and enemy
	public int startPosition;
	//position in array (equivalent to startPosition initially)
	public int tilesIndex;
	
	//level manager - found inside the Start() function
	private GameObject levelManager;
	
	GameObject[] enemies;
	
	//tile board length
	private int tileBoardLength; //gets set inside the Start() function
		
	//for movement algorithms
	public int distance = 4;
	
	//for combat algorithms
	
	//firing range
	public int range = 6;
	
	//amount of damage character can do to target
	public float damage = 25;
	
	//multiplicative modifier for damage taken, damage taken is modified by 1/armor. 
	//If armor is 2, character takes half damage. If armor is 0.5, character takes double damage. 
	//Armor is 1 for standard assault class (ie they take normal damage)
	public float armor = 1;
	
	//bool used to determine whether combat button has been clicked. 
	//If true, selecting an enemy in range will attack them. 
	//If false, selecting an enemy in range will show their current statistics (health, armor, etc.)
	public bool combat = false;
	
	//bool used to determine if an enemy is in range (for heavy class area of effect)
	bool enemyInRange = false;
	
	//startingHealth is 100, current health is modified as damage is taken (or character is healed). Cannot exceed 100.
	public float startingHealth = 100;
	public float health;

	//Explosion
	public GameObject explosion;
	
	//for GUI/camera stuffs
	private bool combatGUI = false;

	
	//for turn management
	public bool hasMoved = false;
	
	//for moving animation
	public bool moving = false;
	public string tempTileName;
	public float distanceToTile;
	
	// Use this for initialization
	void Start () 
	{
		levelManager = GameObject.Find("LevelManager");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");

		//if an initialPosition has not been set, set it to Tile1
		
		string tileName;
		tileName = "Tile" + startPosition.ToString();
		transform.position = GameObject.Find(tileName).transform.position; //move the character to its start position
		levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().isOccupied = true;
		levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().isOccupiedByPlayer = true;
		levelManager.GetComponent<BoardManager>().tiles[startPosition].GetComponent<GameTile>().SetCharacter(gameObject);
		tilesIndex = startPosition;

		tileBoardLength = levelManager.GetComponent<BoardManager>().tileBoardLength; //make sure we have the exact same value from BoardManager.cs
		health = startingHealth;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(health > 100)
			health = 100;
		
		if(health <= 0)
		{
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = false;
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByPlayer = false;
			levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(null);
			Destroy(gameObject);
		}
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		
		
		
		if(moving)
		{
			MoveAnimation();
		}
		
		

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
	
	//handles any GUI updates associated with player characters.
	void OnGUI () {
		
		//if statement activates gui if a player character is selected to allow player to initiate combat
		if (combatGUI)
		{
			GUI.Box(new Rect(10,10,190,190), "Combat Options");
			//changes button color if combat is active
			if(combat)
			{
				GUI.color = Color.red;
			}
			else
			{
				GUI.color = Color.white;
			}
			//button to toggle combat
			if(GUI.Button(new Rect(60,40,80,20), "Combat")) {
				if(combat)
				{
					MovementType1(distance, tilesIndex);
					combat = false;
				}
				else
				{
					MovementType1Inactive(distance, tilesIndex);
					combat = true;
				}
			}
			//label to display stats
			string characterName = gameObject.name;
			characterName = characterName.Substring(6);
			GUI.Label (new Rect (20, 70, 180, 25), "Character Type: " + characterName);
			GUI.Label (new Rect (20, 90, 80, 20), "Health: " + health);
			GUI.Label (new Rect (20, 110, 80, 20), "Armor: " + armor);
			GUI.Label (new Rect (20, 130, 80, 25), "Damage: " + damage);
			GUI.Label (new Rect (20, 150, 120, 25), "Movement Range: " + distance);
			GUI.Label (new Rect (20, 170, 120, 25), "Attack Range: " + range);

			
			/*
			if(GUI.Button(new Rect(20,100,80,20), "Skip Move")) 
			{
				hasMoved = true;
				CameraControl.movesRemaining--;
				DontShowYourMoves();
			}
			*/
			
		}
	}
	
	/**called from CameraControl.MouseSelectCharacter() whenever the player selects this character (highlights the valid movement tiles)**/
	void ShowYourMoves()
	{
		//add logic here to change the distance value or the movement type
		//things such as health or items can change the distance variable
		//print ("Showing : " + gameObject.name + "'s moves.");
		if(!hasMoved)
		{
			combatGUI = true;
			MovementType1(distance, tilesIndex);
			ShowEnemiesInRange(range, tilesIndex);
			if(gameObject.name == "PlayerHeavy" && gameObject.GetComponent<HeavyPlayer>().GetRocket())
			{
				if(enemyInRange)
				{
					GameObject[] tiles;
					tiles = GameObject.FindGameObjectsWithTag("Tile");
					foreach(GameObject tile in tiles)
					{
						if(tile.GetComponent<GameTile>().GetShootHere())
						{
							int tempTileIndex = int.Parse(tile.name.Substring(4));
							ShowAreaOfEffect(gameObject.GetComponent<HeavyPlayer>().GetAreaOfEffect(),tempTileIndex);
						}
					}
				}
			}
			if(gameObject.name == "PlayerMedic")
			{
				ShowPlayersToHeal(gameObject.GetComponent<MedicPlayer>().GetHealRange(), tilesIndex);
			}
		}
	}
	
	/**unhighlight the valid movement tiles**/
	void DontShowYourMoves()
	{
		//add logic here to change the distance value or the movement type
		//print ("No longer showing : " + gameObject.name + "'s moves.");
		combatGUI = false;
		MovementType1Inactive(distance, tilesIndex);
		if(gameObject.name == "PlayerMedic")
		{
			GameObject[] tiles;
			tiles = GameObject.FindGameObjectsWithTag("Tile");
			foreach(GameObject tile in tiles)
			{
				if(tile.GetComponent<GameTile>().GetOccupiedByPlayer())
				{
					int tempTileIndex = int.Parse(tile.name.Substring(4));
					tile.GetComponent<GameTile>().ChangeToDefaultMaterial();
					tile.GetComponent<GameTile>().SetHealHere(false);
				}
			}
		}
		DontShowEnemiesInRange();
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
	
	void MoveCharacter(string newTileName)
	{
		levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = false;
		levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByPlayer = false;
		levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(null);
		
		//transform.position = GameObject.Find(newTileName).transform.position;
		
		tilesIndex = int.Parse(newTileName.Substring(4));
		levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupied = true;
		levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().isOccupiedByPlayer = true;
		levelManager.GetComponent<BoardManager>().tiles[tilesIndex].GetComponent<GameTile>().SetCharacter(gameObject);
		
		moving = true;
		tempTileName = "Tile" + tilesIndex.ToString();
		
		hasMoved = true;
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
	
	// new method - Chris
	//highlights enemy tiles when character is selected
	//modified by Tom on 10/31, only shows enemies if they fall within range
	void ShowEnemiesInRange(int units, int index)
	{
		
		int newIndex;
		int newUnits;
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy)
			{
				levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().ChangeToEnemyMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetShootHere(true);
				enemyInRange = true;
			}
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			ShowEnemiesInRange(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy)
			{
				levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().ChangeToEnemyMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetShootHere(true);
				enemyInRange = true;

			}
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			ShowEnemiesInRange(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy)
			{
				levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().ChangeToEnemyMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetShootHere(true);
				enemyInRange = true;
			}
			newUnits = units-1;
			newIndex = index+1;
			ShowEnemiesInRange(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy)
			{
				levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().ChangeToEnemyMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetShootHere(true);
				enemyInRange = true;
			}
			newUnits = units-1;
			newIndex = index-1;
			ShowEnemiesInRange(newUnits, newIndex);
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
	
	// new method - Chris
	void DontShowEnemiesInRange()
	{
		if(gameObject.name == "PlayerHeavy")
		{
			GameObject[] tiles;
			tiles = GameObject.FindGameObjectsWithTag("Tile");
			foreach(GameObject tile in tiles)
			{
				if(tile.GetComponent<GameTile>().GetShootHere())
				{
					int tempTileIndex = int.Parse(tile.name.Substring(4));
					DontShowAreaOfEffect(gameObject.GetComponent<HeavyPlayer>().GetAreaOfEffect(),tempTileIndex);
				}
			}
		}
		for(int i=0; i<enemies.Length; i++)
		{
			levelManager.GetComponent<BoardManager>().tiles[enemies[i].GetComponent<EnemyType1>().tilesIndex].GetComponent<GameTile>().ChangeToDefaultMaterial();
			levelManager.GetComponent<BoardManager>().tiles[enemies[i].GetComponent<EnemyType1>().tilesIndex].GetComponent<GameTile>().SetShootHere(false);
		}
		enemyInRange = false;
		
	}
	
	//new method - Tom
	void Combat(GameObject enemyCharacter)
	{
		if(combat)
		{
			float enemyHealth = enemyCharacter.GetComponent<EnemyType1>().GetHealth();
			float enemyArmor = enemyCharacter.GetComponent<EnemyType1>().GetArmor();
			float damageToApply = damage*(1/enemyArmor);
			enemyHealth = enemyHealth - damageToApply;
			enemyCharacter.GetComponent<EnemyType1>().SetHealth(enemyHealth);
			if(gameObject.name == "PlayerHeavy" && gameObject.GetComponent<HeavyPlayer>().GetRocket())
			{
				StartCoroutine( "goBoom", enemyCharacter.transform.position);
				float areaMultiplier = gameObject.GetComponent<HeavyPlayer>().GetAreaOfEffectDamage();
				CalculateAreaDamage(gameObject.GetComponent<HeavyPlayer>().GetAreaOfEffect(), enemyCharacter.GetComponent<EnemyType1>().tilesIndex);
				GameObject[] tiles;
				tiles = GameObject.FindGameObjectsWithTag("Tile");
				foreach(GameObject tile in tiles)
				{
					if(tile.GetComponent<GameTile>().GetCharacter()==enemyCharacter)
					{
						tile.GetComponent<GameTile>().SetAreaDamage(false);
					}
					if(tile.GetComponent<GameTile>().GetAreaDamage())
					{	
						if(tile.GetComponent<GameTile>().GetCharacter().tag == "Enemy")
						{
							GameObject enemyChar = tile.GetComponent<GameTile>().GetCharacter();
							enemyHealth = enemyChar.GetComponent<EnemyType1>().GetHealth();
							enemyArmor = enemyChar.GetComponent<EnemyType1>().GetArmor();
							damageToApply = damage*(1/enemyArmor)*areaMultiplier;
							enemyHealth = enemyHealth - damageToApply;
							enemyChar.GetComponent<EnemyType1>().SetHealth(enemyHealth);
						}
						else if(tile.GetComponent<GameTile>().GetCharacter().tag == "Player")
						{
							GameObject playerChar = tile.GetComponent<GameTile>().GetCharacter();
							float playerHealth = playerChar.GetComponent<CharacterType1>().GetHealth();
							float playerArmor = playerChar.GetComponent<CharacterType1>().GetArmor();
							damageToApply = damage*(1/playerArmor)*areaMultiplier;
							playerHealth = playerHealth - damageToApply;
							playerChar.GetComponent<CharacterType1>().SetHealth(playerHealth);
						}
					}
				}
				gameObject.GetComponent<HeavyPlayer>().FireRocket();
				gameObject.GetComponent<HeavyPlayer>().SetRocket(false);
			}
			combat = false;
			hasMoved = true;
		}
	
	}
	
	//new method - Tom
	void Heal(GameObject healCharacter)
	{
		if(gameObject.GetComponent<MedicPlayer>().GetHeal())
		{
			float healPlayerHealth = healCharacter.GetComponent<CharacterType1>().GetHealth();
			float newHealth = healPlayerHealth + gameObject.GetComponent<MedicPlayer>().GetHealAmount();
			healCharacter.GetComponent<CharacterType1>().SetHealth(newHealth);
			gameObject.GetComponent<MedicPlayer>().SetHeal(false);
			hasMoved = true;
		}
	
	}
	
	void MovedFalse()
	{
		hasMoved = false;
	}
	
	//Accessors and Mutators, for modifying different character types, not sure if we'll need them all or not but I went ahead and made them. tom
	
	public void SetRange(int newRange)
	{
		range = newRange;
	}
	public int GetRange()
	{
		return range;
	}
	
	public void SetDamage(float newDamage)
	{
		damage = newDamage;
	}
	float GetDamage()
	{
		return damage;
	}
	
	public void SetDistance(int newDistance)
	{
		distance = newDistance;
	}
	public int GetDistance()
	{
		return distance;
	}
	
	public void SetArmor(float newArmor)
	{
		armor = newArmor;
	}
	public float GetArmor()
	{
		return armor;
	}
	
	public void SetHealth(float newHealth)
	{
		health = newHealth;
	}
	public float GetHealth()
	{
		return health;
	}
	
	public bool GetCombatGUI()
	{
		return combatGUI;
	}
	public bool GetCombat()
	{
		return combat;
	}
	//show heavy class area of effect
	void ShowAreaOfEffect(int units, int index)
	{
		
		int newIndex;
		int newUnits;
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().ChangeToEnemyMaterial();
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			ShowAreaOfEffect(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().ChangeToEnemyMaterial();
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			ShowAreaOfEffect(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().ChangeToEnemyMaterial();
			newUnits = units-1;
			newIndex = index+1;
			ShowAreaOfEffect(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().ChangeToEnemyMaterial();
			newUnits = units-1;
			newIndex = index-1;
			ShowAreaOfEffect(newUnits, newIndex);
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
	
	void DontShowAreaOfEffect(int units, int index)
	{
		
		int newIndex;
		int newUnits;
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().ChangeToDefaultMaterial();
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			DontShowAreaOfEffect(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().ChangeToDefaultMaterial();
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			DontShowAreaOfEffect(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().ChangeToDefaultMaterial();
			newUnits = units-1;
			newIndex = index+1;
			DontShowAreaOfEffect(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().ChangeToDefaultMaterial();
			newUnits = units-1;
			newIndex = index-1;
			DontShowAreaOfEffect(newUnits, newIndex);
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
	
	void CalculateAreaDamage(int units, int index)
	{
		
		int newIndex;
		int newUnits;
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetAreaDamage(true);
			}
			StartCoroutine( "goBoom", levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].transform.position);
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			CalculateAreaDamage(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetAreaDamage(true);
			}
			StartCoroutine( "goBoom", levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].transform.position);
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			CalculateAreaDamage(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetAreaDamage(true);
			}
			StartCoroutine( "goBoom", levelManager.GetComponent<BoardManager>().tiles[index+1].transform.position);
			newUnits = units-1;
			newIndex = index+1;
			CalculateAreaDamage(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetAreaDamage(true);
			}
			StartCoroutine( "goBoom", levelManager.GetComponent<BoardManager>().tiles[index-1].transform.position);
			newUnits = units-1;
			newIndex = index-1;
			CalculateAreaDamage(newUnits, newIndex);
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
	
	IEnumerator goBoom (Vector3 boomPlace)
	{
    	// do something
    	yield return new WaitForSeconds(Random.Range(0.1f,1));
		Instantiate(explosion,boomPlace, Quaternion.identity);

	    // do something else
}
	
	void ShowPlayersToHeal(int units, int index)
	{
		
		int newIndex;
		int newUnits;
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().ChangeToHealMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index+tileBoardLength].GetComponent<GameTile>().SetHealHere(true);
			}
			newUnits = units-1;
			newIndex = index+tileBoardLength;
			ShowPlayersToHeal(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().ChangeToHealMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index-tileBoardLength].GetComponent<GameTile>().SetHealHere(true);
			}
			newUnits = units-1;
			newIndex = index-tileBoardLength;
			ShowPlayersToHeal(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}

		if((!levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupied || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByEnemy || 
			levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().ChangeToHealMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index+1].GetComponent<GameTile>().SetHealHere(true);
			}
			newUnits = units-1;
			newIndex = index+1;
			ShowPlayersToHeal(newUnits, newIndex);
		}
		else
		{
			//break;//the tile is occupied therefore the player cannot advance through this tile
		}
		
		if((!levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupied ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByEnemy ||
			levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer) && units>0)
		{
			if(levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().isOccupiedByPlayer)
			{
				levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().ChangeToHealMaterial();
				levelManager.GetComponent<BoardManager>().tiles[index-1].GetComponent<GameTile>().SetHealHere(true);
			}
			newUnits = units-1;
			newIndex = index-1;
			ShowPlayersToHeal(newUnits, newIndex);
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
