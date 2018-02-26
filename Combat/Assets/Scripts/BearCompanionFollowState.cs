using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal class BearCompanionFollowState : MonoBehaviour, IState
{
    private bool Moving { get; set; }
    private List<Coordinate> path;
    private CompanionController Entity { get; set; }
    public BearCompanionFollowState(CompanionController entity)
    {
        this.Entity = entity;
        Moving = false;
    }

    public void Execute()
    {
        Entity.headingToPlayer = Entity.player.transform.position - Entity.transform.position;
        //directionToPlayer = headingToPlayer / headingToPlayer.magnitude;

        if (Entity.headingToPlayer.sqrMagnitude > Entity.proximityLimit * Entity.proximityLimit)
        {
            if (Entity.headingToPlayer.sqrMagnitude > Entity.distanceLimit * Entity.distanceLimit)
            {
                Entity.StopCoroutine("MoveTowardsPlayer");
                Entity.moving = false;
                Entity.TeleportToPlayer();
            }
            else
            {
                Entity.StartCoroutine("MoveTowardsPlayer");
            }
        }
    }

    public void PrepareState()
    {
    }

    public void FinishState()
    {
    }
}