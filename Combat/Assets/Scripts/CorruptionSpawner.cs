using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionSpawner : StatefulEntity {

    bool spawnMode;
    
    List<Coordinate> SpawnPoints { get; set; }
    public GameObject enemy;

    public readonly float spawnDelay;
    private float spawnCounter = 0;
    bool spawnEnemies = false;

    private bool restartOnce = false;

    // Use this for initialization
    void Start () {
        SpawnPoints = new List<Coordinate>();
        FindSpawnpoints();
	}

    private void Update()
    {
        if (spawnEnemies)
        {
            spawnCounter += Time.deltaTime;
            if (spawnCounter >= spawnDelay)
            {
                Debug.Log("Spawn!");
                StartSpawn();
                spawnEnemies = false;
            }
        }
    }

    private void ReachedCenter()
    {
        if (!restartOnce) { 
            GameDirector.Restart();
            restartOnce = true;
        }
    }

    private void FindSpawnpoints()
    {
        Coordinate potentialPosition;
        int distance = 60;
        for (int i = 0; i < distance; i++)
        {
            potentialPosition = Position + new Coordinate(i, distance - i);
            if (Pathfinding.Graph.IsOpen(potentialPosition))
            {
                SpawnPoints.Add(potentialPosition);
            }

            potentialPosition = Position + new Coordinate(i * -1, distance - i);
            if (Pathfinding.Graph.IsOpen(potentialPosition))
            {
                SpawnPoints.Add(potentialPosition);
            }

            potentialPosition = Position - new Coordinate(i, distance - i);
            if (Pathfinding.Graph.IsOpen(potentialPosition))
            {
                SpawnPoints.Add(potentialPosition);
            }

            potentialPosition = Position - new Coordinate(i * -1, distance - i);
            if (Pathfinding.Graph.IsOpen(potentialPosition))
            {
                SpawnPoints.Add(potentialPosition);
            }
        }
    }

    //start creating enemies after a delay
    private void StartSpawn()
    {
        Coordinate spawnPoint;
        for (int i = 0; i < 5; i++)
        {
            spawnPoint = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)];
            Instantiate(enemy, WorldGen.NodeMapToPixel(spawnPoint), Quaternion.identity).transform.parent = transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spawnEnemies = true;

            var player = collision.gameObject;
            player.GetComponent<PlayerController>().StopMoving();
            Pathfinding.Companion.GetComponent<CompanionController>().takeOver = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ReachedCenter();
        }
    }
}
