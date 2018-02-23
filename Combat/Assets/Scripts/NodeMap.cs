using System;
using System.Collections.Generic;

public class NodeMap : Map
{
    public Dictionary<Coordinate, MapNode> Map { get; private set; }

    public NodeMap()
    {
        this.Map = new Dictionary<Coordinate, MapNode>();
    }

    public MapNode GetNode(Coordinate position)
    {
        return Map.ContainsKey(position) ? Map[position] : null;
    }

    //Adds node at given position
    public void AddNode(Coordinate position)
    {
        if (!Map.ContainsKey(position))
        {
            Map[position] = new MapNode(position);
        }
    }

    public void AddNode(Coordinate position, int tile)
    {
        if (!Map.ContainsKey(position))
        {
            Map[position] = new MapNode(position, tile);
        }
    }


    //Link all neighbouring nodes together
    public void GenerateNodeMap()
    {
        //Loop through every node in the map
        foreach (MapNode node in Map.Values)
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

            if (Map.ContainsKey(neighbourCoordinate))
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
        return Map.ContainsKey(coordinate) && Map[coordinate].Tile > 0;
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
        return Map.ContainsKey(coordinate);
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
