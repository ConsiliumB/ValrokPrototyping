using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public List<Coordinate> GetPath(Map map, Coordinate start, Coordinate destination)
    {
        var checkedCoordinates = new Dictionary<Coordinate, Node>();
        var promiseList = new NodePromiseList();

        if (start == destination || map.IsBlocked(start) || map.IsBlocked(destination))
        {
            Debug.Log("No path");
            return null;
        }

        promiseList.Add(new Node(null, start, 0, Coordinate.Distance(start, destination)));

        Coordinate next;
        Node current = null;

        while (promiseList.Count > 0)
        {
            current = promiseList.PopLowestPromise();

            if (checkedCoordinates.ContainsKey(current.position))
            {
                continue;
            }

            checkedCoordinates[current.position] = current;

            if (current.position == destination)
            {
                List<Coordinate> path = new List<Coordinate>();
                while (current.parent != null)
                {
                    path.Add(current.position);
                    current = current.parent;
                }
                path.Add(start);
                path.Reverse();

                return path;
            }

            foreach (Coordinate direction in Coordinate.directions)
            {
                next = current.position + direction;
                if (!map.WithinBounds(next) || checkedCoordinates.ContainsKey(next) || map.IsBlocked(next))
                {
                    continue;
                }

                //Dont cross non-traversable corners when going diagonally
                if (IsDiagonalDirection(direction) && (map.IsBlocked(current.position + new Coordinate(direction.X, 0)) || map.IsBlocked(current.position + new Coordinate(0, direction.Y))))
                {
                    continue;
                }

                Node newNode = new Node(current, next, current.cost + 1, current.cost + 1 + Coordinate.Distance(next, destination));
                promiseList.Add(newNode);
            }
        }

        Debug.Log("No path");
        return null;
    }

    public static bool IsDiagonalDirection(Coordinate direction)
    {
        return (Math.Abs(direction.X) + Math.Abs(direction.Y) == 2);
    }
}

public class Node
{
    public Node parent;
    public Coordinate position;
    public int cost;
    public int promise;

    public Node(Node parent, Coordinate position, int cost, int promise)
    {
        this.parent = parent;
        this.position = position;
        this.cost = cost;
        this.promise = promise;
    }
}

public class NodePromiseList : List<Node>
{
    /* Temporary!
     * Better to add at sorted position than finding lowest promise.
     * "return lowest promise" always requires one pass of all values, while "insert at promise" only iterates until it finds its position 
     */
    public Node PopLowestPromise()
    {
        if (Count == 0)
        {
            return null;
        }

        Node cheapestNode = this[0];
        foreach (Node node in this)
        {
            if (node.promise < cheapestNode.promise)
            {
                cheapestNode = node;
            }
        }
        this.Remove(cheapestNode);
        return cheapestNode;
    }
}