using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsForPickup : MonoBehaviour {

	public bool locked;
	public List<Object> items = new List<Object>();
	public Node pickUpRoot;


	void Start(){
		CreatePickUpNode ();
	}

	public void TakeItem(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		MenuController menuC = player.GetComponent<MenuController> ();
		PlayerInventory inv = player.GetComponent<PlayerInventory> ();
		if (inv.AbleToAdd ()) {
			inv.AddItem ((GameObject)items [menuC.index]);
			menuC.RemoveFromCurrentNode ();
			items.RemoveAt (menuC.index);
			if (items.Count == 0) {
				gameObject.GetComponent<TileInfo> ().SetToNormalTile();
			}
		}
	}

	public void AddItemToTile(GameObject item){
		items.Add (item);
		pickUpRoot.AddChild (new Node (item.GetComponent<ItemInfo> ().itemName));
		if (gameObject.GetComponent<TileInfo> ().app != TileInfo.application.Inventory) {
			gameObject.GetComponent<TileInfo> ().SetToPickupTile ();
		}
	}

	public void CreatePickUpNode(){
		pickUpRoot = new Node ("Pick Up Root");
		pickUpRoot.children.Clear ();
		for (int i = 0; i < items.Count; i++) {
			GameObject item = (GameObject)items [i];
			ItemInfo info = item.GetComponent<ItemInfo> ();
			pickUpRoot.children.Add (new Node(info.itemName));
		}
		pickUpRoot.options.Add (new MenuOption ("Take", 'E'));
	}

	public void OpenTileInventory(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		MenuController menuC = player.GetComponent<MenuController> ();
		CreatePickUpNode ();
		menuC.ChangeCurrentNode (pickUpRoot, 0);
		menuC.altMenuObject = gameObject;
		menuC.curCat = MenuController.categories.PickUpScreen;
		menuC.OpenCloseMenuScreen ();
	}

	public void PressedButton(int optionIndex){
		if (pickUpRoot.options[optionIndex].title == "Take") {
			TakeItem ();
		}
	}
}
