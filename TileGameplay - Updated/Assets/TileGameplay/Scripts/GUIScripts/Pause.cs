using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	bool isPaused = false;
	bool loadScreen = false;
	bool exitScreen = false;
	float buttonWidth = 200;
	float buttonHeight = 50;
	
	// Use this for initialization
	void Start () {
		isPaused = false;
		loadScreen = false;
		exitScreen = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
		{
			isPaused = !isPaused;
			GetComponent<MouseLook>().enabled = false;
			Time.timeScale = 0.0f;
		}
		if(Input.GetKeyDown(KeyCode.X)& isPaused)
		{
			Application.LoadLevel("LevelCompleteScreen");
		}
	}
	
	void OnGUI()
	{
		if(isPaused)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, 250, 200), "Pause Menu");
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 75, buttonWidth, buttonHeight), "Resume"))
			{
				isPaused = !isPaused;	
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 25, buttonWidth, buttonHeight), "Load Game"))
			{
				loadScreen = !loadScreen;
				isPaused = !isPaused;
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 + 25, buttonWidth, buttonHeight), "Exit"))
			{
				exitScreen = !exitScreen;
				isPaused = !isPaused;
			}
		}
		if(loadScreen)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, 250, 200), "Load Menu");
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 75, buttonWidth, buttonHeight), "Load 1"))
			{
				Application.LoadLevel("TestScene1");	
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 - 25, buttonWidth, buttonHeight), "Load 2"))
			{
				Application.LoadLevel("TestScene1");	
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 25, Screen.height/2 - buttonHeight/2 + 25, buttonWidth, buttonHeight), "Back"))
			{
				loadScreen = !loadScreen;
				isPaused = !isPaused;	
			}
		}
		if(exitScreen)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight/2 - 100, 250, 200), "Exit Game Menu");
			GUI.Label(new Rect(Screen.width/2 - buttonWidth/2 + 10, Screen.height/2 - buttonHeight/2 - 75, 250, buttonHeight), "Are you sure you want to exit the game?");
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 50, Screen.height/2 - buttonHeight/2, buttonHeight, buttonHeight), "Yes"))
			{
				Application.LoadLevel("MainScreen");	
			}
			if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2 + 125, Screen.height/2 - buttonHeight/2, buttonHeight, buttonHeight), "No"))
			{
				exitScreen = !exitScreen;
				isPaused = !isPaused;
			}
		}
		
	}
	
}
