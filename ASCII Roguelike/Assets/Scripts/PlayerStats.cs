using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public int maxHealth;
	public int curHealth;
	public RectTransform healthBar;
	private float healthBarWidth;
	public int maxMana;
	public int curMana;
	public RectTransform manaBar;
	private float manaBarWidth;
	public int maxStamina;
	public int curStamina;
	public RectTransform staminaBar;
	private float staminaBarWidth;

	// Use this for initialization
	void Start () {
		healthBarWidth = healthBar.sizeDelta.x;
		manaBarWidth = manaBar.sizeDelta.x;
		staminaBarWidth = staminaBar.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update () {
		//Health
		if(curHealth > maxHealth) curHealth = maxHealth;
		if (curHealth <= 0) Die ();
		if (maxHealth <= 0) maxHealth = 1;
		if (curHealth > 0) {
			float barWidth = ((float)curHealth / (float)maxHealth);
			healthBar.sizeDelta = new Vector2((healthBarWidth*barWidth), healthBar.sizeDelta.y);
		}
		//Mana
		if(curMana > maxMana) curMana = maxMana;
		if (curMana <= 0) Die ();
		if (maxMana <= 0) maxMana = 1;
		if (curMana > 0) {
			float barWidth = ((float)curMana / (float)maxMana);
			manaBar.sizeDelta = new Vector2((manaBarWidth*barWidth), manaBar.sizeDelta.y);
		}
		//Stamina
		if(curStamina > maxStamina) curStamina = maxStamina;
		if (curStamina <= 0) Die ();
		if (maxStamina <= 0) maxStamina = 1;
		if (curStamina > 0) {
			float barWidth = ((float)curStamina / (float)maxStamina);
			staminaBar.sizeDelta = new Vector2((staminaBarWidth*barWidth), staminaBar.sizeDelta.y);
		}
	}

	public void AddStatsToEquippedNode(Node node){
		node.children.Add (new Node (""));
		node.children.Add (new Node ("Health : " + curHealth + " / " + maxHealth));
		node.children.Add (new Node ("Stamina : " + curStamina + " / " + maxStamina));
		node.children.Add (new Node ("Mana : " + curMana + " / " + maxMana));
	}

	public void UpdateStatNodes(Node equippedNode){
		PlayerEquipped eq = gameObject.GetComponent<PlayerEquipped> ();
		int statsIndexStart = eq.slots.Length + 1;
		equippedNode.children [statsIndexStart].title = "Health : " + curHealth + " / " + maxHealth;
		equippedNode.children [statsIndexStart+1].title = "Stamina : " + curStamina + " / " + maxStamina;
		equippedNode.children [statsIndexStart+2].title = "Mana : " + curMana + " / " + maxMana;

	}

	private void Die(){
		
	}
}
