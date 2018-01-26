using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateController : MonoBehaviour {
    [Range(0,1)]
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
    private Vector3 heading;

    private float debugTimer;

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
        heading = player.transform.position - transform.position;
        if (heading.sqrMagnitude < fightThreshold * fightThreshold)
        {
            if (state != State.Fight)
            {
                state = State.Fight;
                InvokeRepeating("ShootLeft", 0.3f, 0.5f);
                //InvokeRepeating("ShootRight", 0.1f, 0.5f);
            }
     
        }
        else if (heading.sqrMagnitude < chaseThreshold * chaseThreshold)
        {
            if (state != State.Chase)
            {
                state = State.Chase;
                CancelInvoke();
            }
        }

        if (Time.time > debugTimer + 0.2f)
        {
            debugTimer = Time.time;
            Debug.Log(Vector2.SignedAngle(transform.position, player.transform.position));
            Debug.Log(rb2d.rotation);
        }
    }

    void FixedUpdate () {
        switch (state)
        {
            case State.Patrol:
                CirclePatrol();
                break;
            case State.Chase:
                Fight();
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
        transform.Rotate(Vector3.forward * speed);
        rb2d.velocity = transform.up * speed;
    }

    private void Chase()
    {
        //if (rb2d.rotation > -1 * Vector3.SignedAngle(player.transform.position - transform.position, Vector3.down, Vector3.forward))
        //{

        //}
        //else if (rb2d.rotation < -1 * Vector3.SignedAngle(player.transform.position - transform.position, Vector3.down, Vector3.forward))
        //{

        //}
        //else
        //{

        //}
        //rb2d.MoveRotation(Vector3.SignedAngle(player.transform.position - transform.position, Vector3.down, Vector3.forward));
        RotateTowardsPlayer();
        rb2d.velocity = transform.up * speed;
    }

    private void RotateTowardsPlayer()
    {
        rb2d.MoveRotation(Vector3.SignedAngle(player.transform.position - transform.position, Vector3.up, Vector3.back));
        //var angle = Vector2.SignedAngle(transform.position, player.transform.position);
        //angle = rb2d.rotation - angle;
        //if (angle < -5.0f)
        //{
        //    rb2d.MoveRotation(rb2d.rotation + 5.0f);
        //}
        //else if (angle > 5.0f)
        //{
        //    rb2d.MoveRotation(rb2d.rotation - 5.0f);
        //}
        //else
        //{
        //    rb2d.MoveRotation(rb2d.rotation + angle);
        //}
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed);
    }

    private void Flee()
    {
        throw new NotImplementedException();
    }

    private void Fight()
    {
        RotateTowardsPlayer();
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
