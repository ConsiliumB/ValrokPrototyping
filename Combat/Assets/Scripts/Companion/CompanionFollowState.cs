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
        Movement.StopMoving();
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
    //private PlayerController Player;
    //private CompanionController compController;
    private GameObject companion;

    public CompanionTakeOverState(GameObject from)
    {
        companion = from;
        //compController = companion.GetComponent<CompanionController>();
    }


    public override void Execute()
    {
        var takeOver = companion.GetComponent<TakeControll>();
        if (takeOver == null) { throw new MissingComponentException(); }
        takeOver.RunExecute();
    }
}

public class ChaseAndAttackState : State
{
    private CompanionController Companion;
    private StatefulEntity Target;
    private Movement Movement;

    //DEBUG
    public float timer;

    public ChaseAndAttackState(StatefulEntity target)
    {
        Target = target;
    }

    public override void PrepareState()
    {
        Companion = CompanionController.Instance;
        Movement = Companion.GetComponent<Movement>();

        Target.PositionUpdate += FindPathToTarget;

        //Listen for death!?
    }

    public override void FinishState()
    {
        Target.PositionUpdate -= FindPathToTarget;
    }

    public override void Execute()
    {
        timer += Time.deltaTime;
        if (timer > 0.2f)
        {
            var targetDistance = Target.gameObject.transform.position - Companion.transform.position;

            if (targetDistance.magnitude <= 1)
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

    private void FindPathToTarget()
    {
        var targetDistance = Target.transform.position - Companion.transform.position;

        if (targetDistance.magnitude > 1)
            Movement.AddWaypoint(Target.Position, true);
    }

    private void Attack()
    {
        var direction = Target.transform.position - Companion.transform.position;
        Debug.Log("Attack!");
        Movement.StartMoving();

        //Companion.Shoot(direction);
    }
}

public class MoveCommandState : State
{
    public MoveCommandState(Vector2 targetPosition)
    {
        CompanionController.Instance.Movement.AddWaypoint(WorldGen.PixelToNodeMap(targetPosition), true);
    }
}