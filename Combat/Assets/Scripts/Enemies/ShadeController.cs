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
    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            Debug.Log("1s elapsed");
            Debug.Log(Player.Position + " and " + Companion.Position);

            //This comparison should only happen when player or companion moves.. Delegates anyone!?(YES)
            companionHeading = Companion.transform.position - Shade.transform.position;
            playerHeading = Player.transform.position - Shade.transform.position;
            if (companionHeading.sqrMagnitude < playerHeading.sqrMagnitude)
            {
                Movement.AddWaypoint(Companion.Position, true);
            }
            else
            {
                Movement.AddWaypoint(Player.Position, true);
            }
            Movement.StartMoving();
            timer = 0;
        }
    }

    public override void FinishState()
    {
        base.FinishState();
    }
}
