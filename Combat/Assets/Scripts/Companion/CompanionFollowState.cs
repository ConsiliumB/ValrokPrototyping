using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal class CompanionFollowState : State
{
    //private PlayerController Player { get { return PlayerController.Instance; } }
    private Movement Movement;
    private CompanionController Companion;
    private PlayerController Player;

    public override void PrepareState()
    {
        Companion = CompanionController.Instance;
        Player = PlayerController.Instance;
        Movement = Companion.GetComponent<Movement>();

        FindPathToPlayer();

        Player.PositionUpdate += FindPathToPlayer;
    }

    public override void FinishState()
    {
        Player.PositionUpdate -= FindPathToPlayer;
        Movement.ClearPath();
    }

    private void FindPathToPlayer()
    {
        var playerDistance = Player.transform.position - Companion.transform.position;

        if (playerDistance.magnitude > 2)
            Movement.AddWaypoint(Player.Position, true);
    }
}

/***
 * Take over state of the companion.
 * Controlls the companion movement with keyboard input
 */
internal class CompanionTakeOverState : State
{
    private GameObject companion;
    TakeControll takeOverScript;

    public CompanionTakeOverState(GameObject from)
    {
        companion = from;
        takeOverScript = companion.GetComponent<TakeControll>();
    }

    public override void Execute()
    {
        takeOverScript.RunExecute();
    }
}

public class ChaseAndAttackState : State
{
    private CompanionController Companion;
    private StatefulEntity Target;
    private Movement Movement;

    private float timer = 0;

    public ChaseAndAttackState(StatefulEntity target)
    {
        Target = target;
    }

    public override void PrepareState()
    {
        Companion = CompanionController.Instance;
        Movement = Companion.GetComponent<Movement>();

        Target.PositionUpdate += FindPathToTarget;

        FindPathToTarget();

        Target.DeathUpdate += TargetDead;

        //Listen for death!?
    }

    private void TargetDead()
    {
        Companion.ChangeState(new CompanionFollowState());
    }

    public override void FinishState()
    {
        Target.PositionUpdate -= FindPathToTarget;
        Target.DeathUpdate -= TargetDead;
    }

    public override void Execute()
    {
        var targetDistance = Target.transform.position - Companion.transform.position;
        timer += Time.deltaTime;

        if (targetDistance.magnitude < Companion.attackRadius)
        {
            Movement.StopMoving();
            if (timer > Companion.attackSpeed)
            {
                Attack();
                timer = 0;
            }
        }
        else if (!Movement.IsMoving())
        {
            Movement.StartMoving();
            FindPathToTarget();
        }
    }

    private void FindPathToTarget()
    {
        Movement.AddWaypoint(Target.Position, true);
    }

    private void Attack()
    {
        var direction = Target.transform.position - Companion.transform.position;
        Debug.Log("Attack!");

        Companion.Attack(direction);
    }
}

public class MoveCommandState : State
{
    Vector2 target;
    public MoveCommandState(Vector2 targetPosition)
    {
        target = targetPosition;
    }

    public override void PrepareState()
    {
        CompanionController.Instance.Movement.AddWaypoint(WorldGen.PixelToNodeMap(target), true);
    }
}