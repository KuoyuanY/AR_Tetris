using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	
	public static int w = 10;
	public static int h = 20;
	public static Transform[,] grid = new Transform[w, h];

	// Use this for initialization
	void Start () {
		
	}
	public static Vector2 roundVec2(Vector2 v) {//returns a new vector with whole number position
		return new Vector2(Mathf.Round(v.x),
			Mathf.Round(v.y)); 
	}
	public static bool insideBorder(Vector2 pos) {//checks if a block is within border
		return ((int)pos.x >= 0 &&
			(int)pos.x < w &&
			(int)pos.y >= 0);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
