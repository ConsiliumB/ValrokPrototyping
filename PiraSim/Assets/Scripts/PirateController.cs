using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateController : MonoBehaviour {
    public float rotationSpeed;
    public float speed;
    public Sprite healthy;
    public Sprite damaged;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = healthy;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CirclePatrol();
	}

    private void CirclePatrol()
    {
        rb2d.MoveRotation(rb2d.rotation + rotationSpeed * Time.fixedDeltaTime);
        rb2d.velocity = transform.up * speed * -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (spriteRenderer.sprite.Equals(healthy))
            {
                spriteRenderer.sprite = damaged;
            } else
            {
                Destroy(gameObject);
            }            
        }
    }
}
