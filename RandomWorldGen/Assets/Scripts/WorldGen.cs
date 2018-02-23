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
    public int numberOfRooms;

    private float tileWidth;
    private float tileHeight;

    public GameObject foilageContainer;
    public GameObject roadContainer;
    public GameObject floorContainer;

    private int[,] worldMap;
    private NodeMap world;
    public NodeMap nodeMap;
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

    private void Awake()
    {
        tileWidth = tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        tileHeight = tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        UnityEngine.Random.InitState(42);

        GenerateMap();
        GenerateChunks();
        //GenerateRoads();

        AddWorldChunksToMap();
        nodeMap.GenerateNodeMap();
    }

    // Use this for initialization
    void Start()
    {
        //Create gamobjects etc.
        InstantiateMap();
        PopulateUtils(roads, roadContainer.transform);
        PopulateUtils(foilage, foilageContainer.transform);
    }

    private void GenerateChunks()
    {
        chunks = new Chunk[numberOfRooms];
        Chunk currentChunk = new Chunk(-5, -5, 10, 10);
        

        chunks[0] = currentChunk;

        for (int i = 1; i < chunks.Length; i++)
        {
            currentChunk = currentChunk.AppendChunk(UnityEngine.Random.Range(4, 10), UnityEngine.Random.Range(4, 10), (Chunk.Direction)UnityEngine.Random.Range(0,4));
            chunks[i] = currentChunk;
        }
    }

    private void AddWorldChunksToMap()
    {
        foreach (Chunk chunk in chunks)
        {
            AddWorldChunk(chunk);
        }

        //Add border to each room without overwriting. Add within previous loop to see room edges
        foreach (Chunk chunk in chunks)
        {
            AddWorldChunkBorder(chunk);
        }
    }

    private void AddWorldChunk(Chunk chunk)
    {
        //Also need to add metadata/mob spawns/foilage etc from the patch
        Coordinate position;
        for (int map_y = chunk.yPos; map_y < chunk.yPos + chunk.chunkHeight; map_y++)
        {
            for (int map_x = chunk.xPos; map_x < chunk.xPos + chunk.chunkWidth; map_x++)
            {
                position = new Coordinate(map_x, map_y);
                if (world.IsBlocked(position))
                {
                    world.AddNode(position);
                    AddNodeGrid(position);
                }
            }
        }
    }

    private void AddNodeGrid(Coordinate coordinate)
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                nodeMap.AddNode(coordinate * 4 + new Coordinate(x, y));
            }
        }
    }

    private void AddWorldChunkBorder(Chunk chunk)
    {
        int y_max = chunk.yPos + chunk.chunkHeight;
        int x_max = chunk.xPos + chunk.chunkWidth;

        //Run AddNodeGrid(position) on all 4 directions if border is traversable


        Coordinate position;
        for (int y = chunk.yPos - 1; y <= y_max; y++)
        {
            position = new Coordinate(x_max, y);
            world.AddNode(position, 2);

            position = new Coordinate(chunk.xPos - 1, y);
            world.AddNode(position, 2);
        }

        for (int x = chunk.xPos - 1; x <= x_max; x++)
        {
            position = new Coordinate(x, y_max);
            world.AddNode(position, 2);

            position = new Coordinate(x, chunk.yPos - 1);
            world.AddNode(position, 2);
        }
    }

    private void GenerateRoads()
    {
        Vector2 position;
        for (int y = 0; y < worldMap.GetLength(0); y++)
        {
            position = MapToPixel(50, y);
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
        Vector2 position;
        GameObject tileObject;

        foreach (var node in world.Map.Values)
        {
            if (node.Tile > 0)
            {
                tileObject = GetMapTile(node.Tile);
                position = MapToPixel(node.Position.X, node.Position.Y);

                SpawnObject(tileObject, position, floorContainer.transform);
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

    /*
     * Convert a screen position to a map coordinate.
     * Note: int casting should be replaced
     */
    public Coordinate PixelToMap(float sx, float sy)
    {
        int y = (int)((sy ) / tileHeight - sx / tileWidth) / 2;
        int x = (int)(sx / tileWidth + (sy ) / tileHeight) / 2;
        return new Coordinate(x, y);
    }

    public Coordinate PixelToNodeMap(float sx, float sy)
    {
        int y = (int)((sy / tileHeight - sx / tileWidth) * 2);
        int x = (int)((sx / tileWidth + sy / tileHeight) * 2);
        return new Coordinate(x, y);
    }

    public Vector2 MapToPixel(int x, int y)
    {
        return new Vector2((x - y) * tileWidth, (x + y) * tileHeight);
    }

    public Vector2 MapToPixel(Coordinate t)
    {
        return new Vector2((t.X - t.Y) * tileWidth, (t.X + t.Y) * tileHeight);
    }

    public Coordinate NodeMapToMap(Coordinate position)
    {
        return new Coordinate(Mathf.FloorToInt(position.X / 4), Mathf.FloorToInt(position.Y / 4));
    }

    public Vector2 NodeMapToPixel(Coordinate position)
    {
        return new Vector2((position.X - position.Y) * tileWidth / 4, (position.X + position.Y) * tileHeight / 4);
    }

    private void GenerateMap()
    {
        world = new NodeMap();
        nodeMap = new NodeMap();
    }

    public Map GetMap()
    { 
        return new GridMap(worldMap);
    }

}
