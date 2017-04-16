using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {
	public static int score;
	public GameObject[] boxList;

	// Use this for initialization
	void Start () {
		SpawnNew();
	}

	public void SpawnNew() {
		++score;
		int i = Random.Range(0, boxList.Length);
		Quaternion rotation = Quaternion.Euler(-90, 0, 0); //rotates x to make box standing up
		var go = Instantiate(boxList[i], transform.position, rotation) as GameObject;
		go.AddComponent<SphereSounds> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
