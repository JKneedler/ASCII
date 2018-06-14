using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventBox : MonoBehaviour {

	public GameObject[] textBoxes = new GameObject[5];
	public string[] text = new string[5];
	public float[] textTimers = new float[5];
	public float messageTime;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < textBoxes.Length; i++) {
			textBoxes [i].GetComponent<Text> ().text = text [i];
			if (textTimers [i] <= 0) {
				text [i] = "";
				textTimers [i] = 0;
			}
			if (text [i] != "" && textTimers[i] > 0) {
				textTimers [i] -= Time.deltaTime;
			}
		}
	}

	public void AddMessage(string message){
		for (int i = textBoxes.Length - 1; i > 0; i--) {
			text [i] = text [i-1];
			textTimers [i] = textTimers [i-1];
			if (i == 1) {
				text [i-1] = message;
				textTimers [i-1] = messageTime;
			}
		}
	}
}
