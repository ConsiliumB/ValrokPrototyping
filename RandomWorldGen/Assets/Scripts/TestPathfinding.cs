using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{

    public float moveDelay = 2.3f; //Time between each move
    public Transform from;
    public Transform to;
    public GameObject road;

    private Coordinate start;
    private Coordinate end;
    private WorldGen worldGenScript;
    private Pathfinding pathfinder;
    private Map nodeMap;
    private Map globalMap;
    private GameObject pathContainer;

    private float moveTimer = 0;
    private int counter = 0;
    List<Coordinate> path;

    // Use this for initialization
    void Start()
    {
        pathfinder = new Pathfinding();
        GameObject world = GameObject.Find("World");
        globalMap = world.GetComponent<WorldGen>().GetMap();
        nodeMap = world.GetComponent<WorldGen>().nodeMap;
        worldGenScript = world.GetComponent<WorldGen>();
        pathContainer = new GameObject("Path");

        if (globalMap == null)
        {
            throw new MissingComponentException();
        }
        
        //if (path == null)
        //{
        //    throw new MissingComponentException();
        //}

        //print the path
        /*foreach (Coordinate t in path)
        {
            Debug.Log(t);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (path != null)
        {
            if (counter < path.Count)
            {
                moveTimer += Time.deltaTime;

                if (moveTimer >= moveDelay)
                {
                    Vector2 nextCoordinate = worldGenScript.NodeMapToPixel(path[counter]);
                    transform.position = nextCoordinate;
                    counter++;
                    moveTimer = 0;
                }
            }
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Find path using grid"))
            FindPath();
        if (GUI.Button(new Rect(10, 60, 150, 50), "Find path using node"))
            FindPathUsingNode();
    }

    public void FindPath()
    {
        GetPathStartAndEnd();

        path = pathfinder.GetPath(nodeMap, start, end);

        
        foreach (var item in path)
        {
            var pos = worldGenScript.NodeMapToPixel(item);
            Instantiate(road, pos, Quaternion.identity).transform.parent = pathContainer.transform;
        }
        
        counter = 0;
    }

    private void GetPathStartAndEnd()
    {
        start = worldGenScript.PixelToNodeMap(from.position.x, from.position.y);
        end = worldGenScript.PixelToNodeMap(to.position.x, to.position.y);
    }

    public void FindPathUsingNode()
    {
        GetPathStartAndEnd();

        path = pathfinder.GetNodePath((NodeMap)nodeMap, start, end);

        foreach (var item in path)
        {
            var pos = worldGenScript.NodeMapToPixel(item);
            Instantiate(road, pos, Quaternion.identity).transform.parent = pathContainer.transform;
        }

        counter = 0;
    }
}
