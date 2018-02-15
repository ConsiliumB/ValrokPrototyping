using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{

    public float moveDelay = 2.3f; //Time between each move

    private Coordinate start;
    private Coordinate end;
    private WorldGen worldGenScript;
    private Pathfinding pathfinder;
    private Map globalMap;
    private float moveTimer = 0;
    private int counter = 0;
    List<Coordinate> path;

    // Use this for initialization
    void Start()
    {
        pathfinder = new Pathfinding();
        GameObject world = GameObject.Find("World");
        globalMap = world.GetComponent<WorldGen>().GetMap();
        worldGenScript = world.GetComponent<WorldGen>();

        if (globalMap == null)
        {
            throw new MissingComponentException();
        }

        start = new Coordinate(48, 48);
        end = new Coordinate(64, 49);
        path = pathfinder.GetPath(globalMap, start, end);
        if (path == null)
        {
            throw new MissingComponentException();
        }

        //print the path
        /*foreach (Coordinate t in path)
        {
            Debug.Log(t);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (counter < path.Count)
        {
            moveTimer += Time.deltaTime;
        }
        
        if (moveTimer >= moveDelay)
        {
            Coordinate t = path[counter];
            Vector2 nextCoordinate = worldGenScript.MapToPixel(t.X, t.Y);
            transform.position = nextCoordinate;
            counter++;
            moveTimer = 0;
        }
    }
}
