using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public string title;
	public List<Node> children;
	public List<MenuOption> options;
	public GameObject item;

	public Node(string title, GameObject item){
		this.title = title;
		this.item = item;
		this.children = new List<Node> ();
		this.options = new List<MenuOption> ();
	}

	public Node(string title){
		this.title = title;
		this.children = new List<Node> ();
		this.options = new List<MenuOption> ();
		item = null;
	}

	public void AddChild(Node child){
		children.Add (child);
	}
}
