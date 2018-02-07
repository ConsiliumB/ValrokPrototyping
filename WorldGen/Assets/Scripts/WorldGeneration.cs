using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {

    public List<GameObject> grasstiles;

    public int worldWidth;
    public int worldHeight;

    private float tileWidth;
    private float tileHeight;

    private int[,] worldMap;


	// Use this for initialization
	void Start () {
        tileWidth = grasstiles[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        tileHeight = grasstiles[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        GenerateMap();
        PrintMap();
        InstantiateMap();
    }

    private void InstantiateMap()
    {
        GameObject tileShape;
        int tileType;
        StringBuilder tileSumAsString = new StringBuilder();

        for (int map_y = 0; map_y < worldMap.GetLength(0); map_y++)
        {
            for (int map_x = 0; map_x < worldMap.GetLength(1); map_x++)
            {
                tileType = CalculateTileSum(map_x, map_y);
                tileShape = GetMapTile(worldMap[map_y, map_x], tileType);
                tileSumAsString.Append(tileType+", ");

                Instantiate(tileShape, MapToPixel(map_x, map_y), Quaternion.identity);
            }
            tileSumAsString.Append("\n");
        }

        Debug.Log(tileSumAsString.ToString());

    }

    private GameObject GetMapTile(int tile, int tileSum)
    {
        //Get map tile based on tilesum

        if (tile == 0)
        {
            return grasstiles[16];
        }

        return grasstiles[tileSum];
    }

    private int CalculateTileSum(int x, int y)
    {
        int tileSum = 0;
        //byte[,] bitmask = new byte[3, 3] { 
        //    { 7, 0, 4 },
        //    { 3, 1, 1 },
        //    { 6, 2, 5 }};

        int[] yOffset = new int[8]
        {
            -1, 0, 1, 0, -1, 1, 1, -1
        };

        int[] xOffset = new int[8]
        {
            0, 1, 0, -1, 1, 1, -1, -1
        };

        StringBuilder tileSumString = new StringBuilder();
        tileSumString.Append("Checking " + x + "," + y + "\n");

        for (int i = 0; i < 4; i++)
        {
            tileSumString.Append((x + xOffset[i]) + "," + (y + yOffset[i]) + "  ");
            if (y + yOffset[i] < 0 || y + yOffset[i] >= worldMap.GetLength(0) || x + xOffset[i] < 0 || x + xOffset[i] >= worldMap.GetLength(1))
            {
                continue;
            }

            if (worldMap[y + yOffset[i], x + xOffset[i]] == worldMap[y,x])
            {
                tileSum += 1 << i;
            }
        }

        if (tileSum == 15)
        {
            for (int i = 4; i < 8; i++)
            {
                tileSumString.Append((x + xOffset[i]) + "," + (y + yOffset[i]) + "  ");

                if (worldMap[y + yOffset[i], x + xOffset[i]] != worldMap[y, x])
                {
                    tileSum += i-2;
                    break;
                } 
            }
        }

        Debug.Log(tileSumString.ToString() +"\nSum: "+tileSum);

        return tileSum;
    }

    private void PixelToMap()
    {
        throw new NotImplementedException();
    }

    private Vector2 MapToPixel(int x, int y)
    {
        return new Vector2((x - y) * tileWidth, (x + y) * tileHeight - worldHeight / 4);
    }

    private void GenerateMap()
    {
        worldMap = new int[10, 10] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 0, 0, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 1, 1, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
    }

    private void PrintMap()
    {
        StringBuilder mapAsString = new StringBuilder();
        for (int i = 0; i < worldMap.GetLength(0); i++)
        {
            for (int j = 0; j < worldMap.GetLength(1); j++)
            {
                mapAsString.Append(worldMap[i,j]);
            }
            mapAsString.Append("\n");
        }

        Debug.Log(mapAsString.ToString());
    }
}
