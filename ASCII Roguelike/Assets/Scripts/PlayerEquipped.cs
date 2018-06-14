using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipped : MonoBehaviour {

	[System.Serializable]
	public class EquipSlot{
		public EquipSlots slot;
		public string title;
		public GameObject equippedItem;
	}

	public enum EquipSlots
	{
		primary,
		secondary,
		head,
		shoulders,
		chest,
		gloves,
		legs,
		feet
	}

	public EquipSlot[] slots = new EquipSlot[8];
	public Node equipNode;

	// Use this for initialization
	void Start () {
		CreateEquipNode ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateEquipNode(){
		PlayerStats stat = gameObject.GetComponent<PlayerStats> ();
		equipNode = new Node ("Equipped");
		equipNode.options.Add (new MenuOption ("Unequip", 'Q'));
		for (int i = 0; i < slots.Length; i++) {
			string txt = "";
			if (slots [i].equippedItem != null) {
				ItemInfo it = slots [i].equippedItem.GetComponent<ItemInfo> ();
				txt = slots [i].title + it.itemName;
			} else {
				txt = slots [i].title + "-----";
			}
			equipNode.AddChild (new Node (txt));
		}
		stat.AddStatsToEquippedNode (equipNode);
	}

	public void EquipItem(GameObject equipItem, EquipSlots slot){
		EventBox ev = gameObject.GetComponent<EventBox> ();
		ItemInfo it = equipItem.GetComponent<ItemInfo> ();
		int slotIndex = (int)slot;
		slots [slotIndex].equippedItem = equipItem;
		equipNode.children [slotIndex].title = slots [slotIndex].title + it.itemName;
		ev.AddMessage ("Equipped the " + it.itemName + " to " + slot.ToString());
	}

	public void MoveInOutEquipped(bool moveIn, int level){
		MenuController menuC = gameObject.GetComponent<MenuController> ();
		PlayerStats stat = gameObject.GetComponent<PlayerStats> ();
		if (moveIn && level == 0) {
			stat.UpdateStatNodes (equipNode);
			menuC.ChangeCurrentNode (equipNode, 0);
			menuC.endPoint = slots.Length;
			menuC.level++;
		} else if(!moveIn && level == 1) {
			menuC.ChangeCurrentNode (menuC.menuNode, (int)menuC.curCat);
			menuC.level--;
		}
	}

	public void Unequip(EquipSlots slot){
		MenuController menuC = gameObject.GetComponent<MenuController> ();
		int slotIndex = (int)slot;
		ItemInfo it = slots [slotIndex].equippedItem.GetComponent<ItemInfo> ();
		gameObject.GetComponent<EventBox> ().AddMessage ("Unequipped the " + it.itemName);
		slots [slotIndex].equippedItem = null;
		equipNode.children [slotIndex].title = slots [slotIndex].title + "-----";
		if (menuC.curNode == equipNode) {
			menuC.DestroyOldTiles ();
			menuC.DestroyOldOptions ();
			menuC.RefreshMenuView ();
		}
	}

	public void PressedButton(int optionIndex){
		MenuController menuC = gameObject.GetComponent<MenuController> ();
		//Unequip
		if (equipNode.options [optionIndex].title == "Unequip") {
			if (slots [menuC.index].equippedItem != null) {
				Unequip ((EquipSlots)menuC.index);
			}
		}
	}
}
