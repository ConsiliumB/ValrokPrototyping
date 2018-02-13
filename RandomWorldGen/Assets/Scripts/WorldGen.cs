using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WorldGen : MonoBehaviour {
    public List<GameObject> tiles;

    [System.Serializable]
    public struct Util
    {
        public float xpos;
        public float ypos;
        public Tiles tile;

        public Util(float xpos, float ypos, Tiles tile)
        {
            this.xpos = xpos;
            this.ypos = ypos;
            this.tile = tile;
        }
    }

    [Header("Other stuff List")]
    public List<Util> foilage;
    public List<Util> roads;

    [Space]
    public int worldWidth;
    public int worldHeight;

    private float tileWidth;
    private float tileHeight;

    public GameObject foilageContainer;
    public GameObject roadContainer;
    public GameObject floorContainer;

    private int[,] worldMap;
    private Chunk[] chunks;


    public enum Tiles
    {
        blank_tile = -1,
        black_tile = 0,
        frost_tile = 1,
        grass_tile = 2,
        road_tile = 3,
        flower_tile = 4,
        tree_tile = 5,
        tree_tile_dead = 6,
    }

    // Use this for initialization
    void Start()
    {
        tileWidth = tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        tileHeight = tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        UnityEngine.Random.InitState(1337);

        GenerateChunks();
        GenerateMap();
        GenerateRoads();

        AddWorldChunksToMap();

        PrintMap();

        InstantiateMap();
        PopulateUtils(roads, roadContainer.transform);
        PopulateUtils(foilage, foilageContainer.transform);
    }

    private void GenerateChunks()
    {
        chunks = new Chunk[20];
        Chunk currentChunk = new Chunk(50, 50, 10, 10);
        

        chunks[0] = currentChunk;

        for (int i = 1; i < chunks.Length; i++)
        {
            currentChunk = currentChunk.AppendChunk(UnityEngine.Random.Range(2, 10), UnityEngine.Random.Range(2, 10), (Chunk.Direction)UnityEngine.Random.Range(0,4));
            chunks[i] = currentChunk;
        }
    }

    private void AddWorldChunksToMap()
    {
        //Seed should determine locations/types/shapes
        foreach (Chunk chunk in chunks)
        {
            AddWorldChunk(chunk);
        }
    }

    private void AddWorldChunk(Chunk chunk)
    {
        //Also need to add metadata/mob spawns/foilage etc from the patch

        for (int map_y = chunk.yPos; map_y < chunk.yPos + chunk.chunkHeight; map_y++)
        {
            for (int map_x = chunk.xPos; map_x < chunk.xPos + chunk.chunkWidth; map_x++)
            {
                worldMap[map_y, map_x] = 1;
            }
        }
    }

    private void GenerateRoads()
    {
        Vector2 position;
        for (int y = 0; y < worldMap.GetLength(0); y++)
        {
            position = MapToPixel(5, y);
            roads.Add(new Util(position.x, position.y, Tiles.road_tile));
        }
    }

    private void PopulateUtils(List<Util> utils, Transform parent = null)
    {
        foreach (Util item in utils)
        {
            SpawnObject(GetMapTile((int)item.tile), new Vector2(item.xpos, item.ypos), parent);
        }
    }

    private void InstantiateMap()
    {
        int tileType;
        //int tileVariation;
        Vector2 position;
        GameObject tileObject;

        for (int map_y = 0; map_y < worldMap.GetLength(0); map_y++)
        {
            for (int map_x = 0; map_x < worldMap.GetLength(1); map_x++)
            {
                //tileVariation = CalculateTileSum(map_x, map_y);
                tileType = worldMap[map_y, map_x];
                if (tileType >= 0)
                {
                    tileObject = GetMapTile(tileType);
                    position = MapToPixel(map_x, map_y);

                    SpawnObject(tileObject, position, floorContainer.transform);
                }
            }
        }
    }

    private void SpawnObject(GameObject tileShape, Vector2 position, Transform parent = null)
    {
        Instantiate(tileShape, position, Quaternion.identity).transform.parent = parent;
    }

    private GameObject GetMapTile(int tile, int tileSum = 0)
    {
        //Get map tile based on tilesum

        //if (tile == 0)
        //{
        //    return grasstiles[16];
        //}

        return tiles[tile];
    }

    private int CalculateTileSum(int x, int y)
    {
        int tileSum = 0;

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

            if (worldMap[y + yOffset[i], x + xOffset[i]] == worldMap[y, x])
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
                    tileSum += i - 2;
                    break;
                }
            }
        }

        Debug.Log(tileSumString.ToString() + "\nSum: " + tileSum);

        return tileSum;
    }

    private void PixelToMap()
    {
        throw new NotImplementedException();
    }

    private Vector2 MapToPixel(int x, int y)
    {
        return new Vector2((x - y) * tileWidth, (x + y) * tileHeight - (worldHeight * tileHeight));
    }

    private void GenerateMap()
    {
        worldMap = new int[worldWidth, worldHeight];
    }

    private void PrintMap()
    {
        StringBuilder mapAsString = new StringBuilder();
        for (int i = worldMap.GetLength(1) - 1; i >= 0; i--)
        {
            for (int j = 0; j < worldMap.GetLength(0); j++)
            {
                mapAsString.Append(worldMap[i, j]);
            }
            mapAsString.Append("\n");
        }

        Debug.Log(mapAsString.ToString());
    }
}
