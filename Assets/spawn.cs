using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {
	
	public GameObject[] boxList;

	// Use this for initialization
	void Start () {
		SpawnNewBox();
	}

	public void SpawnNewBox() {
		int i = Random.Range(0, boxList.Length);
		Quaternion rotation = Quaternion.Euler(-90, 0, 0); //rotates x to make box standing up
		Instantiate(boxList[i], transform.position, rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
