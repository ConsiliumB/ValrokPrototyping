using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WorldGen : MonoBehaviour {
    public List<GameObject> tiles;
    public List<GameObject> foilage;
    public List<GameObject> trees;

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
    private GameObject cliffContainer;
    private GameObject seaContainer;
    private GameObject collisionContainer;
    private List<Coordinate> FoilagePositions = new List<Coordinate>();


    public enum Tiles
    {
        blank_tile = -1,
        black_tile = 0,
        frost_tile = 1,
        collision_tile = 2,
        road_tile = 3,
        flower_tile = 4,
        tree_tile = 5,
        tree_tile_dead = 6,
        corruption_spawn = 7,
        cliff_se = 8,
        cliff_sw = 9,
        water = 10,
    }

    private void Awake()
    {
        //var seed = UnityEngine.Random.Range(0, 100);
        UnityEngine.Random.InitState(1);
        //Debug.Log("Seed = " + seed);

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
        //DrawWater();
        DrawCliffs();
        InstantiateMap();
        SpawnFoilage();
    }

    private void DrawWater()
    {
        seaContainer = new GameObject("Water");
        seaContainer.transform.parent = transform;

        foreach (Chunk chunk in chunks)
        {
            AddSea(chunk);
        }
    }

    private void AddSea(Chunk chunk)
    {
        Coordinate potentialPosition;
        Coordinate Position = new Coordinate(chunk.xPos, chunk.yPos);
        int distance = 10;
        for (int i = -distance; i < chunk.chunkWidth + distance; i++)
        {
            for (int j = -distance; j < chunk.chunkHeight + distance; j++)
            {
                potentialPosition = Position + new Coordinate(i, j);
                if (!World.WithinBounds(potentialPosition))
                {
                    SpawnObject(GetMapTile((int)Tiles.water), MapToPixel(potentialPosition), transform);
                }
            }
        }
    }

    private void DrawCliffs()
    {
        cliffContainer = new GameObject("Cliffs");
        cliffContainer.transform.parent = transform;

        foreach (Chunk chunk in chunks)
        {
            AddCliffsides(chunk);
        }
    }

    public void GenerateFoilage()
    {
        int random;

        foreach (Coordinate position in nodeMap.Map.Keys)
        {
            random = UnityEngine.Random.Range(0, 100);
            if (random > 92)
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
        int random;

        foreach (Coordinate position in FoilagePositions)
        {
            random = UnityEngine.Random.Range(0, 100);
            if (random > 90)
            {
                SpawnObject(trees[UnityEngine.Random.Range(0, trees.Count)], NodeMapToPixel(position), foilageContainer.transform);
            }
            else
            {
                SpawnObject(foilage[UnityEngine.Random.Range(0, foilage.Count)], NodeMapToPixel(position), foilageContainer.transform);
            }
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

        collisionContainer = new GameObject("Cliff Collision");
        collisionContainer.transform.parent = transform;

        //Add collision around edge of map
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


        Coordinate position;
        for (int y = chunk.yPos - 1; y <= y_max; y++)
        {
            position = new Coordinate(x_max, y);
            if (!World.WithinBounds(position))
            {
                SpawnObject(GetMapTile((int)Tiles.collision_tile), MapToPixel(position), collisionContainer.transform);
            }

            position = new Coordinate(chunk.xPos - 1, y);
            if (!World.WithinBounds(position))
            {
                SpawnObject(GetMapTile((int)Tiles.collision_tile), MapToPixel(position), collisionContainer.transform);
            }
        }

        for (int x = chunk.xPos - 1; x <= x_max; x++)
        {
            position = new Coordinate(x, y_max);
            if (!World.WithinBounds(position))
            {
                SpawnObject(GetMapTile((int)Tiles.collision_tile), MapToPixel(position), collisionContainer.transform);
            }

            position = new Coordinate(x, chunk.yPos - 1);
            if (!World.WithinBounds(position))
            {
                SpawnObject(GetMapTile((int)Tiles.collision_tile), MapToPixel(position), collisionContainer.transform);
            }
        }
    }

    private void AddCliffsides(Chunk chunk)
    {
        int y_max = chunk.yPos + chunk.chunkHeight - 1;
        int x_max = chunk.xPos + chunk.chunkWidth - 1;


        Coordinate position;
        for (int y = chunk.yPos - 1; y < y_max; y++)
        {
            position = new Coordinate(chunk.xPos - 1, y);
            if (!World.WithinBounds(position + Coordinate.North))
            {
                SpawnObject(GetMapTile((int)Tiles.cliff_sw), MapToPixel(position), cliffContainer.transform);
            }
        }

        for (int x = chunk.xPos - 1; x < x_max; x++)
        {
            position = new Coordinate(x, chunk.yPos - 1);
            if (!World.WithinBounds(position + Coordinate.East))
            {
                SpawnObject(GetMapTile((int)Tiles.cliff_se), MapToPixel(position), cliffContainer.transform);
            }
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

        //int actualTile;

        foreach (var node in World.Map.Values)
        {
            if (node.Tile > 0)
            {
                //if (node.Tile == 1)
                //{
                //    actualTile = UnityEngine.Random.Range(11, 13);

                //    tileObject = GetMapTile(actualTile);

                //}
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

    public static Coordinate PixelToNodeMap(Vector2 innPosition)
    {
        return PixelToNodeMap(innPosition.x, innPosition.y);
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
