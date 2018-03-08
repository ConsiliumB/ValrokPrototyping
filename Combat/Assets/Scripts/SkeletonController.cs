using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	public GameObject player;
	private Animator animator;
	//private Rigidbody2D rigidbody;
	private State state;
	private int currentHealth;

	private enum State
	{
		Chase, Attack, Patrol, Flee
	}

	// Use this for initialization
	void Start () {
		currentHealth = 2;
		animator = GetComponent<Animator>();
		//rigidbody2D = GetComponent<Rigidbody2D>();
		state = State.Patrol;
	}
	
	// Update is called once per frame
	void Update () {
		switch (state)
		{
			case State.Patrol:
				break;
			case State.Chase:
				break;
			case State.Attack:
				break;
			case State.Flee:
				break;
			default:
				break;
		}
	}

	void FixedUpdate() {
		switch (state)
		{
			case State.Patrol:
				// WalkToNextPoint();
				break;
			case State.Chase:
				// WalkTowardsPlayer();
				break;
			case State.Attack:
				// Do nothing? Proximity damage for now?
				break;
			case State.Flee:
				// FleeFromPlayer();
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Projectile"))
        {
            currentHealth -= 1;
            animator.SetInteger("health", currentHealth);
        }
	}
}
