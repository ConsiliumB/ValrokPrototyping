using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed;

    //Subscribe to this in controller if you want to act based on movement/direction
    public delegate void MovementUpdateHandler(Vector2 direction);
    public event MovementUpdateHandler MovementUpdate;

    private List<Coordinate> Path = new List<Coordinate>();
    private bool moving = false;

    //Variables used for path movement
    private Coordinate pathNode;
    private Vector2 targetPosition;
    private Vector2 initialPosition;
    private Vector2 interpolatedMovement;

    public Coordinate Destination
    {
        get
        {
            if (Path.Count < 1)
            {
                return WorldGen.PixelToNodeMap(transform.position);
            }
            else
            {
                return Path[Path.Count - 1];
            }
        }
    }

    //Add waypoint to path. If overwrite is true, will clear the current path
    public void AddWaypoint(Coordinate newDestination, bool overwrite = false)
    {
        if (Destination == newDestination)
        {
            return;
        }

        if (overwrite)
        {
            ClearPath();
        }

        var newPath = Pathfinding.GetPath(Destination, newDestination);

        if (newPath.Count > 0)
        {
            if (Path.Count > 0)
            {
                newPath.RemoveAt(0);
            }
            Path.AddRange(newPath);
        }
        StartMoving();
    }

    //Clears the current path. Will leave one node if currently moving to it
    public void ClearPath()
    {
        if (Path.Count > 0)
        {
            Path.RemoveRange(1,Path.Count - 1);
        }
    }

    public void Update()
    {
        if (moving)
        {
            MoveAlongPath();
        }
    }

    //Enables movement
    public void StartMoving()
    {
        moving = true;
    }

    //Disables movement
    public void StopMoving()
    {
        if(MovementUpdate != null)
        {
            MovementUpdate(Vector2.zero);
        }
        moving = false;
    }

    public bool IsMoving()
    {
        return moving;
    }

    //Move towards next point in current path
    public void MoveAlongPath()
    {
        //If current path is empty, stop moving
        if (Path == null || Path.Count < 1)
        {
            StopMoving();
            //Debug.Log("Destination reached.");
            return;
        }


        //Current node should always be the first in path.
        if (pathNode != Path[0])
        {
            pathNode = Path[0];

            //Set target/initial positions and calculate normalized vector
            targetPosition = WorldGen.NodeMapToPixel(pathNode);
            initialPosition = transform.position;
            interpolatedMovement = (targetPosition - initialPosition).normalized;

            //Debug.Log("Next node. Moving from " + initialPosition + " to " + targetPosition);

            //Start movement animation in the direction of the current target node
            if (MovementUpdate != null)
            {
                MovementUpdate(interpolatedMovement);
            }
        }

        //Move to the next destination
        //Entity.rigidbody.MovePosition(interpolatedMovement);
        transform.Translate(interpolatedMovement * Time.smoothDeltaTime * movementSpeed);

        /*
         * This could give increased precision and no risk of just walking past the target position
        Vector2 nextPosition = (Vector2)transform.position + interpolatedMovement * Time.smoothDeltaTime * movementSpeed;

        //Check if next position would go past target position
        if (Vector2.Dot((targetPosition - nextPosition), interpolatedMovement) < 0)
        {
            Path.Remove(pathNode);
        }
        */

        //If we've reached our current destination, remove the path node we just reached
        if ((targetPosition - (Vector2)transform.position).magnitude < Time.smoothDeltaTime * movementSpeed * 2)
        {
            Path.Remove(pathNode);
        }
    }
}
