using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnvPlacement : MonoBehaviour {

    public GameObject[] foliage;

    //List of all spawn rates should
    public float[] spawnRate;
    //public float otherSpawnRate;

    private void Awake()
    {
        if (foliage.Length != spawnRate.Length)
        {
            throw new System.Exception("Need spawn rate for each prefab");
        }
    }

    // Use this for initialization
    void Start () {
        for (int i =0; i<foliage.Length; i++) { 
            float lottery = Random.Range(0, 10);
            if (lottery < spawnRate[i])
            {
                //Need to add a semi random placement for each folliage
                Instantiate(foliage[i], transform);
            }
        
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
