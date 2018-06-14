using UnityEngine;
using System.Collections;

public class TileBrush : MonoBehaviour {

	public Vector3 brushLocation = Vector3.zero;
	public Vector2 brushSize = Vector2.zero;
	public int tileID = 0;
	public Object[] tileOptions;

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (brushLocation, brushSize);
	}
}
