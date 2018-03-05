using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : StatefulEntity
{

    public GameObject player;

    public int proximityLimit;
    public int distanceLimit;

    public GameObject world;
    public Map worldMap;
    [Space]
    public Vector2 headingToPlayer;
    private Vector2 directionToPlayer;
    //public bool Moving { get; set; }

    [Header("TakeOver Controll of the Companion")]
    public bool takeOver = false;
    //[Space]
    //public new Rigidbody2D rigidbody;
    public CompanionMovement Movement;

    private Animator animator;
    private float prevDirX;
    private float prevDirY;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        Pathfinding.Companion = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == null)
        {
            ChangeState(new CompanionFollowState(this));
        }
        if (takeOver)
        {
            ChangeState(new CompanionTakeOverState(gameObject));
        }

        currentState.Execute();
    }


    public void UpdateAnimation(Vector2 heading)
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

        var companionRenderer = GetComponent<SpriteRenderer>();

        if (heading.x > 0)
        {
            companionRenderer.flipX = false;
        }
        else if (heading.x < 0)
        {
            companionRenderer.flipX = true;
        } else if (companionRenderer.flipX)
        {
            companionRenderer.flipX = false;
        }

        animator.SetFloat("DirX", heading.x);
        animator.SetFloat("DirY", heading.y);
    }
}
