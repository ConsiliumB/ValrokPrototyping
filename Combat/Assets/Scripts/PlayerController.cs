using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StatefulEntity
{
    public static PlayerController Instance { get; private set; }

    public int speed;

    //public int currentHealth;
    [Header("Lock movement. Currently Debug")]
    public bool lockMovement = false;

    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRend;
    private Animator animator;
    private bool isDead;
    private float vmov;
    private float hmov;

    private float prevDirX;
    private float prevDirY;

    void Awake()
    {
        Instance = this;

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    //Kept the E key press for player world interaction
    void Update()
    {

        NextMove();

        if (Input.GetKeyDown(KeyCode.E))
        {
            CastSpell("E");
        }

        if (isDead)
        {
            rigidbody.velocity *= 0;
            rigidbody.rotation = 180;
            return;
        }
    }

    //Moves the player in a new direction
    private void NextMove()
    {
        if (!lockMovement)
        {
            vmov = Input.GetAxis("Vertical");
            hmov = Input.GetAxis("Horizontal");
            Vector2 nextDirection = new Vector2(hmov, vmov);

            Vector2 nextPosition = (Vector2)transform.position + (nextDirection * speed * Time.deltaTime);
            //TODO Check if next position crashes or not

            rigidbody.MovePosition(nextPosition);

            if (nextDirection.x > 0)
            {
                spriteRend.flipX = false;
            }
            else if (nextDirection.x < 0)
            {
                spriteRend.flipX = true;
            }

            UpdateAnimation(nextDirection.normalized);
        }
    }

    //If the player changes anything from start of game reset it here.
    private void RestartPlayer()
    {
        vmov = 0;
        hmov = 0;
        lockMovement = false;
    }

    void UpdateAnimation(Vector2 heading)
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

    //lock play in place when touching the corruption
    public void LockPlayer()
    {
        lockMovement = true;
        rigidbody.velocity *= 0;
        hmov = 0;
        vmov = 0;
        //Walk down one frame to make it idle down next frame
        UpdateAnimation(Vector2.down + Vector2.left);
        UpdateAnimation(Vector2.zero);
    }

    public void UnlockPlayer()
    {
        lockMovement = false;
    }

    private void CastSpell(string v)
    {
        switch (v)
        {
            case "E":
                Debug.Log("Companion, come hither!");
                CompanionController.Instance.ChangeState(new CompanionFollowState(CompanionController.Instance));
                break;
            case "Q":
                break;
            default:
                break;
        }
    }

    //Currently on the corruption spawner object
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CorruptionSpawner"))
        {
            vmov = 0;
            hmov = 0;
            UpdateAnimation(Vector2.down + Vector2.right);
            lockMovement = true;
        }
    }*/

}
