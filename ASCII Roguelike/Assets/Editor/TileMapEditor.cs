using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {

	public TileMap map;
	public TileBrush brush;

	Vector3 mouseHitPos;

	bool mouseOnMap{
		get { 
			return mouseHitPos.x > 0 && mouseHitPos.x < map.gridSize.x && mouseHitPos.y < 0 && mouseHitPos.y > -map.gridSize.y;
		}
	}
	public override void OnInspectorGUI(){
		EditorGUILayout.BeginVertical ();
		var oldSize = map.mapSize;
		map.mapSize = EditorGUILayout.Vector2Field ("Map Size", map.mapSize);
		map.tileSize = EditorGUILayout.Vector2Field ("Tile Size", map.tileSize);
		EditorGUILayout.Vector3Field ("Mouse Position:", map.mousePos);
		map.currentTile = EditorGUILayout.ObjectField ("Current Tile :", map.currentTile, typeof(Object), false);
		if (map.mapSize != oldSize) {
			UpdateCalculations ();
		}
			
		map.tilePadding = EditorGUILayout.Vector2Field ("Tile Padding", map.tilePadding);
		EditorGUILayout.LabelField ("Grid Size In Units:", map.gridSize.x + "x" + map.gridSize.y);

		if (GUILayout.Button ("Clear Tiles")) {
			if (EditorUtility.DisplayDialog ("Clear map's tiles?", "Are you sure?", "Clear", "Do not clear")) {
				ClearMap ();
			}
		}
		EditorGUILayout.EndVertical ();
	}

	void OnEnable(){
		map = target as TileMap;
		brush = map.transform.GetChild (0).gameObject.GetComponent<TileBrush> ();
		Tools.current = Tool.View;
		foreach (Transform child in map.transform) {
			if (child.name == "Tiles") {
				map.tiles = child.gameObject;
			}
		}
		if (map.tiles == null) {
			var go = new GameObject ("Tiles");
			go.transform.SetParent (map.transform);
			go.transform.position = Vector3.zero;

			map.tiles = go;
		}

		UpdateCalculations ();
	}

	void OnSceneGUI(){
		if (brush != null) {
			UpdateHitPosition ();
			MoveBrush ();
			if (mouseOnMap) {
				Event current = Event.current;
				if (current.shift) {
					Draw ();
				} else if(current.alt){
					RemoveTile ();
				}
			}
		}
	}

	void UpdateCalculations(){
		var width = map.tileSize.x;
		var height = map.tileSize.y;

		map.tileSize = new Vector2 (width, height);
		map.gridSize = new Vector2 (width * map.mapSize.x, height * map.mapSize.y);
	}
		
	void UpdateHitPosition(){

		var p = new Plane (map.transform.TransformDirection (Vector3.forward), Vector3.zero);
		var ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
		var hit = Vector3.zero;
		var dist = 0f;

		if (p.Raycast (ray, out dist))
			hit = ray.origin + ray.direction.normalized * dist;

		mouseHitPos = map.transform.InverseTransformPoint (hit);
		map.mousePos = mouseHitPos;
	}

	void MoveBrush(){

		var x = Mathf.Floor (mouseHitPos.x / map.tileSize.x) * map.tileSize.x;
		var y = Mathf.Floor (mouseHitPos.y / map.tileSize.y) * map.tileSize.y;
		brush.brushSize = new Vector2 (map.tileSize.x, map.tileSize.y);

		var row = x / map.tileSize.x;
		var column = Mathf.Abs (y / map.tileSize.y) - 1;

		if (!mouseOnMap) {
			return;
		}
		var id = (int)((column * map.mapSize.x) + row);

		brush.tileID = id;
		x += map.transform.position.x + map.tileSize.x / 2;
		y += map.transform.position.y + map.tileSize.y / 2;
		brush.brushLocation = new Vector3 (x, y, map.transform.position.z);
		SceneView.RepaintAll ();
	}

	void Draw(){
		var id = brush.tileID.ToString ();

		var posX = brush.brushLocation.x;
		var posY = brush.brushLocation.y;

		GameObject tile = GameObject.Find (map.name + "/Tiles/tile_" + id);

		if (tile != null) {
			DestroyImmediate (tile);
		}
		tile = (GameObject)Instantiate (map.currentTile, new Vector3 (posX, posY, 0), map.tiles.transform.rotation);
		tile.gameObject.name = "tile_" + id;
		tile.transform.SetParent (map.tiles.transform);
		tile.transform.position = new Vector3 (posX, posY, 0);

	}

	void RemoveTile(){
		var id = brush.tileID.ToString ();
		GameObject tile = GameObject.Find (map.name + "/Tiles/tile_" + id);

		if (tile != null) {
			DestroyImmediate (tile);
		}

	}

	void ClearMap(){
		while (map.tiles.transform.childCount > 0) {
			Transform t = map.tiles.transform.GetChild (0);
			DestroyImmediate (t.gameObject);
		}
	}
}
