using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public enum categories
	{
		Inventory,
		Player,
		Magic,
		Building,
		Crafting,
		Settings,
		Exit,
		PickUpScreen,
		None
	}
	public Node menuNode;
	public Node curNode;
	public List<MenuOption> curOptions;
	private int totalPages;
	private int currentPage;
	public int startPoint;
	public int endPoint;
	private bool menuShowing;
	public int index;
	public GameObject menuObject;
	public GameObject menuOptionPrefab;
	public GameObject optionsContainer;
	public GameObject optionsPrefab;
	public GameObject pageNumber;
	public categories curCat;
	public int level;
	private bool displayArrow;
	public GameObject altMenuObject;


	// Use this for initialization
	void Start () {
		currentPage = 1;
		CreateMenuNode ();
		ChangeCurrentNode (menuNode, 0);
		menuShowing = false;
		level = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("M")) {
			OpenCloseMenuScreen ();
			if (menuShowing) {
				
			} else {
				ChangeCurrentNode (menuNode, 0);
			}
		}
		if (menuShowing) {
			if (Input.GetButtonDown ("Down")) {
				MoveDown ();
			}
			if (Input.GetButtonDown ("Up")) {
				MoveUp ();
			}
			if (Input.GetButtonDown ("Right")) {
				MoveIn ();
			}
			if (Input.GetButtonDown ("Left")) {
				MoveOut ();
			}
			if (curNode.children.Count > 0 && displayArrow) {
				menuObject.transform.GetChild (index).GetChild (1).gameObject.GetComponent<Text> ().text = ">";
			}
			//Accounts for extra frame where the childCount hasnt caught up yet
			displayArrow = true;
			for (int i = 0; i < curOptions.Count; i++) {
				if (Input.GetButtonDown (curOptions [i].key.ToString())) {
					switch (curCat) {
					case categories.Inventory:
						PlayerInventory inv = gameObject.GetComponent<PlayerInventory> ();
						inv.PressedButton (curOptions[i].title);
						break;
					case categories.Player:
						PlayerEquipped eq = gameObject.GetComponent<PlayerEquipped> ();
						eq.PressedButton (i);
						break;
					case categories.Magic:
						break;
					case categories.Building:
						break;
					case categories.Crafting:
						break;
					case categories.Settings:
						break;
					case categories.PickUpScreen:
						ItemsForPickup it = altMenuObject.GetComponent<ItemsForPickup> ();
						it.PressedButton (i);
						break;
					}
				}
			}
		}
	}

	public void OpenCloseMenuScreen(){
		PlayerInventory inv = gameObject.GetComponent<PlayerInventory> ();
		menuShowing = !menuShowing;
		Camera.main.GetComponent<GameController> ().paused = menuShowing;
		menuObject.SetActive (menuShowing);
		if (menuShowing) {
			if (totalPages > 1) {
				pageNumber.SetActive (menuShowing);
			}
		} else {
			inv.infoTab.SetActive (false);
		}
		level = 0;
	}

	void CreateMenuNode(){
		menuNode = new Node ("main");
		for (int i = 0; i < System.Enum.GetValues(typeof(categories)).Length-2; i++) {
			menuNode.AddChild (new Node (((categories)i).ToString()));
		}
	}

	public void RefreshMenuView(){
		//Determines pages/starting point/ending point
		int menuOptionsCount = curNode.children.Count;
		totalPages = (int)Mathf.Ceil ((float)menuOptionsCount / 38f);
		if (totalPages > 1) {
			pageNumber.GetComponent<Text> ().text = "[ " + currentPage + " / " + totalPages + " ]";
			pageNumber.SetActive (true);
		} else {
			pageNumber.SetActive (false);
		}
		startPoint = (currentPage - 1) * 38;
		endPoint = (currentPage) * 38;
		if (currentPage == totalPages) {
			endPoint = menuOptionsCount;
		}
		//Creates menu objects
		for (int i = startPoint; i < endPoint; i++) {
			GameObject go = (GameObject)Instantiate (menuOptionPrefab, menuObject.transform);
			go.transform.GetChild(0).gameObject.GetComponent<Text> ().text = curNode.children[i].title;
			go.transform.GetChild(1).gameObject.GetComponent<Text> ().text = "";
		}
		//Accounts for the extra frame where the childCount hasn't caught up yet
		displayArrow = false;
		//Creates Options on Top
		ChangeCurrentOptions(curOptions);
	}

	public void ChangeCurrentOptions(List<MenuOption> options){
		DestroyOldOptions ();
		curOptions = options;
		for(int i = 0; i < options.Count; i++){
			GameObject go = (GameObject)Instantiate (optionsPrefab, optionsContainer.transform);
			MenuOption curOpt = options [i];
			go.GetComponent<Text> ().text = curOpt.key + ") " + curOpt.title;
		}
	}

	public void ChangeCurrentNode(Node newNode, int newIndex){
		curNode = newNode;
		curOptions = newNode.options;
		index = newIndex;
		DestroyOldTiles ();
		DestroyOldOptions ();
		RefreshMenuView ();
	}

	public void MoveIn(){
		if (level == 0 && curNode.title == menuNode.title) {
			curCat = (categories)index;
		}
		switch (curCat) {
		case categories.Inventory:
			PlayerInventory inv = gameObject.GetComponent<PlayerInventory> ();
			inv.MoveInOutInventory (true, level, index);
			break;
		case categories.Player:
			PlayerEquipped eq = gameObject.GetComponent<PlayerEquipped> ();
			eq.MoveInOutEquipped (true, level);
			break;
		}
	}

	public void MoveOut(){
		if (level > 0) {
			switch (curCat) {
			case categories.Inventory:
				PlayerInventory inv = gameObject.GetComponent<PlayerInventory> ();
				inv.MoveInOutInventory (false, level, index);
				break;
			case categories.Player:
				PlayerEquipped eq = gameObject.GetComponent<PlayerEquipped> ();
				eq.MoveInOutEquipped (false, level);
				break;
			}
		}
	}

	public void MoveDown(){
		if (index == (endPoint-((currentPage-1)*38))-1) {
			if (currentPage == totalPages) {
				//Do Nothing
			} else {
				currentPage++;
				index = 0;
				DestroyOldTiles ();
				DestroyOldOptions ();
				RefreshMenuView ();
			}
		} else {
			index++;
			menuObject.transform.GetChild(index-1).GetChild(1).gameObject.GetComponent<Text>().text = "";
		}
		switch (curCat) {
		case categories.Inventory:
			PlayerInventory inv = gameObject.GetComponent<PlayerInventory> ();
			if (level == 2) {
				inv.UpdateInfo (index);
			}
			break;
		}
		if (!curOptions.Equals (curNode.options)) {
			ChangeCurrentOptions (curNode.options);
		}
	}

	public void MoveUp(){
		if (index == (startPoint/38)-(currentPage-1)) {
			if (currentPage == 1) {
				//Do Nothing
			} else {
				currentPage--;
				DestroyOldOptions ();
				DestroyOldTiles ();
				RefreshMenuView ();
				index = (currentPage*38)-currentPage;
			}
		} else {
			index--;
			menuObject.transform.GetChild(index+1).GetChild(1).gameObject.GetComponent<Text>().text = "";
		}
		switch (curCat) {
		case categories.Inventory:
			PlayerInventory inv = gameObject.GetComponent<PlayerInventory> ();
			if (level == 2) {
				inv.UpdateInfo (index);
			}
			break;
		}
		if (!curOptions.Equals (curNode.options)) {
			ChangeCurrentOptions (curNode.options);
		}
	}

	public void AddToCurrentNode(){
		RefreshMenuView ();
	}

	public void RemoveFromCurrentNode(){
		curNode.children.RemoveAt (index);
		if (index == endPoint - 1) {
			if (index == 0) {
				if (curCat == categories.PickUpScreen) {
					ChangeCurrentNode (menuNode, 0);
					OpenCloseMenuScreen ();
				} else if (curCat == categories.Inventory) {
					curNode.AddChild (new Node ("No Items found here."));
					DestroyOldTiles ();
					DestroyOldOptions ();
					RefreshMenuView ();
				}
			} else {
				index--;
				DestroyOldTiles ();
				DestroyOldOptions ();
				RefreshMenuView ();
			}
		} else {
			DestroyOldTiles ();
			DestroyOldOptions ();
			RefreshMenuView ();
		}

	}

	public void DestroyOldTiles(){
		//Destroy Old Menu tiles
		int chCount = menuObject.transform.childCount;
		for(int i = 0; i < chCount; i++){
			Destroy (menuObject.transform.GetChild(i).gameObject);
		}
	}

	public void DestroyOldOptions(){
		//Destroy Old Options
		int chCountOptions = optionsContainer.transform.childCount;
		for (int i = 0; i < chCountOptions; i++) {
			Destroy (optionsContainer.transform.GetChild (i).gameObject);
		}
	}
}
