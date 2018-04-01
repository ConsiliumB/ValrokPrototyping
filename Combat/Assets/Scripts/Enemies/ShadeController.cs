using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeController : StatefulEntity {

    public GameObject projectile;
    public float projectileSpeed;

    // Use this for initialization
    void Start () {
        StartCoroutine("CheckPositionChange");
        ChangeState(new ChaseNearestState(this));
    }

    // Update is called once per frame
    void Update () {
        currentState.Execute();
	}

    public void Shoot(Vector3 heading)
    {
        Debug.Log(heading);
        Debug.Log(heading.normalized);
        var bullet = Instantiate(projectile, transform.position + new Vector3(0, 1.2f, 0) + heading.normalized, transform.rotation);
        bullet.transform.right = heading.normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = (Vector2)heading.normalized * projectileSpeed;

        Destroy(bullet, 5.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CompanionAttack"))
        {
            TakeDamage();
        }
    }
}

public class ChaseNearestState : State
{
    private CompanionController Companion;
    private PlayerController Player;
    private ShadeController Shade;
    private StatefulEntity Nearest;
    private Movement Movement;

    private Vector2 playerHeading;
    private Vector2 companionHeading;

    //DEBUG
    public float timer;

    public ChaseNearestState(ShadeController shade)
    {
        Shade = shade;
    }

    public override void PrepareState()
    {
        Movement = Shade.GetComponent<Movement>();
        Player = PlayerController.Instance;
        Companion = CompanionController.Instance;

        Nearest = FindNearest();
        FindPathToNearest();


        Player.PositionUpdate += delegate ()
        {
            var previousNearest = Nearest;
            Nearest = FindNearest();
            if (Nearest != previousNearest || Nearest == Player)
            {
                FindPathToNearest();
            }
        };

        Companion.PositionUpdate += delegate ()
        {
            var previousNearest = Nearest;
            Nearest = FindNearest();
            if (Nearest != previousNearest || Nearest == Companion)
            {
                FindPathToNearest();
            }
        };
    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            var nearestDistance = Nearest.transform.position - Shade.transform.position;

            if (nearestDistance.magnitude < 10)
            {
                Attack();
                Movement.StopMoving();
                timer = -2;
            }
            else
            {
                timer = 0;
            }
        }
    }

    private void FindPathToNearest()
    {
        var nearestDistance = Nearest.transform.position - Shade.transform.position;

        if (nearestDistance.magnitude > 10)
            Movement.AddWaypoint(Nearest.Position, true);
    }

    private StatefulEntity FindNearest()
    {
        companionHeading = Companion.transform.position - Shade.transform.position;
        playerHeading = Player.transform.position - Shade.transform.position;
        if (companionHeading.sqrMagnitude < playerHeading.sqrMagnitude)
        {
            return Companion;
        }
        else
        {
            return Player;
        }
    }
    private void Attack()
    {
        var direction = Nearest.transform.position - Shade.transform.position;

        Shade.Shoot(direction);
    }
}
