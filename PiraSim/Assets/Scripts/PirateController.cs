using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateController : MonoBehaviour {
    public float rotationSpeed;
    public float speed;
    public float chaseThreshold;
    public float fightThreshold;
    public GameObject player;
    public GameObject projectile;



    private Rigidbody2D rb2d;
    private Animator animator;

    private int currentHealth;
    private bool isDead;
    private State state;

    private enum State
    {
        Patrol, Chase, Flee, Fight, Dead
    }

	void Start () {
        state = State.Patrol;
        currentHealth = 3;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}

    private void Update()
    {
        if (currentHealth < 1)
        {
            state = State.Dead;
            Destroy(gameObject, 1.0f);
            return;
        }

        var distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < fightThreshold)
        {
            if (state != State.Fight)
            {
                state = State.Fight;
                InvokeRepeating("ShootLeft", 0.1f, 0.5f);
                //InvokeRepeating("ShootRight", 0.1f, 0.5f);
            }
     
        }
        else if (distance < chaseThreshold)
        {
            if (state != State.Chase)
            {
                state = State.Chase;
                CancelInvoke();
            }
        }
    }

    void FixedUpdate () {
        switch (state)
        {
            case State.Patrol:
                CirclePatrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Flee:
                Flee();
                break;
            case State.Fight:
                Fight();
                break;
            case State.Dead:
                rb2d.velocity *= 0;
                break;
            default:
                CirclePatrol();
                break;
        }
	}
    private void CirclePatrol()
    {
        rb2d.MoveRotation(rb2d.rotation + rotationSpeed * Time.fixedDeltaTime);
        rb2d.velocity = transform.up * speed * -1;
    }

    private void Chase()
    {

        //rb2d.MoveRotation(-1 * Vector3.SignedAngle(player.transform.position - transform.position, Vector3.down, Vector3.forward));
        rb2d.MoveRotation(-1 * Vector3.SignedAngle(player.transform.position - transform.position, Vector3.down, Vector3.forward));
        rb2d.velocity = transform.up * speed * -1;
    }

    private void Flee()
    {
        throw new NotImplementedException();
    }

    private void Fight()
    {
        rb2d.MoveRotation(-1 * Vector3.SignedAngle(player.transform.position - transform.position, Vector3.left, Vector3.forward));
        rb2d.velocity *= 0;
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
        if (collision.gameObject.CompareTag("Projectile"))
        {
            currentHealth -= 1;
            animator.SetInteger("health", currentHealth);
        }
    }
}
