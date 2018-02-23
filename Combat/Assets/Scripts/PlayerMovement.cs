using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D rigidbody;
	private Animator animator;

    float prevDirX;
    float prevDirY;
    
    [Range(0.1f,10f)]
    public float moveSpeed;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        Vector3 y = Vector3.up * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 x = Vector3.right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");

        Vector3 heading = (x + y).normalized;

        transform.position += x;
        transform.position += y;
        if (x.x > 0)
        {
            transform.localScale = Vector3.forward + Vector3.up + Vector3.right;
        }
        else if (x.x < 0)
        {
            transform.localScale = Vector3.forward + Vector3.up + Vector3.left;
        }
        UpdateAnimation(heading);
    }

    private void UpdateAnimation(Vector3 heading)
    {
        if (heading.x == 0f && heading.y == 0f)
        {
            animator.SetFloat("LastDirX", prevDirX);
            animator.SetFloat("LastDirY", prevDirY);

            animator.SetBool("Moving", false);
        }
        else
        {
            prevDirX = heading.x;
            prevDirY = heading.y;

            animator.SetBool("Moving", true);
        }

        animator.SetFloat("DirX", heading.x);
        animator.SetFloat("DirY", heading.y);
    }
}
