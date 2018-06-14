using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckShitUp : MonoBehaviour {

	public GameObject addItem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("P")) {
			gameObject.GetComponent<PlayerInventory> ().AddItem (addItem);
			Debug.Log ("Added");
		}
	}
}
