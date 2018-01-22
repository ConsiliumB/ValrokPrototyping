using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move_normal : MonoBehaviour {

    public float speed;
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    private SpriteRenderer srend;

	// Use this for initialization
	void Start () {
        srend = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");

        var pos = transform.position;
        pos.x += inputX * speed * Time.deltaTime;
        pos.y += inputY * speed * Time.deltaTime;
        transform.position = pos;

        if (inputX > 0)
        {
            srend.sprite = left;
        }
        else if (inputX < 0)
        {
            srend.sprite = right;
        }
        else if (inputY > 0)
        {
            srend.sprite = up;
        }
        else if (inputY < 0)
        {
            srend.sprite = down;
        }
    }
}
