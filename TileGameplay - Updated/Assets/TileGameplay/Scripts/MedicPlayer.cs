using UnityEngine;
using System.Collections;

public class MedicPlayer : MonoBehaviour {
	
	GameObject medic;
	public bool heal;

	
	//the amount of healing a medic can do, and how close they have to be to the character
	public float healAmount = 25;
	public int healRange = 1;
	
	// Use this for initialization
	void Start () {
		medic = GameObject.Find("PlayerMedic");
       	medic.SendMessage("SetRange", 4);
		medic.SendMessage("SetDamage", 100);
		medic.SendMessage("SetDistance", 6);
		medic.SendMessage("SetArmor", 0.5);
		heal = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI () {
		
			
		//if statement activates gui if a player character is selected to allow player to initiate combat
		if (medic.GetComponent<CharacterType1>().GetCombatGUI())
		{
			GUI.Box(new Rect(1100,10,190,140), "Medic Specific Options");
			//potentially use something like this if special abilities have to be activated
			if(heal)
			{
				GUI.color = Color.blue;
			}
			else
			{
				GUI.color = Color.white;
			}
			
			//button to toggle combat
			if(GUI.Button(new Rect(1150,40,80,20), "Heal")) {
				if(heal)
				{
					heal = false;
				}
				else
				{
					heal = true;
				}
			}
			string characterName = gameObject.name;
			characterName = characterName.Substring(6);
			GUI.Label (new Rect (1120, 70, 180, 25), "Character Type: " + characterName);
			GUI.Label (new Rect (1120, 90, 180, 20), "Heal Amount: " + healAmount);
			GUI.Label (new Rect (1120, 110, 180, 25), "Heal Range: " + healRange);


		}
	}
	
	public int GetHealRange()
	{
		return healRange;
	}
	
	public float GetHealAmount()
	{
		return healAmount;
	}
	public void SetHealAmount(float newHealAmount)
	{
		healAmount = newHealAmount;
	}
	
	public bool GetHeal()
	{
		return heal;
	}
	public void SetHeal(bool newHeal)
	{
		heal = newHeal;
	}
}