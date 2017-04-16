using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class Group : MonoBehaviour {
	float lastFall = 0;
	// Use this for initialization

	// Tap and Navigation gesture recognizer.
	public GestureRecognizer NavigationRecognizer { get; private set; }

	// Manipulation gesture recognizer.
	public GestureRecognizer ManipulationRecognizer { get; private set; }

	// Currently active gesture recognizer.
	public GestureRecognizer ActiveRecognizer { get; private set; }

	public bool IsNavigating { get; private set; }

	public Vector3 NavigationPosition { get; private set; }


	void Awake()
	{
		/* TODO: DEVELOPER CODING EXERCISE 2.b */

		// 2.b: Instantiate the NavigationRecognizer.
		NavigationRecognizer = new GestureRecognizer();

		// 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
		NavigationRecognizer.SetRecognizableGestures(
			GestureSettings.Tap |
			GestureSettings.NavigationX |
            GestureSettings.NavigationY);

		// 2.b: Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.
		NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
		// 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
		NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
		// 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
		NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
		// 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 
		NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
		// 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 
		NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

		ResetGestureRecognizers();
	}

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
		if (Input.GetKeyDown(KeyCode.LeftArrow) || IsNavigating) {
            // Modify position
            if (NavigationPosition.x < 0 || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0);

                // See if valid
                if (isValidGridPos())
                    // It's valid. Update grid.
                    updateGrid();
                else
                    // It's not valid. revert.
                    transform.position += new Vector3(1, 0, 0);
            }
            else if (NavigationPosition.x > 0)
            {
                // Modify position
                transform.position += new Vector3(1, 0, 0);

                // See if valid
                if (isValidGridPos())
                    // It's valid. Update grid.
                    updateGrid();
                else
                    // It's not valid. revert.
                    transform.position += new Vector3(-1, 0, 0);
            }
           else if (NavigationPosition.y > 0) {
                transform.Rotate(0, -90, 0);

                // See if valid
                if (isValidGridPos())
                    // It's valid. Update grid.
                    updateGrid();
                else
                    // It's not valid. revert.
                    transform.Rotate(0, 90, 0);
            }
            else if (NavigationPosition.y < 0 || Time.time - lastFall >= 1) {

                // Modify position
                transform.position += new Vector3(0, -1, 0);

                // See if valid
                if (isValidGridPos())
                {
                    // It's valid. Update grid.
                    updateGrid();
                }
                else
                {
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

        //Move Right
		else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(-1, 0, 0);
        }

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


	void OnDestroy()
	{
		// 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.
		NavigationRecognizer.TappedEvent -= NavigationRecognizer_TappedEvent;

		NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
		NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
		NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
		NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;

	}


	public void ResetGestureRecognizers()
	{
		// Default to the navigation gestures.
		Transition(NavigationRecognizer);
	}

	/// <summary>
	/// Transition to a new GestureRecognizer.
	/// </summary>
	/// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
	public void Transition(GestureRecognizer newRecognizer)
	{
		if (newRecognizer == null)
		{
			return;
		}

		if (ActiveRecognizer != null)
		{
			if (ActiveRecognizer == newRecognizer)
			{
				return;
			}

			ActiveRecognizer.CancelGestures();
			ActiveRecognizer.StopCapturingGestures();
		}

		newRecognizer.StartCapturingGestures();
		ActiveRecognizer = newRecognizer;
	}

	private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		// 2.b: Set IsNavigating to be true.
		IsNavigating = true;

		// 2.b: Set NavigationPosition to be relativePosition.
		NavigationPosition = relativePosition;
	}

	private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		// 2.b: Set IsNavigating to be true.
		IsNavigating = true;

		// 2.b: Set NavigationPosition to be relativePosition.
		NavigationPosition = relativePosition;
	}

	private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		// 2.b: Set IsNavigating to be false.
		IsNavigating = false;
	}

	private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		// 2.b: Set IsNavigating to be false.
		IsNavigating = false;
	}

	private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
	{
		GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

		if (focusedObject != null)
		{
			focusedObject.SendMessageUpwards("OnSelect");
		}
	}











}
