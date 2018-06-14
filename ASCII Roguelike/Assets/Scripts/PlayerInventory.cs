using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

	public enum invSlot
	{
		Weapons,
		Armors,
		Potions,
		Misc
	}
	public invSlot currentInvSlot;
	public List<GameObject> items = new List<GameObject> ();
	public List<int> itemAmts = new List<int> ();
	public Node invNode;
	public GameObject infoTab;
	public string[] itemInfoTitles = new string[5];


	// Use this for initialization
	void Start () {
		CreateInventoryNodes ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveInOutInventory(bool moveIn, int level, int index){
		MenuController menuC = gameObject.GetComponent<MenuController> ();
		if (moveIn) {
			if (level == 0) {
				menuC.ChangeCurrentNode (invNode, 0);
				menuC.level++;
			} else if (level == 1) {
				menuC.ChangeCurrentNode (invNode.children [index], 0);
				currentInvSlot = (invSlot)index;
				menuC.level++;
				infoTab.SetActive (true);
				UpdateInfo (menuC.index);
			}
		} else {
			if (level == 1) {
				menuC.ChangeCurrentNode (menuC.menuNode, (int)menuC.curCat);
				menuC.level--;
			} else if (level == 2) {
				menuC.ChangeCurrentNode (invNode, (int)currentInvSlot);
				menuC.level--;
				infoTab.SetActive (false);
			}
		}
	}

	public void UpdateInfo(int index){
		if (invNode.children [(int)currentInvSlot].children [0].item == null) {
			infoTab.SetActive (false);
		} else {
			GameObject item = invNode.children [(int)currentInvSlot].children [index].item;
			if (item != null) {
				ItemInfo it = item.GetComponent<ItemInfo> ();
				infoTab.transform.GetChild (0).GetComponent<Text> ().text = itemInfoTitles [0] + it.itemName;
				infoTab.transform.GetChild (1).GetComponent<Text> ().text = itemInfoTitles [1] + it.slotPos.ToString ();
				infoTab.transform.GetChild (2).GetComponent<Text> ().text = itemInfoTitles [2] + it.toolType.ToString ();
				infoTab.transform.GetChild (3).GetComponent<Text> ().text = itemInfoTitles [3] + it.damage;
				infoTab.transform.GetChild (4).GetComponent<Text> ().text = itemInfoTitles [4] + it.defense;
			}
		}
	}

	void CreateInventoryNodes(){
		invNode = new Node ("Inventory");
		for (int i = 0; i < System.Enum.GetValues(typeof(invSlot)).Length; i++) {
			invNode.AddChild (new Node (((invSlot)i).ToString()));
		}
		for (int i = 0; i < items.Count; i++) {
			ItemInfo it = items [i].GetComponent<ItemInfo> ();
			Node slot = invNode.children [(int)it.invSlotPos];
			string txt = "";
			if (itemAmts[i] > 1) {
				txt = it.itemName + " (" + itemAmts[i] + ")";
			} else {
				txt = it.itemName;
			}
			slot.children.Add (new Node (txt, items [i]));
		}
		for (int i = 0; i < invNode.children.Count; i++) {
			if (invNode.children[i].children.Count == 0) {
				invNode.children[i].children.Add (new Node ("No Items found here."));
			}
			Node child = invNode.children [i];
			switch (i) {
			case 0:
				child.options.Add (new MenuOption ("Equip", 'E'));
				child.options.Add(new MenuOption("Drop", 'Q'));
				break;
			case 1:
				child.options.Add (new MenuOption ("Equip", 'E'));
				child.options.Add(new MenuOption("Drop", 'Q'));
				break;
			case 2:
				child.options.Add (new MenuOption ("Use", 'E'));
				child.options.Add(new MenuOption("Drop", 'Q'));
				break;
			case 3:
				child.options.Add (new MenuOption ("Use", 'E'));
				child.options.Add(new MenuOption("Drop", 'Q'));
				break;
			}
		}
	}

	public void AddItem(GameObject addItem){
		ItemInfo itAdd = addItem.GetComponent<ItemInfo> ();
		int indexOfItem = ContainsItem (addItem);
		if (itAdd.stackable && indexOfItem > -1) {
			itemAmts [indexOfItem]++;
			if (itemAmts [indexOfItem] > 1) {
				Node slot = invNode.children [(int)itAdd.invSlotPos];
				for (int i = 0; i < slot.children.Count; i++) {
					if (slot.children [i].title.Substring (0, itAdd.itemName.Length) == itAdd.itemName) {
						slot.children [i].title = itAdd.itemName + " (" + itemAmts [indexOfItem] + ")";
					}
				}
			}
		} else {
			items.Add (addItem);
			itemAmts.Add (1);
			Node slot = invNode.children [(int)itAdd.invSlotPos];
			if (slot.children [0].item == null) {
				slot.children.Clear ();
			}
			slot.children.Add (new Node (itAdd.itemName, addItem));
		}

	}

	public void RemoveItem(Node itemNode){
		GameObject removeItem = itemNode.item;
		MenuController menuC = gameObject.GetComponent<MenuController> ();
		ItemInfo it = removeItem.GetComponent<ItemInfo> ();
		//items.Remove (removeItem);
		int indexOfItem = ContainsItem(removeItem);
		if (it.stackable && itemAmts [indexOfItem] > 1) {
			itemAmts [indexOfItem]--;
			Node slot = invNode.children [(int)it.invSlotPos];
			if (invNode.children.Contains (menuC.curNode)) {
				string txt = ""; 
				if (itemAmts [indexOfItem] > 1) {
					txt = it.itemName + " (" + itemAmts [indexOfItem] + ")";
				} else {
					txt = it.itemName;
				}
				int i = slot.children.IndexOf (itemNode);
				menuC.menuObject.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = txt;
			}
		} else {
			items.RemoveAt (indexOfItem);
			itemAmts.RemoveAt (indexOfItem);
			if (invNode.children.Contains (menuC.curNode)) {
				menuC.RemoveFromCurrentNode ();
			}
		}
		gameObject.GetComponent<EventBox> ().AddMessage ("Dropped " + it.itemName);
		UpdateInfo (menuC.index);
	}

	public bool AbleToAdd(){
		return true;
	}

	public int ContainsItem(GameObject searchItem){
		ItemInfo itSearch = searchItem.GetComponent<ItemInfo>();
		int indexForItem = -1;
		for (int i = 0; i < items.Count; i++) {
			ItemInfo it = items [i].GetComponent<ItemInfo> ();
			if (it.itemName == itSearch.itemName) {
				indexForItem = i;
			}
		}
		return indexForItem;
	}
		
	public void PressedButton(string optionName){
		MenuController menuC = gameObject.GetComponent<MenuController> ();
		ItemsForPickup curTile = gameObject.GetComponent<PlayerMovement> ().currentTile.GetComponent<ItemsForPickup> ();
		PlayerEquipped eq = gameObject.GetComponent<PlayerEquipped> ();
		int curInvSlotIndex = (int)currentInvSlot;
		switch (optionName) {
		case "Drop":
			GameObject itemToDrop = menuC.curNode.children [menuC.index].item;
			curTile.AddItemToTile (itemToDrop);
			RemoveItem (menuC.curNode.children [menuC.index]);
			if (invNode.children [curInvSlotIndex].children.Count == 0) {
				invNode.children [curInvSlotIndex].AddChild (new Node ("No Items found here."));
			}
			break;
		case "Equip":
			GameObject itemToEquip = menuC.curNode.children [menuC.index].item;
			ItemInfo it = itemToEquip.GetComponent<ItemInfo> ();
			for(int i = 0; i < eq.slots.Length; i++){
				if((int)it.slotPos == (int)eq.slots[i].slot){
					eq.EquipItem (itemToEquip, eq.slots [i].slot);
				}
			}
			break;
		}
	}
}
