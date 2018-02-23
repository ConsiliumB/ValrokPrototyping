using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{

    public float delayMove = 0.5f;
    public GameObject snakeHead;
    public GameObject snakeBody;
    public WorldGen worldgenerator;

    private Map gameboard;
    private float delay = 0;
    public LinkedList<Coordinate> bodyCoords;
    public List<GameObject> body;

    // Use this for initialization
    void Start()
    {
        //body = new Coordinate[4];
        //Should add a number to size. This will do for now
        bodyCoords = new LinkedList<Coordinate>();
        Coordinate part = worldgenerator.PixelToMap(0, 0);
        bodyCoords.AddLast(part); //head
        part += Coordinate.East;
        bodyCoords.AddLast(part);
        part += Coordinate.South;
        bodyCoords.AddLast(part);
        part += Coordinate.South;
        bodyCoords.AddLast(part);
        part += Coordinate.South;
        bodyCoords.AddLast(part);
        part += Coordinate.South;
        bodyCoords.AddLast(part);

        gameboard = worldgenerator.GetMap();
        if (gameboard == null)
        {
            throw new MissingComponentException();
        }

        //List of referances to gameobject in the scene
        body = new List<GameObject>(5);

        //Initiate the body elements
        var current = bodyCoords.First;
        for (int i = 0; i < bodyCoords.Count; i++)
        {
            if (i == 0)
            {
                body.Add(Instantiate(snakeHead, worldgenerator.MapToPixel(current.Value),
                    Quaternion.identity, this.transform));
            }
            if (current.Next != null)
            {
                float angle;
                Vector2 pos;
                CalculateBetween(current.Value, current.Next.Value, out pos, out angle);
                body.Add(Instantiate(snakeBody, pos, Quaternion.Euler(0, 0, angle), this.transform));

            }
            //Go to next node
            current = current.Next;
        }

    }

    // Update is called once per frame
    void Update()
    {
        var horizontalIn = Input.GetAxisRaw("Horizontal");
        var verticalIn = Input.GetAxisRaw("Vertical");
        if (delay > delayMove)
        {
            Coordinate direction = new Coordinate(0, 0);
            if (horizontalIn == 1)
            {
                //right
                direction += Coordinate.East;
            }
            else if (horizontalIn == -1)
            {
                //left
                direction += Coordinate.West;
            }
            if (verticalIn == 1)
            {
                //up
                direction += Coordinate.North;
            }
            else if (verticalIn == -1)
            {
                //down
                direction += Coordinate.South;
            }
            if (horizontalIn != 0 || verticalIn != 0)
            {
                Move(direction);
                UpdatePosition();
                delay = 0;
            }

        }
        else
        {
            delay += Time.deltaTime;
        }
    }

    //Gets the position and angle between two cordinates
    //Added out parameters since it needs to return two things
    void CalculateBetween(Coordinate from, Coordinate to, out Vector2 betweenPosition, out float angle)
    {
        Vector2 vectFrom = worldgenerator.MapToPixel(from);
        Vector2 vectTo = worldgenerator.MapToPixel(to);
        betweenPosition = vectFrom - ((vectFrom - vectTo) / 2);
        float dx = vectFrom.x - vectTo.x;
        float dy = vectFrom.y - vectTo.y;
        angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg - 180;
    }

    //Updates positions and coordinates, moving the snake
    void Move(Coordinate direction)
    {
        Coordinate t = bodyCoords.First.Value + direction;
        bodyCoords.AddFirst(t);
        bodyCoords.RemoveLast();
    }

    //Iterates over bodyCoords (linkelist) and updates position of the snake body
    void UpdatePosition()
    {
        var node = bodyCoords.First;
        for (int i = 0; i < bodyCoords.Count; i++)
        {
            if (i == 0)
            {
                body[i].transform.position = worldgenerator.MapToPixel(node.Value);
            }
            if (node.Next != null)
            {
                Vector2 pos;
                float angle;
                CalculateBetween(node.Value, node.Next.Value, out pos, out angle);
                body[i + 1].transform.position = pos;
                //Quaternion angleNow = body[i + 1].transform.rotation;
                body[i + 1].transform.localEulerAngles = new Vector3(0, 0, angle);
                node = node.Next;
            }
        }
    }
}
