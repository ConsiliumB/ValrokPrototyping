using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeController : StatefulEntity {

    public GameObject projectile;
    public float projectileSpeed;
    public float attackRadius;


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
        var bullet = Instantiate(projectile, transform.position + new Vector3(0, 1.2f, 0) + heading.normalized, transform.rotation);
        bullet.transform.right = heading.normalized;

        Attack attack = bullet.GetComponent<Attack>();
        attack.attacker = gameObject;
        attack.friendlyFire = true;

        bullet.GetComponent<Rigidbody2D>().velocity = (Vector2)heading.normalized * projectileSpeed;
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

        FindNearest();

        Player.PositionUpdate += PlayerMoved;
        Companion.PositionUpdate += CompanionMoved;
    }

    private void CompanionMoved()
    {
        TargetMoved(Companion);
    }

    private void PlayerMoved()
    {
        TargetMoved(Player);
    }

    void TargetMoved(StatefulEntity target)
    {
        var previousNearest = Nearest;
        Nearest = FindNearest();
        if (Movement.IsMoving())
        {
            if (Nearest != previousNearest || Nearest == target)
            {
                FindPathToNearest();
            }
        }
    }

    public override void Execute()
    {
        var nearestDistance = Nearest.transform.position - Shade.transform.position;
        timer += Time.deltaTime;

        if (nearestDistance.magnitude < Shade.attackRadius)
        {
            Movement.StopMoving();
            if (timer > 1.5f)
            {
                Attack();
                timer = 0;
            }
        } else if (!Movement.IsMoving())
        {
            FindPathToNearest();
            Movement.StartMoving();
        }
    }

    private void FindPathToNearest()
    {
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
