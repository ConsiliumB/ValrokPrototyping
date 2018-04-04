using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundController : MonoBehaviour
{

    public static MenuBackgroundController Instance;

    public List<GameObject> tiles = new List<GameObject>();
    public AnimationCurve easeInCurve;

    private List<Coordinate> coordinates = new List<Coordinate>();

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //GameObject tile = new GameObject("Tiles");
        StartCoroutine("GenerateBackground");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GenerateBackground()
    {
        var origin = new Coordinate(0, 0);
        var directions = new Coordinate[] {
            Coordinate.North,
            Coordinate.North + Coordinate.East,
            Coordinate.East,
            Coordinate.East + Coordinate.South,
            Coordinate.South,
            Coordinate.South + Coordinate.West,
            Coordinate.West,
            Coordinate.West + Coordinate.North
        };

        Coordinate curr;
        coordinates.Add(origin);

        foreach (var item in directions)
        {
            coordinates.Add(origin + item);
        }

        foreach (var item in directions)
        {
            curr = origin + item * 2;
            if (curr.X == origin.X)
            {
                if (curr.Y > 0)
                {
                    coordinates.Add(curr + Coordinate.West);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.East);
                }
                else
                {
                    coordinates.Add(curr + Coordinate.East);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.West);
                }
            }
            else if (curr.Y == origin.Y)
            {
                if (curr.X > 0)
                {
                    coordinates.Add(curr + Coordinate.North);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.South);
                }
                else
                {
                    coordinates.Add(curr + Coordinate.South);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.North);
                }

            }
            else
            {
                coordinates.Add(curr);
            }
        }

        foreach (var item in directions)
        {
            curr = origin + item * 3;
            if (curr.X == origin.X)
            {
                if (curr.Y > 0)
                {
                    coordinates.Add(curr + Coordinate.West * 2);
                    coordinates.Add(curr + Coordinate.West);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.East);
                    coordinates.Add(curr + Coordinate.East * 2);
                }
                else
                {
                    coordinates.Add(curr + Coordinate.East * 2);
                    coordinates.Add(curr + Coordinate.East);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.West);
                    coordinates.Add(curr + Coordinate.West * 2);
                }

            }
            else if (curr.Y == origin.Y)
            {
                if (curr.X > 0)
                {
                    coordinates.Add(curr + Coordinate.North * 2);
                    coordinates.Add(curr + Coordinate.North);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.South);
                    coordinates.Add(curr + Coordinate.South * 2);
                }
                else
                {
                    coordinates.Add(curr + Coordinate.South * 2);
                    coordinates.Add(curr + Coordinate.South);
                    coordinates.Add(curr);
                    coordinates.Add(curr + Coordinate.North);
                    coordinates.Add(curr + Coordinate.North * 2);
                }

            }
            else
            {
                coordinates.Add(curr);
            }
            yield return null;
        }

        StartCoroutine("InstantiateBackground");
    }

    IEnumerator InstantiateBackground()
    {
        GameObject tile;
        foreach (Coordinate item in coordinates)
        {
            /*if (Random.Range(0, 5) <= 1f)
            {
                tile = tiles[1];
            }
            else
            {*/
                //Test allways 1
                tile = tiles[0];
            //}

            Instantiate(tile, WorldGen.MapToPixel(item), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
