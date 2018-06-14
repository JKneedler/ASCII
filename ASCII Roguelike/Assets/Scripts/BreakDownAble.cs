using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDownAble : MonoBehaviour {

	public List<Drops> dropPossibilites;
	public List<GameObject> drops;
	public enum tools {sword, axe, shovel, pickaxe, hoe, any};
	public tools reqTool;
	public tools prefTool;
	public Sprite groundSprite;

	// Use this for initialization
	void Start () {
		GenerateDrops ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateDrops(){
		foreach (Drops drop in dropPossibilites) {
			int num = Random.Range (drop.min, drop.max);
			for (int i = 0; i < num; i++) {
				drops.Add (drop.drop);
			}
		}
	}

	public void BreakDown(){
		bool canBreakDown = true;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		PlayerEquipped eq = player.GetComponent<PlayerEquipped> ();
		if (reqTool != tools.any) {
			if (eq.slots [0].equippedItem != null) {
				ItemInfo it = eq.slots [0].equippedItem.GetComponent<ItemInfo> ();
				if ((int)it.toolType != (int)reqTool) {
					canBreakDown = false;
				}
			} else {
				canBreakDown = false;
			}
		}
		if (canBreakDown) {
			gameObject.AddComponent (typeof(ItemsForPickup));
			ItemsForPickup it = gameObject.GetComponent<ItemsForPickup> ();
			it.CreatePickUpNode ();
			foreach (GameObject item in drops) {
				it.AddItemToTile (item);
			}
			SpriteRenderer thisSprite = gameObject.GetComponent<SpriteRenderer> ();
			thisSprite.sprite = groundSprite;
			gameObject.tag = "Untagged";
		} else {
		}
	}
}
