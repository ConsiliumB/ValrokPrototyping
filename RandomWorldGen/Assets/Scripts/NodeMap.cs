using System;
using System.Collections.Generic;

public class NodeMap : Map
{
    private Dictionary<Coordinate, MapNode> map;

    public NodeMap()
    {
        this.map = new Dictionary<Coordinate, MapNode>();
    }

    public MapNode GetNode(Coordinate position)
    {
        return map.ContainsKey(position) ? map[position] : null;
    }

    //Adds node at given position
    public void AddNode(Coordinate position)
    {
        if (!map.ContainsKey(position))
        {
            map[position] = new MapNode(position);
        }
    }


    //Link all neighbouring nodes together
    public void GenerateNodeMap()
    {
        //Loop through every node in the map
        foreach (MapNode node in map.Values)
        {
            LinkWithNeighbours(node);
        }
    }

    //Link one node with its neighbours
    private void LinkWithNeighbours(MapNode node)
    {
        Coordinate neighbourCoordinate;
        //Loop through every direction
        foreach (Coordinate direction in Coordinate.directions)
        {
            //Check if node has a neighbour in given direction
            neighbourCoordinate = node.Position + direction;

            if (map.ContainsKey(neighbourCoordinate))
            {
                node.AddNeighbour(GetNode(neighbourCoordinate));
            }
        }
    }

    public bool IsOpen(int x, int y)
    {
        return IsOpen(new Coordinate(x, y));
    }

    //Returns true if the node exists and has a walkable tile(tile > 0)
    public bool IsOpen(Coordinate coordinate)
    {
        return map.ContainsKey(coordinate) && map[coordinate].Tile > 0;
    }
    
    public bool IsBlocked(int x, int y)
    {
        return !(IsOpen(x, y));
    }

    public bool IsBlocked(Coordinate coordinate)
    {
        return !(IsOpen(coordinate));
    }

    public bool WithinBounds(Coordinate coordinate)
    {
        return map.ContainsKey(coordinate);
    }
}

public class MapNode
{
    public Coordinate Position { get; private set; }
    public int Tile { get; set; }
    private List<MapNode> neighbours;

    public MapNode(Coordinate position, int tile = 1)
    {
        neighbours = new List<MapNode>();
        Position = position;
        Tile = tile;
    }

    public void AddNeighbour(MapNode node)
    {
        neighbours.Add(node);
    }

    //Necessary?
    public IEnumerable<MapNode> Neighbours
    {
        get { return neighbours; }
    }
}
