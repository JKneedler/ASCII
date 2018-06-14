using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour {

	public string itemName;
	public enum equipSlot {primary, secondary, head, shoulders, chest, gloves, legs, feet, X};
	public equipSlot slotPos;
	public enum invSlot {Weapon, Armor, Potion, Misc};
	public invSlot invSlotPos;
	public enum toolTypes {Sword, Axe, Shovel, Pickaxe, Hoe, X};
	public toolTypes toolType;
	public int damage;
	public int defense;
	public bool stackable;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
