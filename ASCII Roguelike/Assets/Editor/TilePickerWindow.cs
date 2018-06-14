using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilePickerWindow : EditorWindow {

	public Vector2 scrollPosition = Vector2.zero;
	public int selectedOption = 0;

	[MenuItem("Window/Tile Picker")]
	public static void OpenTilePickerWindow(){
		var window = EditorWindow.GetWindow (typeof(TilePickerWindow));
		var title = new GUIContent ();
		title.text = "Tile Picker";
		window.titleContent = title;
	}

	void OnGUI(){
		if (Selection.activeGameObject == null) {
			return;
		}
		var selection = Selection.activeGameObject.GetComponent<TileMap> ();
		var brush = Selection.activeGameObject.transform.GetChild(0).gameObject.GetComponent<TileBrush> ();
		if (selection != null) {
			Texture2D[] textures = new Texture2D[brush.tileOptions.Length];
			for(int i = 0; i < brush.tileOptions.Length; i++){
				GameObject gameObj = (GameObject)brush.tileOptions[i];
				Sprite sprite1 = gameObj.GetComponent<SpriteRenderer> ().sprite;
				Sprite sprite2 = gameObj.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite;
				Color fillColorBack = gameObj.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color;
				Color fillColorFront = gameObj.GetComponent<SpriteRenderer> ().color;
				Texture2D finalTexture = ConfigureTextures (sprite1, sprite2, fillColorBack, fillColorFront);
				finalTexture.filterMode = FilterMode.Point;
				textures [i] = finalTexture;
			}
			var viewPort = new Rect (0, 0, position.width, position.height);
			var contentSize = new Rect (0, 0, 200, textures.Length * 122);
			scrollPosition = GUI.BeginScrollView (viewPort, scrollPosition, contentSize);
			string[] arrows = new string[textures.Length];
			for (int i = 0; i < textures.Length; i++) {
				GUI.DrawTexture (new Rect (50, i * 86, 63, 77), textures [i]);
				arrows[i] = "-->";
			}
			selectedOption = GUI.SelectionGrid (new Rect (0, 0, 40, textures.Length * 83), selectedOption, arrows, 1);
			GUI.EndScrollView ();
			selection.currentTile = brush.tileOptions [selectedOption];
		}
	}

	private Texture2D ConfigureTextures(Sprite sprite, Sprite sprite2, Color backFillColor, Color frontFillColor){
		var finalTexture = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
		var pixels = sprite.texture.GetPixels(  (int)sprite.textureRect.x, 
			(int)sprite.textureRect.y, 
			(int)sprite.textureRect.width, 
			(int)sprite.textureRect.height );
		var tex2 = new Texture2D( (int)sprite2.rect.width, (int)sprite2.rect.height );
		var cols2 = sprite2.texture.GetPixels(  (int)sprite2.textureRect.x, 
			(int)sprite2.textureRect.y, 
			(int)sprite2.textureRect.width, 
			(int)sprite2.textureRect.height );
		for (int i = 0; i < cols2.Length; i++) {
			cols2 [i] = backFillColor;
			if (pixels [i].a != 0) {
				pixels [i] = frontFillColor;
				cols2 [i] = pixels [i];
			}
		}
		finalTexture.SetPixels(cols2);
		finalTexture.Apply ();
		tex2.SetPixels (cols2);
		tex2.Apply ();
		return finalTexture;
	}
}
