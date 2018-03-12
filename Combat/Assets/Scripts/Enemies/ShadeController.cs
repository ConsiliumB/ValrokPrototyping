using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeController : StatefulEntity {

    //public Movement Movement { get; private set; }

    // Use this for initialization
    void Start () {
        ChangeState(new ChaseNearestState(this));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class ChaseNearestState : State
{
    private CompanionController Companion;
    private PlayerController Player;
    private ShadeController Shade;

    private Vector2 playerHeading;
    private Vector2 companionHeading;

    public ChaseNearestState(ShadeController shade)
    {
        Shade = shade;
    }

    public override void PrepareState()
    {
        Player = PlayerController.Instance;
        Companion = CompanionController.Instance;
    }

    public override void Execute()
    {
        companionHeading = Companion.transform.position - Shade.transform.position;
        playerHeading = Player.transform.position - Shade.transform.position;
        if (companionHeading.sqrMagnitude < playerHeading.sqrMagnitude)
        {
            //movement?
        }
        else
        {
            //movement?
        }
    }

    public override void FinishState()
    {
        base.FinishState();
    }
}
