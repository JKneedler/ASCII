using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileRenderInfo {

	public Sprite mainSprite;
	public Color32 frontColor;
	public Color32 backColor;

	public Sprite GetMainSprite(){
		return mainSprite;
	}

	public Color32 GetFrontColor(){
		return frontColor;
	}

	public Color32 GetBackColor(){
		return backColor;
	}
}
