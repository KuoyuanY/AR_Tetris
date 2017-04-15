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
		Instantiate(boxList[i], transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
