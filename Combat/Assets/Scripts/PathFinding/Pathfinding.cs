using System;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    public static NodeMap Graph { get; internal set; }

    //Find shortest path on a Map
    //Use GetNodePath this is 80% Obsolete
    public static List<Coordinate> GetPath(Map map, Coordinate start, Coordinate destination)
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


    /// Finds a path in the NodeMap(map) from start to destination
    /// Uses optimized Astar algorithem
    /// returns a empty list if start is blocked, end is blocked or start == end.
    /// Retruns a full list with each nodegrid to target on success
    public static List<Coordinate> GetNodePath(NodeMap map, Coordinate start, Coordinate destination)
    {
        var checkedCoordinates = new Dictionary<Coordinate, Node>();
        var promiseList = new NodePromiseList();

        if (start == destination || map.IsBlocked(start) || map.IsBlocked(destination))
        {
            Debug.Log("No path");
            return new List<Coordinate>();
        }

        promiseList.AddSorted(new Node(null, start, 0, Coordinate.Distance(start, destination)));

        Node current = null;
        float cost = 1;

        while (promiseList.Count > 0)
        {
            current = promiseList[0];
            promiseList.Remove(current);

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


            /* To speed this up, diagonal neighbours could be in their own list.
             * This way we dont have to calculate the movement vector, check if they are diagonals and change the cost on every loop
             */
            foreach (var neighbour in map.GetNode(current.position).Neighbours)
            {
                if (checkedCoordinates.ContainsKey(neighbour.Position))
                {
                    continue;
                }

                if (IsDiagonalDirection(neighbour.Position - current.position))
                {
                    cost = 1.4f;
                }
                else
                {
                    cost = 1f;
                }

                Node newNode = new Node(current, neighbour.Position, current.cost + cost, current.cost + cost + Coordinate.Distance(neighbour.Position, destination));
                promiseList.AddSorted(newNode);
            }
        }

        Debug.Log("No path");
        return new List<Coordinate>();
    }

    public static List<Coordinate> GetPath(Coordinate start, Coordinate destination)
    {
        return GetNodePath(Graph, start, destination);
    }

    public static bool IsDiagonalDirection(Coordinate direction)
    {
        return (Math.Abs(direction.X) + Math.Abs(direction.Y) == 2);
    }
}

//The connection between tiles in the map/grid
public class Node : IComparable<Node>
{
    public Node parent;
    public Coordinate position;
    public float cost;
    public float promise;

    public Node(Node parent, Coordinate position, float cost = 0, float promise = 0)
    {
        this.parent = parent;
        this.position = position;
        this.cost = cost;
        this.promise = promise;
    }

    public int CompareTo(Node other)
    {
        if (other == null)
        {
            return 1;
        }

        return promise.CompareTo(other.promise);
    }
}

//Promiselist is created to calcualte shortes path in Astar
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

    /* Create another add to use binarysearch to find index, insert on absolutevalue
     * Greatly increases performance compared to poplowest solution.(x10)
     * 
     * Note: This adds "before" - if list operations are cheaper towards its end, maybe
     * inserting cheapest nodes at the end is better? Thats where most operations
     * will take place. (add, remove)
     */
    public void AddSorted(Node node)
    {
        var index = BinarySearch(node);
        if (index < 0) index = ~index;
        Insert(index, node);
    }
}
