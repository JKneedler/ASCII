using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour {

	public Vector3 location;
	public string description;
	public enum tileTypes {Wall, Grass, CaveGround, Plant};
	public tileTypes tileType;
	public bool farmAble;
	public TileRenderInfo soilTileInfo;
	public enum application {None, Dialogue, Inventory, Breakdown, Farmland};
	public application app;
	public TileRenderInfo groundTileInfo;
	private GameObject player;

	// Use this for initialization
	void Start () {
		location = transform.position;
		ItemsForPickup it = gameObject.GetComponent<ItemsForPickup> ();
		if (it) {
			if (it.items.Count > 0) {
				app = application.Inventory;
				gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
			}
		}
		while (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate(){
		PlayerEquipped eq = player.GetComponent<PlayerEquipped> ();
		if (app == application.None) {
			if (tileType == tileTypes.Grass && farmAble) {
				if (eq.slots [0].equippedItem.GetComponent<ItemInfo> ().toolType == ItemInfo.toolTypes.Hoe) {
					SillTile ();
				}
			}
		}
		if (app == application.Dialogue) {
			OpenDialoguePopUp ();
		}
		if (app == application.Inventory) {
			ItemsForPickup item = gameObject.GetComponent<ItemsForPickup> ();
			item.OpenTileInventory ();
		}
		if (app == application.Breakdown) {
			BreakDownAble brDown = gameObject.GetComponent<BreakDownAble> ();
			brDown.BreakDown ();
		}
	}

	void OpenDialoguePopUp(){
		
	}

	void SillTile(){
		SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer> ();
		sp.sprite = soilTileInfo.GetMainSprite ();
		sp.color = soilTileInfo.GetFrontColor ();
		transform.GetChild (0).GetComponent<SpriteRenderer> ().color = soilTileInfo.GetBackColor ();
		description = "Looks like the ground is ready for planting.";
		tileType = tileTypes.Plant;
		app = application.Farmland;

	}

	public void SetToNormalTile(){
		gameObject.GetComponent<SpriteRenderer>().color = groundTileInfo.GetFrontColor();
		gameObject.GetComponent<SpriteRenderer> ().sprite = groundTileInfo.GetMainSprite();
		app = TileInfo.application.None;
	}

	public void SetToPickupTile(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		PlayerMovement pMove = player.GetComponent<PlayerMovement> ();
		if (pMove.currentTile.Equals (gameObject)) {
			pMove.currentTileColor = Color.yellow;
		} else {
			gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
		}
		gameObject.GetComponent<TileInfo> ().app = TileInfo.application.Inventory;
	}
}
