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
			(int)pos.y >= 4);
	}
	public static void deleteRow(int y) {//deletes the row that's full
		for (int x = 0; x < w; ++x) {
			Destroy(grid[x, y].gameObject);
			grid[x, y] = null;
		}
	}
	public static void decreaseRow(int y) {//move parameter row down by 1
		for (int x = 0; x < w; ++x) {
			if (grid[x, y] != null) {
				// Move one towards bottom
				grid[x, y-1] = grid[x, y];
				grid[x, y] = null;

				// Update Block position in world
				grid[x, y-1].position += new Vector3(0, -1, 0);
			}
		}
	}
	public static void decreaseRowsAbove(int y) {//move all rows above down by 1 
		for (int i = y; i < h; ++i)
			decreaseRow(i);
	}
	public static bool isRowFull(int y) {//checks if parameter row is full
		for (int x = 0; x < 10; ++x)
			if (grid[x, y] == null)
				return false;
		return true;
	}
	public static void deleteFullRows() {//deletes rows that are full
		for (int y = 0; y < h; ++y) {
			if (isRowFull(y)) {
				deleteRow(y);
				decreaseRowsAbove(y+1);
				--y;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
