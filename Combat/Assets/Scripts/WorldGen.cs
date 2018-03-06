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


    [Space]
    [Header("¨World Generation")]
    public int numberOfRooms;

    private static float tileWidth = 3f;
    private static float tileHeight = 1.5f;

    public NodeMap World { get; private set; }
    public NodeMap nodeMap;
    private Chunk[] chunks;
    private List<Coordinate> FoilagePositions = new List<Coordinate>();


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
        corruption_spawn = 7,

    }

    private void Awake()
    {

        UnityEngine.Random.InitState(42);

        GenerateMap();
        GenerateChunks();

        AddWorldChunksToMap();

        GenerateFoilage();

        nodeMap.GenerateNodeMap();
        Pathfinding.Graph = nodeMap;
    }

    // Use this for initialization
    void Start()
    {
        //Create gamobjects etc.
        InstantiateMap();
        SpawnFoilage();
    }

    public void GenerateFoilage()
    {
        int random;

        foreach (Coordinate position in nodeMap.Map.Keys)
        {
            random = UnityEngine.Random.Range(0, 100);
            if (random > 98)
            {
                FoilagePositions.Add(position);
            }
        }

        foreach (Coordinate position in FoilagePositions)
        {
            nodeMap.RemoveNode(position);
        }
    }

    private void SpawnFoilage()
    {
        var foilageContainer = new GameObject("Foilage");
        foilageContainer.transform.parent = this.gameObject.transform;

        foreach (Coordinate position in FoilagePositions)
        {
            SpawnObject(GetMapTile((int)Tiles.tree_tile), NodeMapToPixel(position), foilageContainer.transform);
        }
    }

    private void GenerateChunks()
    {
        chunks = new Chunk[numberOfRooms];
        Chunk currentChunk = new Chunk(-5, -5, 10, 10);

        Chunk lastCorruptionSpawned = currentChunk;

        chunks[0] = currentChunk;

        for (int i = 1; i < chunks.Length; i++)
        {
            currentChunk = currentChunk.AppendChunk(UnityEngine.Random.Range(4, 10), UnityEngine.Random.Range(4, 10), (Chunk.Direction)UnityEngine.Random.Range(0,4));

            if (currentChunk.chunkWidth > 8 && currentChunk.chunkWidth > 8)
            {
                if (Mathf.Abs(currentChunk.xPos - lastCorruptionSpawned.xPos) > 20 && Mathf.Abs(currentChunk.yPos - lastCorruptionSpawned.yPos) > 20)
                {
                    var node = MapToNodeMap(new Coordinate(currentChunk.xPos, currentChunk.yPos)) + new Coordinate(4, 4);
                    SpawnObject(GetMapTile((int)Tiles.corruption_spawn), NodeMapToPixel(node), gameObject.transform);
                    lastCorruptionSpawned = currentChunk;
                }

            }

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
                if (World.IsBlocked(position))
                {
                    World.AddNode(position);
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
            World.AddNode(position, 2);

            position = new Coordinate(chunk.xPos - 1, y);
            World.AddNode(position, 2);
        }

        for (int x = chunk.xPos - 1; x <= x_max; x++)
        {
            position = new Coordinate(x, y_max);
            World.AddNode(position, 2);

            position = new Coordinate(x, chunk.yPos - 1);
            World.AddNode(position, 2);
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
        var floorContainer = new GameObject("Floor");
        floorContainer.transform.parent = this.gameObject.transform;
        Vector2 position;
        GameObject tileObject;

        foreach (var node in World.Map.Values)
        {
            if (node.Tile > 0)
            {
                tileObject = GetMapTile(node.Tile);
                position = MapToPixel(node.Position.X, node.Position.Y);

                SpawnObject(tileObject, position, floorContainer.transform);
            }
        }
    }

    private GameObject SpawnObject(GameObject tileShape, Vector2 position, Transform parent = null)
    {
        GameObject createdObject = Instantiate(tileShape, position, Quaternion.identity);
        createdObject.transform.parent = parent;
        return createdObject;
    }

    private GameObject GetMapTile(int tile, int tileSum = 0)
    {
        return tiles[tile];
    }

    /*
     * Convert a screen position to a map coordinate.
     * Note: int casting should be replaced
     */
    public static Coordinate PixelToMap(float sx, float sy)
    {
        int y = (int)((sy ) / tileHeight - sx / tileWidth) / 2;
        int x = (int)(sx / tileWidth + (sy ) / tileHeight) / 2;
        return new Coordinate(x, y);
    }

    public static Coordinate PixelToNodeMap(float sx, float sy)
    {
        int y = (int)((sy / tileHeight - sx / tileWidth) * 2);
        int x = (int)((sx / tileWidth + sy / tileHeight) * 2);
        return new Coordinate(x, y);
    }

    public static Vector2 MapToPixel(float x, float y)
    {
        return new Vector2((x - y) * tileWidth, (x + y) * tileHeight);
    }

    public static Vector2 MapToPixel(Coordinate position)
    {
        return MapToPixel(position.X, position.Y);
    }

    public static Coordinate NodeMapToMap(Coordinate position)
    {
        return new Coordinate(Mathf.FloorToInt(position.X / 4), Mathf.FloorToInt(position.Y / 4));
    }

    public static Vector2 NodeMapToPixel(Coordinate position)
    {
        return new Vector2((position.X - position.Y) * tileWidth / 4, (position.X + position.Y) * tileHeight / 4);
    }

    public static Coordinate MapToNodeMap(Coordinate position)
    {
        return new Coordinate(Mathf.FloorToInt(position.X * 4), Mathf.FloorToInt(position.Y * 4));
    }

    private void GenerateMap()
    {
        World = new NodeMap();
        nodeMap = new NodeMap();

    }
}
