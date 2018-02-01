using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveChar : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var inx = Input.GetAxis("Horizontal");
        var iny = Input.GetAxis("Vertical");

        var pos = transform.position;
        pos.x += inx * speed * Time.deltaTime;
        pos.y += iny * speed * Time.deltaTime;
        transform.position = pos;
	}
}
