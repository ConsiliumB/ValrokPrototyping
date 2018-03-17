using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : StatefulEntity
{
    public static CompanionController Instance { get; private set; }

    public int proximityLimit;
    public int distanceLimit;

    public GameObject world;
    public Map worldMap;
    [Space]
    public Vector2 headingToPlayer;
    private Vector2 directionToPlayer;
    //public bool Moving { get; set; }

    private bool takeOver = false;
    public CompanionMovement Movement;

    private Animator animator;
    private float prevDirX;
    private float prevDirY;



    public void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this;
    }

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeToTakeover()
    {
        takeOver = true;
        ChangeState(new CompanionTakeOverState(gameObject));
    }

    public void RestartCompanion()
    {
        ChangeState(new CompanionFollowState(this));

        var takeoverScript = gameObject.GetComponent<TakeControll>();
        if (takeoverScript)
        {
            takeoverScript.UndoTakeover();
        }
        takeOver = false;

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
        }

        animator.SetFloat("DirX", heading.x);
        animator.SetFloat("DirY", heading.y);
    }
}
