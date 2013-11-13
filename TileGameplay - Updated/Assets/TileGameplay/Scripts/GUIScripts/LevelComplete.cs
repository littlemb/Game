using UnityEngine;
using System.Collections;

public class LevelComplete : MonoBehaviour {

	public float spacing = 250;
	public float cellWidth = 150;
	public float cellHeight = 30;
	
	float buttonWidth = 200;
	float buttonHeight = 50;
	float leftSpacing = 150;
	float rightSpacing = 0;
	
	bool levelComplete = false;
	bool replay = false;
	bool characterOne = false;
	bool characterTwo = false;
	bool characterThree = false;
	bool characterFour = false;
	
	// Use this for initialization
	void Start () {
		levelComplete = true;
		replay = false;
		characterOne = false;
		characterTwo = false;
		characterThree = false;
		characterFour = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(levelComplete)
		{
			GUI.Box(new Rect(Screen.width/2 - 250, Screen.height/2 -  spacing, 500, 400),"Level Complete");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"Character 1");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"Character 2");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"Character 3");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2, cellWidth, cellHeight),"Character 4");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2, cellWidth, cellHeight),"# of Points");
			
			if(GUI.Button(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Replay"))
			{
				levelComplete = !levelComplete;
				replay = !replay;
				
			}
			
			if(GUI.Button (new Rect(Screen.width/2 + rightSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Continue"))
			{
				levelComplete = !levelComplete;
				characterOne = !characterOne;
			}
		}
		
		if(replay)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, 250, 200), "Replay");
			GUI.Label(new Rect(Screen.width/2 - buttonWidth/2 + 10, Screen.height/2 - buttonHeight/2 - 75, 240, buttonHeight), "Are you sure you want to replay the game? You will lose all of the skill points you have generated so far.");
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 50, Screen.height/2 - buttonHeight/2, buttonHeight, buttonHeight), "Yes"))
			{
				Application.LoadLevel("TestScene1");	
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 125, Screen.height/2 - buttonHeight/2, buttonHeight, buttonHeight), "No"))
			{
				replay = !replay;
				levelComplete = !levelComplete;
			}
		}
		
		if(characterOne)
		{
			GUI.Box(new Rect(Screen.width/2 - 250, Screen.height/2 -  spacing, 500, 400),"Character 1");
			GUI.Label(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"Points Available:");
			GUI.Label(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"Special Ability");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"Weapon Damage");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"Weapon Accuracy");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2, cellWidth, cellHeight),"Health/Armor");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2, cellWidth, cellHeight),"# of Points");
			
			if(GUI.Button(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Save Game"))
			{
				
				
			}
			
			if(GUI.Button (new Rect(Screen.width/2 + rightSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Next Character"))
			{
				characterOne = !characterOne;
				characterTwo = !characterTwo;
			}
		
		}
		
		if(characterTwo)
		{
			GUI.Box(new Rect(Screen.width/2 - 250, Screen.height/2 -  spacing, 500, 400),"Character 2");
			GUI.Label(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"Points Available:");
			GUI.Label(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"Special Ability");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"Weapon Damage");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"Weapon Accuracy");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2, cellWidth, cellHeight),"Health/Armor");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2, cellWidth, cellHeight),"# of Points");
			
			if(GUI.Button(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Save Game"))
			{
				
				
			}
			
			if(GUI.Button (new Rect(Screen.width/2 + rightSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Next Character"))
			{
				characterTwo = !characterTwo;
				characterThree = !characterThree;
			}
		}
		
		if(characterThree)
		{
			GUI.Box(new Rect(Screen.width/2 - 250, Screen.height/2 -  spacing, 500, 400),"Character 3");
			GUI.Label(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"Points Available:");
			GUI.Label(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"Special Ability");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"Weapon Damage");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"Weapon Accuracy");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2, cellWidth, cellHeight),"Health/Armor");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2, cellWidth, cellHeight),"# of Points");
			
			if(GUI.Button(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Save Game"))
			{
				
				
			}
			
			if(GUI.Button (new Rect(Screen.width/2 + rightSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Next Character"))
			{
				characterThree = !characterThree;
				characterFour = !characterFour;
			}
		}
		
		if(characterFour)
		{
			GUI.Box(new Rect(Screen.width/2 - 250, Screen.height/2 -  spacing, 500, 400),"Character 4");
			GUI.Label(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"Points Available:");
			GUI.Label(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  200, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"Special Ability");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"Weapon Damage");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"Weapon Accuracy");
			GUI.Box(new Rect(Screen.width/2 - leftSpacing, Screen.height/2, cellWidth, cellHeight),"Health/Armor");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  150, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  100, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2 -  50, cellWidth, cellHeight),"# of Points");
			GUI.Box(new Rect(Screen.width/2 + rightSpacing, Screen.height/2, cellWidth, cellHeight),"# of Points");
			
			if(GUI.Button(new Rect(Screen.width/2 - leftSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Save Game"))
			{
				
				
			}
			
			if(GUI.Button (new Rect(Screen.width/2 + rightSpacing, Screen.height/2 + 50, cellWidth, cellHeight),"Next Level"))
			{
				
				
			}
		}
		
	}
}
