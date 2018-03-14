using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeController : StatefulEntity {

    // Use this for initialization
    void Start () {
        ChangeState(new ChaseNearestState(this));
	}
	
	// Update is called once per frame
	void Update () {
        currentState.Execute();
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

        Player.PositionUpdate += delegate ()
        {
            Debug.Log("Player moved");
            UpdateNearest();

            if (Nearest == Player)
            {
                FindPathToNearest();
            }
        };

        Companion.PositionUpdate += delegate ()
        {
            Debug.Log("Companion moved");
            UpdateNearest();

            if (Nearest == Companion)
            {
                FindPathToNearest();
            }
        };
    }

    public override void Execute()
    {
        //timer += Time.deltaTime;
        //if (timer > 0.5f)
        //{
        //    UpdateNearest();
        //    timer = 0;
        //}
    }

    private void FindPathToNearest()
    {
        Movement.AddWaypoint(Nearest.Position, true);
    }

    private void UpdateNearest()
    {
        companionHeading = Companion.transform.position - Shade.transform.position;
        playerHeading = Player.transform.position - Shade.transform.position;
        if (companionHeading.sqrMagnitude < playerHeading.sqrMagnitude)
        {
            Nearest = Companion;
        }
        else
        {
            Nearest = Player;
        }
    }

    public override void FinishState()
    {
        base.FinishState();
    }
}
