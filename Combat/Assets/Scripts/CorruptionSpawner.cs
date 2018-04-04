using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CorruptionSpawner : StatefulEntity {

    bool spawnMode;
    
    List<Coordinate> SpawnPoints { get; set; }
    public GameObject enemyPrefab;
    public int amountSpawned;
    public readonly float spawnDelay;

    private float spawnCounter = 0;
    private int enemyCount = 0;
    bool spawnEnemies = false;
    bool spawnerHit = false;

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
                Debug.Log("Spawn slimes");
                StartSpawn();
                spawnEnemies = false;
            }
            
        }
        if (spawnerHit && enemyCount == 0)
        {
            DespawnPortal();
        }
    }

    private void DespawnPortal()
    {
        gameObject.SetActive(false);
        UnlockPlayerControll();
    }

    private void ReachedCenter()
    {
        if (!restartOnce) { 
            //GameDirector.Restart();
            restartOnce = true;
        }
    }

    //Change if there is a better way to keep track of enemies spawned
    public void EnemyDied()
    {
        enemyCount--;
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
        for (int i = 0; i < amountSpawned; i++)
        {
            spawnPoint = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)];
            Instantiate(enemyPrefab, WorldGen.NodeMapToPixel(spawnPoint), Quaternion.identity, transform);
            enemyCount++;
        }
    }

    //Unlocks movement of the companion and player 
    private void UnlockPlayerControll()
    {
        PlayerController.Instance.UnlockPlayer();
        CompanionController.Instance.RestartCompanion();
    }

    //Locks player in place and takes over the bear with player controll
    private void LockPlayerControll()
    {
        spawnEnemies = true;
        PlayerController.Instance.LockPlayer();
        CompanionController.Instance.ChangeToTakeover();
    }

    //Unsure if this should be on the player on corruptionSpawner
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !spawnerHit)
        {
            spawnerHit = true;
            LockPlayerControll();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ReachedCenter();
        }
    }
}
