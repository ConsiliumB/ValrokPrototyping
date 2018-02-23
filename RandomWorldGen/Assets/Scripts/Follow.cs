using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    [Header("Make smooth camera")]
    [Range(0,1)]
    public float smooth;

    private Transform newpos;
    private bool start = true;

	// Use this for initialization
	void Start () {
        
	}
	
    //assumes the gameobject "snake" has a single schild
    void Init()
    {
        newpos = GameObject.Find("Snake").transform.GetChild(0);
        if (newpos == null)
        {
            throw new MissingComponentException();
        }
    }

	// Update is called once per frame
	void Update () {
        if (start)
        {
            start = false;
            Init();
        }

        var lerpx = Mathf.Lerp(transform.position.x, newpos.position.x, smooth);
        var lerpy = Mathf.Lerp(transform.position.y, newpos.position.y, smooth);

        transform.position = new Vector3(lerpx, lerpy, transform.position.z);
    }
}
