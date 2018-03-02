using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StatefulEntity {
    public int speed;
    public GameObject projectile;

    public int currentHealth;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private bool isDead;
    private float vmov;
    private float hmov;

    private float prevDirX;
    private float prevDirY;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Pathfinding.Player = this;
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

        if (rigidbody.velocity.x > 0)
        {
            transform.localScale = Vector3.forward + Vector3.up + Vector3.right;
        }
        else if (rigidbody.velocity.x < 0)
        {
            transform.localScale = Vector3.forward + Vector3.up + Vector3.left;
        }
        UpdateAnimation(rigidbody.velocity.normalized);
        

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

    private void UpdateAnimation(Vector2 heading)
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
            rigidbody.velocity *= 0;
            rigidbody.rotation = 180;
            return;
        }

        rigidbody.velocity = new Vector2(hmov,vmov) * speed;
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
        }
    }
}
