using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move_iso : MonoBehaviour {

    public float speed;
    private float speedY;

	// Use this for initialization
	void Start () {
        speedY = speed / 1.7f;
	}
	
	// Update is called once per frame
	void Update () {
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");

        Vector3 pos = transform.position;
        //RIGHT && UP
        if (inputX > 0 && inputY > 0)
        {
            pos.x += speed * Time.deltaTime;
        }
        //RIGHT DOWN
        else if (inputX > 0 && inputY < 0)
        {
            pos.y -= speed * Time.deltaTime; 
        }
        //LEFT UP
        else if (inputX < 0 && inputY > 0)
        {
            pos.y += speed * Time.deltaTime;
        }
        //LEFT DOWN
        else if (inputX < 0 && inputY < 0)
        {
            pos.x -= speed * Time.deltaTime;
        }
        else if (inputX > 0)
        {
            pos.x += speed * Time.deltaTime;
            pos.y -= speedY * Time.deltaTime;
        }
        else if (inputY > 0)
        {
            pos.x += speed * Time.deltaTime;
            pos.y += speedY * Time.deltaTime;
        }
        else if (inputX < 0)
        {
            pos.x -= speed * Time.deltaTime;
            pos.y += speedY * Time.deltaTime;
        }
        else if (inputY < 0)
        {
            pos.x -= speed * Time.deltaTime;
            pos.y -= speedY * Time.deltaTime;
        }

        transform.position = pos;
	}
}
