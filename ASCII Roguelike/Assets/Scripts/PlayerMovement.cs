using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	/* 0 = Top Tile
	 * 1 = Left Tile
	 * 2 = Bottom Tile
	 * 3 = Right Tile
	 */
	public GameObject[] surroundTiles = new GameObject[4];
	public GameObject currentTile;
	public Sprite currentTileIcon;
	public Color currentTileColor;
	public Sprite atIcon;
	public Color atColor;

	// Use this for initialization
	void Start () {
		currentTile = GameObject.FindGameObjectWithTag ("Start");
		currentTile.GetComponent<TileInfo> ().location = currentTile.transform.position;
		transform.position = currentTile.GetComponent<TileInfo> ().location;
		currentTile.GetComponent<Collider2D> ().enabled = false;
		GetSurroundingTiles ();
		UpdateTile ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Camera.main.GetComponent<GameController> ().paused) {
			if (Input.GetButtonDown ("Left")) {
				Move (1);
			}
			if (Input.GetButtonDown ("Up")) {
				Move (0);
			}
			if (Input.GetButtonDown ("Down")) {
				Move (2);
			}
			if (Input.GetButtonDown ("Right")) {
				Move (3);
			}
		}
	}

	void GetSurroundingTiles () {
		RaycastHit2D[] tileHits = new RaycastHit2D[4];
		tileHits [0] = Physics2D.Raycast (transform.position, new Vector2 (0, 1), 11, 1);
		tileHits [1] = Physics2D.Raycast (transform.position, new Vector2 (-1, 0), 11, 1);
		tileHits [2] = Physics2D.Raycast (transform.position, new Vector2 (0, -1), 11, 1);
		tileHits [3] = Physics2D.Raycast (transform.position, new Vector2 (1, 0), 11, 1);
		for (int i = 0; i < tileHits.Length; i++) {
			if (tileHits [i].collider != null) {
				surroundTiles [i] = tileHits [i].collider.gameObject;
			}
		}
	}

	void ClearTiles(){
		for (int i = 0; i < surroundTiles.Length; i++) {
			surroundTiles [i] = null;
		}
	}

	void Move(int direction){
		if (surroundTiles [direction].tag != "Wall") {
			currentTile.GetComponent<SpriteRenderer> ().sprite = currentTileIcon;
			currentTile.GetComponent<SpriteRenderer> ().color = currentTileColor;
			currentTile.GetComponent<Collider2D> ().enabled = true;
			currentTile = surroundTiles [direction];
			UpdateTile ();
			currentTile.GetComponent<Collider2D> ().enabled = false;
			transform.position = currentTile.GetComponent<TileInfo> ().location;
			ClearTiles ();
			GetSurroundingTiles ();
		}
	}

	void UpdateTile(){
		currentTileIcon = currentTile.GetComponent<SpriteRenderer> ().sprite;
		currentTileColor = currentTile.GetComponent<SpriteRenderer> ().color;
		currentTile.GetComponent<SpriteRenderer> ().sprite = atIcon;
		currentTile.GetComponent<SpriteRenderer> ().color = atColor;
	}
}
