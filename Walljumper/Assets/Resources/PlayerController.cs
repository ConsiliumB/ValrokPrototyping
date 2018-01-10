using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int speed;
    bool jumping;
    private Rigidbody2D rb2d;
    List<Collider2D> floorsTouched = new List<Collider2D>();
    List<Collider2D> leftWallsTouched = new List<Collider2D>();
    List<Collider2D> rightWallsTouched = new List<Collider2D>();

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && (floorsTouched.Count != 0 || leftWallsTouched.Count != 0 || rightWallsTouched.Count != 0))
        {
            jumping = true;
        }
    }

    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        if (jumping)
        {
            if (leftWallsTouched.Count != 0)
            {
                rb2d.AddForce(Vector2.right * 4.5f, ForceMode2D.Impulse);
            } else if (rightWallsTouched.Count != 0)
            {
                rb2d.AddForce(Vector2.left * 4.5f, ForceMode2D.Impulse);
            }
            rb2d.AddForce(Vector2.up * 4.5f, ForceMode2D.Impulse);
            jumping = false;
        }
        Vector2 movement = new Vector2(horizontalMovement, 0);
        rb2d.AddForce(movement * speed, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.position = new Vector2(-1, -1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundColliders") && !floorsTouched.Contains(collision.collider))
        {
            floorsTouched.Add(collision.collider);
        }
        else if (collision.gameObject.CompareTag("LeftWallColliders") && !leftWallsTouched.Contains(collision.collider))
        {
            leftWallsTouched.Add(collision.collider);
        }
        else if (collision.gameObject.CompareTag("RightWallColliders") && !rightWallsTouched.Contains(collision.collider))
        {
            rightWallsTouched.Add(collision.collider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundColliders"))
        {
            floorsTouched.Remove(collision.collider);
        }
        else if (collision.gameObject.CompareTag("LeftWallColliders"))
        {
            leftWallsTouched.Remove(collision.collider);
        }
        else if (collision.gameObject.CompareTag("RightWallColliders"))
        {
            rightWallsTouched.Remove(collision.collider);
        }
    }
}
