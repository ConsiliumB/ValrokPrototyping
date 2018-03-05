using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionMovement : MonoBehaviour {
    private CompanionController Companion { get; set; }
    public List<Coordinate> Path { get; private set; }

    Coordinate pathNode;
    Vector2 targetPosition;
    Vector2 initialPosition;
    Vector2 interpolatedMovement;
    float Interpolation { get; set; }
    public NodeMap WorldMap { get; set; }
    public bool Moving { get; private set; }
    public Coordinate Destination {
        get {
            if (Path.Count < 1)
            {
                return Companion.Position;
            }
            else
            {
                return Path[Path.Count - 1];
            }
        }
    }

    private void Awake()
    {
        Moving = false;
        Path = new List<Coordinate>();
    }

    public void Start()
    {
        Companion = GetComponent<CompanionController>();
        WorldMap = Pathfinding.Graph;
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
            if (Interpolation > 0)
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
        if (Moving)
        {
            MoveAlongPath();
        }
    }

    //Enables movement
    public void StartMoving()
    {
        Moving = true;
    }

    //Disables movement
    public void StopMoving()
    {
        Companion.UpdateAnimation(Vector2.zero);
        Moving = false;
    }

    /***
     * A lot of comments
     * Just comment inside a method if you use black magic
    */

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
            //Reset interpolation 
            Interpolation = 0;

            //Set target/initial positions to use when interpolating/lerping
            targetPosition = WorldGen.NodeMapToPixel(pathNode);
            initialPosition = Companion.transform.position;

            //Debug.Log("Next node. Moving from " + initialPosition + " to " + targetPosition);

            //Start movement animation in the direction of the current target node
            Companion.UpdateAnimation(targetPosition - initialPosition);
        }

        //Increase interpolation.
        //Divide to regulate speed, currently 0.1f, should be public variable
        Interpolation += Time.smoothDeltaTime / 0.1f;

        //Find next destination
        interpolatedMovement = Vector2.Lerp(initialPosition, targetPosition, Interpolation);
        //Debug.Log("Moving to " + interpolatedMovement);

        //Move to the next destination
        //Companion.rigidbody.MovePosition(interpolatedMovement);
        transform.position = interpolatedMovement;

        //If we've reached our destination, reset interpolation and remove the path we just reached
        if (Interpolation >= 1)
        {
            Interpolation = 0;
            Path.Remove(pathNode);
        }
    }
}
