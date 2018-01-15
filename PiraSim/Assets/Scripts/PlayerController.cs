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
        if (Input.GetButtonDown("Jump"))
        {
            ShootLeft();
            ShootRight();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShootLeft();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootRight();
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb2d.velocity *= 0;
            return;
        }
        float vmov = Input.GetAxis("Vertical");
        float hmov = Input.GetAxis("Horizontal");
        rb2d.MoveRotation(rb2d.rotation - hmov * speed * 5 * Time.fixedDeltaTime);
        rb2d.velocity = transform.up * vmov * speed / 2;
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
