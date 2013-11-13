using UnityEngine;
using System.Collections;

public class AssaultPlayer : MonoBehaviour {
	
	GameObject assault;

	
	// Use this for initialization
	void Start () {
    	assault = GameObject.Find("PlayerAssault");
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnGUI () {
		
			
		//if statement activates gui if a player character is selected to allow player to initiate combat
		if (assault.GetComponent<CharacterType1>().GetCombatGUI())
		{
			GUI.Box(new Rect(1100,10,190,100), "Assault Specific Options");
			/*potentially use something like this if special abilities have to be activated
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
					combat = false;
				}
				else
				{
					combat = true;
				}
			}
			*/
			string characterName = gameObject.name;
			characterName = characterName.Substring(6);
			GUI.Label (new Rect (1120, 30, 180, 25), "Character Type: " + characterName);
			//Assault Specific options can go here
			
		}
	}
}
