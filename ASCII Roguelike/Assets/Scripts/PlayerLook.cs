using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLook : MonoBehaviour {

	public Vector3 lookPosition;
	public GameObject highlightedTile;
	public Object highlightObject;
	public GameObject highlight;
	public GameObject lookText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!Camera.main.GetComponent<GameController> ().paused) {
			GetPosition ();
			if (highlightedTile != null && highlight != null) {
				highlight.transform.position = highlightedTile.transform.position;
			}
			if (highlightedTile == null && highlight != null) {
				Destroy (highlight);
			} else if (highlightedTile == null) {
			
			}
			if (highlightedTile != null && highlight == null) {
				highlight = (GameObject)Instantiate (highlightObject, highlightedTile.transform.position, Quaternion.identity);
			}
			if (highlightedTile != null) {
				lookText.GetComponent<Text> ().text = highlightedTile.GetComponent<TileInfo> ().description;
				if (Input.GetMouseButtonDown (0)) {
					bool contains = false;
					for (int i = 0; i < gameObject.GetComponent<PlayerMovement> ().surroundTiles.Length; i++) {
						if (gameObject.GetComponent<PlayerMovement> ().surroundTiles [i] == highlightedTile)
							contains = true;
					}
					if (contains == true) {
						highlightedTile.GetComponent<TileInfo> ().Activate ();
					} else {
						gameObject.GetComponent<EventBox> ().AddMessage ("There's no way you're gonna reach that from there.");
					}
				}
			} else {
				lookText.GetComponent<Text> ().text = "";
			}
		}
	}

	void GetPosition(){
		lookPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lookPosition.z = 0;
		RaycastHit2D tileHit = new RaycastHit2D();
		//Ray ray = new Ray (Camera.main.transform.position, lookPosition);
		Vector2 origin = new Vector2 (lookPosition.x, lookPosition.y);
		tileHit = Physics2D.Raycast (origin, new Vector2(0, 1), .5f, 1);
		if (tileHit) {
			if (tileHit.collider.GetComponent<TileInfo> ()) {
				highlightedTile = tileHit.collider.gameObject;
			}
		} else {
			highlightedTile = null;
		}
	}

	void Overlay(){
		if (highlight == null) {
			
		}
	}
}
