using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawn : MonoBehaviour {
	public static int score = 0;

	public GameObject[] boxList;
    public int[] goodRNG = new int[10] {1,1,1,1,1,1,1,1,1,1};
    public int[,] moreRNG = new int[7, 1];
    private int forceCounter = 0;
    private int y = 0;


    Text instruction;
    // Use this for initialization
    void Start () {
		SpawnNew();
        instruction = GetComponentInChildren<Text>();
    }

	public void SpawnNew() {
		++score;

		int i = Random.Range(0, boxList.Length);

       //if (!check5(i)) {
       //     SpawnNew();
       //     return;
       // }

       // if (forceCounter < 7 || !forceSpawn(i))
        //{
            Quaternion rotation = Quaternion.Euler(-90, 0, 0); //rotates x to make box standing up
            Instantiate(boxList[i], transform.position, rotation);
        //    forceCounter++;
        //    goodRNG[y++] = i;
        //    moreRNG[i, 1]++;
        //}
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    bool check5(int index) {

        int x = 0;
        for (int i = 0; i < goodRNG.Length; i++) {
            if (goodRNG[i] == index)
            {
                x++;
            }
        }

        if(x > 4) { return false; }

        return true;

    }

    bool forceSpawn(int index) {

        for (int i = 0; i < moreRNG.Length; i++) {
            if (moreRNG[i, 1] == 0) {
                Quaternion rotation = Quaternion.Euler(-90, 0, 0); //rotates x to make box standing up
                Instantiate(boxList[i], transform.position, rotation);
                forceCounter = 0;
                clear(moreRNG);
                return true;
            }
        }

        return false;
    }

    void clear(int[,] array) {
        for(int i = 0; i < array.Length; i++)
        {
            array[i,1] = 0;

        }
    }
}
