using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeScore : MonoBehaviour
{
	 Text instruction;
	// Use this for initialization
	void Start ()
	{
		instruction = GetComponent<Text>();
        //instruction = GetComponentInChildren<Text>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		instruction.text = "Score: "+ spawn.score;
	}
}

