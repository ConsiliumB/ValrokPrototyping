using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Subscribe to this in controller if you want to act based on movement/direction
    public delegate void MovementUpdatedHandler(Vector2 direction);
    public event MovementUpdatedHandler MovementUpdated;

    private List<Coordinate> Path = new List<Coordinate>();
    private bool moving = false;

    //Variables used for interpolated movement
    private Coordinate pathNode;
    private Vector2 targetPosition;
    private Vector2 initialPosition;
    private Vector2 interpolatedMovement;
    private float interpolation;

    public Coordinate Destination
    {
        get
        {
            if (Path.Count < 1)
            {
                return null;
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
            if (interpolation > 0)
            {
                Path.RemoveRange(1, Path.Count - 1);
            }
            else
            {
                Path.Clear();
            }
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
        MovementUpdated(Vector2.zero);
        moving = false;
    }

    public bool IsMoving()
    {
        return moving;
    }

    /***
     * A lot of comments
     * Just comment inside a method if you use black magic
    */

    //Move towards next point in current path
    //Nullable optinal Vector2 = black magic.
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
            //Reset interpolation 
            interpolation = 0;

            //Set target/initial positions to use when interpolating/lerping
            targetPosition = WorldGen.NodeMapToPixel(pathNode);
            initialPosition = transform.position;


            //Debug.Log("Next node. Moving from " + initialPosition + " to " + targetPosition);

            //Start movement animation in the direction of the current target node
            MovementUpdated(targetPosition - initialPosition);
        }

        //Increase interpolation.
        //Divide to regulate speed, currently 0.1f, should be public variable
        interpolation += Time.smoothDeltaTime / 0.1f;

        //Find next destination
        interpolatedMovement = Vector2.Lerp(initialPosition, targetPosition, interpolation);
        //Debug.Log("Moving to " + interpolatedMovement);

        //Move to the next destination
        //Entity.rigidbody.MovePosition(interpolatedMovement);
        transform.position = interpolatedMovement;

        //If we've reached our destination, reset interpolation and remove the path we just reached
        if (interpolation >= 1)
        {
            interpolation = 0;
            Path.Remove(pathNode);
        }
    }
}
