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
    private Map nodeMap;
    private Map globalMap;
    private GameObject pathContainer;

    private float moveTimer = 0;
    private int counter = 0;
    List<Coordinate> path;

    // Use this for initialization
    void Start()
    {
        GameObject world = GameObject.Find("World");
        nodeMap = world.GetComponent<WorldGen>().nodeMap;
        pathContainer = new GameObject("Path");
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
                    Vector2 nextCoordinate = WorldGen.NodeMapToPixel(path[counter]);
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

    private void GetPathStartAndEnd()
    {
        start = WorldGen.PixelToNodeMap(from.position.x, from.position.y);
        end = WorldGen.PixelToNodeMap(to.position.x, to.position.y);
    }

    public void FindPath()
    {
        GetPathStartAndEnd();

        path = Pathfinding.GetPath(nodeMap, start, end);


        ShowPath();

        counter = 0;
    }

    public void FindPathUsingNode()
    {
        GetPathStartAndEnd();

        path = Pathfinding.GetNodePath((NodeMap)nodeMap, start, end);

        ShowPath();

        counter = 0;
    }

    private void ShowPath()
    {
        if (path == null) return;
        foreach (var item in path)
        {
            var pos = WorldGen.NodeMapToPixel(item);
            Instantiate(road, pos, Quaternion.identity).transform.parent = pathContainer.transform;
        }
    }
}
