using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int speed;
    public GameObject projectile;

    public int currentHealth;
    private Rigidbody2D rb2d;
    private Animator animator;
    private bool isDead;
    private float vmov;
    private float hmov;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHealth < 1)
        {
            isDead = true;
            return;
        }

        vmov = Input.GetAxis("Vertical");
        hmov = Input.GetAxis("Horizontal");
        if (hmov > 0)
        {
            animator.SetTrigger("right");
        }
        else if (hmov < 0)
        {
            animator.SetTrigger("left");
        }
        else if (vmov > 0)
        {
            animator.SetTrigger("up");
        }
        else
        {
            animator.SetTrigger("down");
        }

        if (Input.GetButtonDown("Jump"))
        {
            ShootLeft();
            ShootRight();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastSpell("Q");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastSpell("E");
        }
    }

    private void CastSpell(string v)
    {
        switch(v) {
            case "E":
                break;
            case "Q":
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb2d.velocity *= 0;
            return;
        }

        rb2d.velocity = new Vector2(hmov,vmov) * speed;
        // transform.Translate(new Vector2(hmov,vmov) * speed);
    }

    private void ShootLeft()
    {
        var bullet = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * -1 * 10;

        Destroy(bullet, 2.0f);
    }

    private void ShootRight()
    {
        var bullet = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * 10;

        Destroy(bullet, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            currentHealth -= 1;
            animator.SetInteger("health", currentHealth);
        }
    }
}
