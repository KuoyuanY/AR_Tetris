using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {
	float lastFall = 0;
	// Use this for initialization
	void Start () {
		if (!isValidGridPos()) {
			Debug.Log("GAME OVER");
			Destroy(gameObject);
		}
	}
	bool isValidGridPos() {//checking for child block's position
		foreach (Transform child in transform) {
			Vector2 v = Grid.roundVec2(child.position);

			// Not inside Border?
			if (!Grid.insideBorder(v))
				return false;

			// Block in grid cell (and not part of same group)?
			if (Grid.grid[(int)v.x, (int)v.y] != null &&
				Grid.grid[(int)v.x, (int)v.y].parent != transform)
				return false;
		}
		return true;
	}
	void updateGrid() {//update block position
		// Remove old children from grid
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.w; ++x)
				if (Grid.grid[x, y] != null)
				if (Grid.grid[x, y].parent == transform)
					Grid.grid[x, y] = null;

		// Add new children to grid
		foreach (Transform child in transform) {
			Vector2 v = Grid.roundVec2(child.position);
			Grid.grid[(int)v.x, (int)v.y] = child;
		}        
	}
	void Update() {//falling blocks
		
		// Move Left
		if (Input.GetKeyDown(KeyCode.LeftArrow) || GestureManager.Instance.IsNavigating) {
			// Modify position
			transform.position += new Vector3(-1, 0, 0);

			// See if valid
			if (isValidGridPos())
				// It's valid. Update grid.
				updateGrid();
			else
				// It's not valid. revert.
				transform.position += new Vector3(1, 0, 0);
		}

		// Move Right
		//else if (Input.GetKeyDown(KeyCode.RightArrow)) {
		//	// Modify position
		//	transform.position += new Vector3(1, 0, 0);

		//	// See if valid
		//	if (isValidGridPos())
		//		// It's valid. Update grid.
		//		updateGrid();
		//	else
		//		// It's not valid. revert.
		//		transform.position += new Vector3(-1, 0, 0);
		//}

		// Rotate //tap
		else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			transform.Rotate(0, -90, 0);

			// See if valid
			if (isValidGridPos())
				// It's valid. Update grid.
				updateGrid();
			else
				// It's not valid. revert.
				transform.Rotate(0, 90, 0);
		}

		// Move Downwards and Fall
		else if (Input.GetKeyDown(KeyCode.DownArrow) ||
			Time.time - lastFall >= 1) {
			// Modify position
			transform.position += new Vector3(0, -1, 0);

			// See if valid
			if (isValidGridPos()) {
				// It's valid. Update grid.
				updateGrid();
			} else {
				// It's not valid. revert.
				transform.position += new Vector3(0, 1, 0);

				// Clear filled horizontal lines
				Grid.deleteFullRows();

				// Spawn next Group
				FindObjectOfType<spawn>().SpawnNew();

				// Disable script
				enabled = false;
			}

			lastFall = Time.time;
		}
	}
}
