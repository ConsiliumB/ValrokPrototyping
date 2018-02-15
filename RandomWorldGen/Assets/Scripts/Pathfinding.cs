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
            return null;
        }

        Coordinate next;
        Node current = null;
        foreach (Coordinate direction in Coordinate.directions)
        {
            next = start + direction;
            if (!(map.WithinBounds(next)))
            {
                continue;
            }

            if (map.IsOpen(next))
            {
                current = new Node(null, next, 0, Coordinate.Distance(start, destination));
                checkedCoordinates[start] = current;
            }
        }

        promiseList.Add(current);

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

                //Dont cut corners
                /*if (map.IsBlocked(next + new Coordinate(direction.X, 0)) || map.IsBlocked(next + new Coordinate(0, direction.Y)))
                {
                    continue;
                }*/

                Node newNode = new Node(current, next, current.cost + 1, current.cost + 1 + Coordinate.Distance(next, destination));
                promiseList.Add(newNode);
                //current = newNode;
            }
        }

        Debug.Log("No path");
        return null;
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