using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal class CompanionFollowState : State
{
    private PlayerController Player { get; set; }
    private CompanionController Companion { get; set; }

    public CompanionFollowState(CompanionController companion)
    {
        Player = Pathfinding.Player;
        Companion = companion;
    }

    public override void Execute()
    {
        //If current destination is not player position AND players distance to our current destination is larger than our proximitylimit, add waypoint
        if (Companion.Movement.Destination != Player.Position && Coordinate.DistanceSquared(Player.Position, Companion.Movement.Destination) > Companion.proximityLimit * Companion.proximityLimit)
        {
            //If player is closer than current destination, find new path. Else, append path from destination to player
            if (Coordinate.DistanceSquared(Player.Position, Companion.Position) < Coordinate.DistanceSquared(Companion.Position, Companion.Movement.Destination))
            {
                Companion.Movement.AddWaypoint(Player.Position, true);
            } else
            {
                Companion.Movement.AddWaypoint(Player.Position);
            }
        }
    }

    public override void PrepareState()
    {
        //Find path to player, and start moving
        Companion.Movement.AddWaypoint(Player.Position, true);
        Companion.Movement.StartMoving();
    }
}