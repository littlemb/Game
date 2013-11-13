using UnityEngine;
using System.Collections;

public class HeavyPlayer : MonoBehaviour {
	
	GameObject heavy;
	
	/*areaOfEffect for heavy class
	view heavy class as having some type of rocket/grenade
	an area of effect 1 would mean that for one square around the targeted area, there is some damage
	[ ][ ][!][ ][ ]
	[ ][!][!][!][ ]
	[!][!][*][!][!]
	[ ][!][!][!][ ]
	[ ][ ][!][ ][ ]
	if the starred square is targeted and areaOfEffect = 2, any character in the ! squares would receive damage
	the targeted character receieves the full damage from the heavy class, any character in the surrounding squares receives
	the heavy class damage multiplied by the areaOfEffectDamage multiplier
	*/
	public int areaOfEffect = 4;
	public float areaOfEffectDamage = 0.25f;
	public int rocketRange = 7;
	public int standardRange = 4;
	public GameObject theRocket;
	public bool rocket;
	
	// Use this for initialization
	void Start () {
		heavy = GameObject.Find("PlayerHeavy");
       	heavy.SendMessage("SetRange", standardRange);
		heavy.SendMessage("SetDamage", 50);
		heavy.SendMessage("SetDistance", 3);
		heavy.SendMessage("SetArmor", 2);
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnGUI () {
		
			
		//if statement activates gui if a player character is selected to allow player to initiate combat
		if (heavy.GetComponent<CharacterType1>().GetCombatGUI())
		{
			GUI.Box(new Rect(1100,10,190,130), "Heavy Specific Options");
			if(rocket)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.white;
			}
			
			//button to toggle missle
			if(GUI.Button(new Rect(1150,40,80,20), "Fire Rocket")) {
				if(heavy.GetComponent<CharacterType1>().GetCombat())
				{
					if(rocket)
					{
						rocket = false;
						heavy.SendMessage("DontShowYourMoves");
						heavy.SendMessage("SetRange", standardRange);
						heavy.SendMessage("ShowYourMoves");
					}
					else
					{
						rocket = true;
						heavy.SendMessage("SetRange", rocketRange);
						heavy.SendMessage("ShowYourMoves");
					}

				}
			}
			string characterName = gameObject.name;
			characterName = characterName.Substring(6);
			GUI.Label (new Rect (1120, 70, 180, 25), "Character Type: " + characterName);
			GUI.Label (new Rect (1120, 90, 180, 20), "Area Of Effect: " + areaOfEffect);
			GUI.Label (new Rect (1120, 110, 180, 25), "Area Damage: " + areaOfEffectDamage);
			
		}
	}
	
	public int GetAreaOfEffect()
	{
		return areaOfEffect;
	}
	
	public float GetAreaOfEffectDamage()
	{
		return areaOfEffectDamage;
	}
	
	public bool GetRocket()
	{
		return rocket;
	}
	
	public void SetRocket(bool newRocket)
	{
		rocket = newRocket;
	}
	
	public void FireRocket()
	{
		Vector3 rocketPosition = transform.position;
		rocketPosition.y += 2;
		GameObject tempRocket = (GameObject)Instantiate(theRocket,rocketPosition, Quaternion.identity);
	}
}
