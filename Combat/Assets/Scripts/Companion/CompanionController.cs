using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : StatefulEntity
{
    public float attackSpeed;
    public float attackRadius;

    public static CompanionController Instance { get; private set; }

    public Movement Movement;
    private Animator animator;
    private float prevDirX;
    private float prevDirY;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        Movement = GetComponent<Movement>();
        Instance = this;
    }

    void Start()
    {
        Movement.MovementUpdate += UpdateAnimation;
        StartCoroutine("CheckPositionChange");
        ChangeState(new CompanionFollowState());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D scan = Physics2D.Raycast(position, Vector2.zero);

            if(scan)
            {
                StatefulEntity target = scan.transform.gameObject.GetComponent<StatefulEntity>();
                if(target && target != PlayerController.Instance)
                {
                    ChangeState(new ChaseAndAttackState(target));
                }
            }
        } else if (Input.GetMouseButtonDown(1))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ChangeState(new MoveCommandState(position));
        }
    }

    public void ChangeToTakeover()
    {
        ChangeState(new CompanionTakeOverState(gameObject));
    }

    public void RestartCompanion()
    {
        ChangeState(new CompanionFollowState());

        var takeoverScript = gameObject.GetComponent<TakeControll>();
        if (takeoverScript)
        {
            takeoverScript.UndoTakeover();
        }
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
