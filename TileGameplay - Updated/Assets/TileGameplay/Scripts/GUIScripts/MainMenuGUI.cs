using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
	
	float buttonWidth = 200;
	float buttonHeight = 50;
	float spacing = 100;
	float tableWidth = 250;
	float tableHeight = 200;
	bool loadScreen = false;
	bool mainScreen = false;
	bool exitScreen = false;
	public GUIStyle background;
	public GUIStyle backgroundBox;
	// Use this for initialization
	void Start () {
		loadScreen = false;
		mainScreen = true;
		exitScreen = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI ()
	{
		GUI.Box (new Rect(0,0,Screen.width,Screen.height),"",background);
		if(mainScreen){
			//Make Background
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, tableWidth, tableHeight), "Main Menu",backgroundBox);
			
			//New Game
			if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 75, buttonWidth, buttonHeight), "New Game"))
			{
				Application.LoadLevel("TestScene1");
			}
			
			//Load Game
			if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 25, buttonWidth, buttonHeight), "Load Game"))
			{
				mainScreen = !mainScreen;
				loadScreen = !loadScreen;
			}
			
			//Exit Game
			if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 + 25, buttonWidth, buttonHeight), "Exit"))
			{
				mainScreen = !mainScreen;
				exitScreen = !exitScreen;
			}
		}
		if(loadScreen)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, tableWidth, tableHeight), "Load Menu",backgroundBox);
			
			
			if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 75, buttonWidth, buttonHeight), "Load 1"))
			{
				Application.LoadLevel("TestScene1");
			}
			
			
			if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 25, buttonWidth, buttonHeight), "Load 2"))
			{
				Application.LoadLevel("TestScene1");
			}
			
			
			if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 + 25, buttonWidth, buttonHeight), "Back"))
			{
				loadScreen = !loadScreen;
				mainScreen = !mainScreen;
			}
		}
		if(exitScreen)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, tableWidth, tableHeight), "Exit Game Menu",backgroundBox);
			GUI.Label(new Rect(Screen.width/2 - buttonWidth/2 + 10, Screen.height/2 - buttonHeight/2 - 60, 250, buttonHeight), "Are you sure you want to exit the game?");
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 50, Screen.height/2 - buttonHeight/2, buttonHeight, buttonHeight), "Yes"))
			{
				Application.Quit();	
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 150, Screen.height/2 - buttonHeight/2, buttonHeight, buttonHeight), "No"))
			{
				exitScreen = !exitScreen;
				mainScreen = !mainScreen;
			}
		}
	}
}
